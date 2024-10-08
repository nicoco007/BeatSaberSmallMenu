using BeatSaberMarkupLanguage.Attributes;
using BeatSaberMarkupLanguage.Components.Settings;
using BeatSaberMarkupLanguage.Settings;
using System;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

namespace SmallMenu
{
    internal class SettingsHost : IInitializable, IDisposable
    {
        [UIValue("scale-options")]
        internal readonly List<object> scaleOptions = new List<object> { MenuScale.Big, MenuScale.Default, MenuScale.Small, MenuScale.Tiny, MenuScale.Custom };

        [UIObject("custom-scale")]
        internal GameObject customScaleObject;

        [UIComponent("custom-scale")]
        internal SliderSetting customScaleSetting;

        private readonly Settings _settings;
        private readonly BSMLSettings _bsmlSettings;

        public SettingsHost(Settings settings, BSMLSettings bsmlSettings)
        {
            _settings = settings;
            _bsmlSettings = bsmlSettings;
        }

        [UIValue("custom-scale-active")]
        internal bool customScaleActive
        {
            get => scale == MenuScale.Custom;
        }

        [UIValue("custom-scale-value")]
        internal float customScale
        {
            get => _settings.scale;
            set
            {
                _settings.scale = value;
            }
        }

        [UIValue("scale-value")]
        internal MenuScale scale
        {
            get
            {
                if (Mathf.Approximately(_settings.scale, 1.5f))
                {
                    return MenuScale.Default;
                }
                else if (Mathf.Approximately(_settings.scale, 2f))
                {
                    return MenuScale.Big;
                }
                else if (Mathf.Approximately(_settings.scale, 1f))
                {
                    return MenuScale.Small;
                }
                else if (Mathf.Approximately(_settings.scale, 0.5f))
                {
                    return MenuScale.Tiny;
                }
                else
                {
                    return MenuScale.Custom;
                }
            }
            set
            {
                switch (value)
                {
                    case MenuScale.Default:
                        _settings.scale = 1.5f;
                        break;

                    case MenuScale.Big:
                        _settings.scale = 2f;
                        break;

                    case MenuScale.Small:
                        _settings.scale = 1f;
                        break;

                    case MenuScale.Tiny:
                        _settings.scale = 0.5f;
                        break;
                }

                customScaleObject.SetActive(value == MenuScale.Custom);
                customScaleSetting.Value = _settings.scale;
            }
        }

        public void Initialize()
        {
            _bsmlSettings.AddSettingsMenu("SmallMenu", "SmallMenu.UI.Settings.bsml", this);
        }

        public void Dispose()
        {
            _bsmlSettings.RemoveSettingsMenu(this);
        }

        [UIAction("scale-formatter")]
        internal string ScaleFormatter(MenuScale value)
        {
            switch (value)
            {
                case MenuScale.Default:
                    return "Default";

                case MenuScale.Big:
                    return "Big";

                case MenuScale.Small:
                    return "Small";

                case MenuScale.Tiny:
                    return "Tiny";

                case MenuScale.Custom:
                    return "Custom";

                default:
                    return string.Empty;
            }
        }
    }
}
