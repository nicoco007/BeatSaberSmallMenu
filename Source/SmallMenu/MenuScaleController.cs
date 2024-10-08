﻿using HMUI;
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
        private ScreenModeController _screenModeController;

        private Transform _bottomScreen;

        [Inject]
        public void Construct(Logger logger, Settings settings, HierarchyManager hierarchyManager, ScreenModeController screenModeController)
        {
            _logger = logger;
            _settings = settings;
            _hierarchyManager = hierarchyManager;
            _screenModeController = screenModeController;
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
            if (_settings.scale <= 0) return;

            ScreenModeData screenModeData = _screenModeController._defaultModeData;

            // menu screen (scale 1.0) was originally at y = 1.35 and at scale 1.5 it is at y = 0.85
            // therefore, at scale 1.0, the screen system must be moved up by y += 0.5
            float offset = Mathf.Max(0, 1.5f - 1f * _settings.scale);

            screenModeData.position.y = offset;
            screenModeData.scale = _settings.scale;

            _screenModeController.SetDefaultMode();

            if (!_bottomScreen) return;

            Vector3 bottomScreenPosition = _bottomScreen.position;
            bottomScreenPosition.y = 0.02f;
            _bottomScreen.position = bottomScreenPosition;
        }
    }
}
