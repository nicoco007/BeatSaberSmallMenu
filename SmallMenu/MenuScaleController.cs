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

        private GameObject _screenSystem;
        private Transform _bottomScreen;

        [Inject]
        public void Construct(Logger logger, Settings settings)
        {
            _logger = logger;
            _settings = settings;
        }

        public void Initialize()
        {
            _screenSystem = GameObject.Find("Wrapper/ScreenSystem");

            if (!_screenSystem)
            {
                _logger.Error("ScreenSystem GameObject not found!");
                return;
            }

            _bottomScreen = _screenSystem.transform.Find("BottomScreen");

            if (!_bottomScreen)
            {
                _logger.Error("ScreenSystem GameObject not found!");
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
            if (!_screenSystem || !_bottomScreen || _settings.scale <= 0) return;

            // UI center is approximately 0.85 m up
            float offset = Mathf.Max(0, 0.85f - 0.85f / 1.5f * _settings.scale);

            Vector3 screenSystemPosition = _screenSystem.transform.localPosition;
            _screenSystem.transform.localPosition = new Vector3(screenSystemPosition.x, offset, screenSystemPosition.z);
            _screenSystem.transform.localScale = Vector3.one * _settings.scale;

            Vector3 bottomScreenPosition = _bottomScreen.localPosition;
            _bottomScreen.localPosition = new Vector3(bottomScreenPosition.x, (0.02f - offset) / _settings.scale, bottomScreenPosition.z);
        }
    }
}
