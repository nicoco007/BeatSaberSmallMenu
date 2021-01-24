using BeatSaberMarkupLanguage.Attributes;
using BeatSaberMarkupLanguage.Settings;
using System;
using Zenject;

namespace SmallMenu
{
    internal class SettingsHost : IInitializable, IDisposable
    {
        [UIValue("scale")]
        public float scale
        {
            get => _settings.scale;
            set => _settings.scale = value;
        }

        private readonly Settings _settings;

        public SettingsHost(Settings settings)
        {
            _settings = settings;
        }

        public void Initialize()
        {
            BSMLSettings.instance.AddSettingsMenu("SmallMenu", "SmallMenu.UI.Settings.bsml", this);
        }

        public void Dispose()
        {
            if (BSMLSettings.instance)
            {
                BSMLSettings.instance.RemoveSettingsMenu(this);
            }
        }
    }
}
