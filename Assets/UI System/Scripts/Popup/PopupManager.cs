using System;
using System.Collections.Generic;
using System.Linq;
using Framework.Abstract;
using Framework.ServiceLocator.Signals;
using UI_System.Scripts.Popup.Abstract;
using UI_System.Scripts.Popup.Settings;
using UI_System.Scripts.Popup.Signals;
using UnityEngine;

namespace UI_System.Scripts.Popup
{
    public abstract class PopupManager : MonoManager
    {
        [Header("Settings")]
        [SerializeField] private PopupManagerSettings settings;
        public PopupManagerSettings Settings => settings;

        private List<PopupBase> _instantiatedPopups;
        
        public override void Initialize()
        {
            _instantiatedPopups = new List<PopupBase>();
            SignalServices.GetEvent<OnPopupClosedSignal>().AddListener(OnPopupClosedHandler);
        }

        private void OnDisable()
        {
            SignalServices.GetEvent<OnPopupClosedSignal>().RemoveListener(OnPopupClosedHandler);
        }

        public void Show<T>(bool isSingle = true) where T : PopupBase
        {
            if (isSingle)
            {
                foreach (var instantiatedPopup in _instantiatedPopups)
                {
                    instantiatedPopup.Close();
                }

                _instantiatedPopups.Clear();
            }
            
            var selectedPopup = Settings.popups.FirstOrDefault(menuBase => menuBase.GetType() == typeof(T));
            if (selectedPopup == null)
            {
                Debug.LogError($"Could not find {typeof(T).Name} in settings.");
                return;
            }
            var popup = Instantiate(selectedPopup, transform);
            _instantiatedPopups.Add(popup);
        }

        public T GetPopup<T>() where T : PopupBase
        {
            var popup = _instantiatedPopups.FirstOrDefault(popupBase => popupBase.GetType() == typeof(T));
            if (popup == null)
            {
                throw new Exception($"{typeof(T).Name} does not exist in {GetType().Name}");
            }

            return (T) popup;
        }

        private void OnPopupClosedHandler(PopupBase popup)
        {
            if (!_instantiatedPopups.Exists(popupBase => popupBase.GetType() == popup.GetType())) return;
            {
                var existingPopup = 
                    _instantiatedPopups.FirstOrDefault(popupBase => popupBase.GetType() == popup.GetType());
                if (existingPopup != null)
                {
                    _instantiatedPopups.Remove(existingPopup);
                }
            }
        }
    }
}
