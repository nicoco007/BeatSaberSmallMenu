using HMUI;
using System;
using System.ComponentModel;
using UnityEngine;
using Zenject;
using Logger = IPA.Logging.Logger;

namespace SmallMenu
{
    internal class MenuScaleController : IInitializable, IDisposable
    {
        private Logger _logger;
        private Settings _settings;
        private HierarchyManager _hierarchyManager;

        private Transform _bottomScreen;

        [Inject]
        public void Construct(Logger logger, Settings settings, HierarchyManager hierarchyManager)
        {
            _logger = logger;
            _settings = settings;
            _hierarchyManager = hierarchyManager;
        }

        public void Initialize()
        {
            _bottomScreen = _hierarchyManager.transform.Find("BottomScreen");

            if (!_bottomScreen)
            {
                _logger.Error("BottomScreen transform not found!");
                return;
            }

            _settings.PropertyChanged += OnSettingsPropertyChanged;

            UpdateScale();
        }

        public void Dispose()
        {
            _settings.PropertyChanged -= OnSettingsPropertyChanged;
        }

        private void OnSettingsPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName == nameof(Settings.scale))
            {
                UpdateScale();
            }
        }

        private void UpdateScale()
        {
            if (!_bottomScreen || _settings.scale <= 0) return;

            // menu screen (scale 1.0) was originally at y = 1.35 and at scale 1.5 it is at y = 0.85
            // therefore, at scale 1.0, the screen system must be moved up by y += 0.5
            float offset = Mathf.Max(0, 1.5f - 1f * _settings.scale);

            Vector3 screenSystemPosition = _hierarchyManager.transform.localPosition;
            _hierarchyManager.transform.localPosition = new Vector3(screenSystemPosition.x, offset, screenSystemPosition.z);
            _hierarchyManager.transform.localScale = Vector3.one * _settings.scale;

            Vector3 bottomScreenPosition = _bottomScreen.localPosition;
            _bottomScreen.localPosition = new Vector3(bottomScreenPosition.x, (0.02f - offset) / _settings.scale, bottomScreenPosition.z);
        }
    }
}
