using Framework.ServiceLocator.Signals.Interface;
using UI_System.Scripts.Popup.Abstract;

namespace UI_System.Scripts.Popup.Signals
{
    public class OnPopupClosedSignal : ISignal<OnPopupClosedSignal.PopupClosed>
    {
        public delegate void PopupClosed(PopupBase popup);

        private event PopupClosed OnPopupClosed;

        public void RaiseOnPopupClosed(PopupBase popup)
        {
            OnPopupClosed?.Invoke(popup);
        }

        public void AddListener(PopupClosed action)
        {
            OnPopupClosed += action;
        }

        public void RemoveListener(PopupClosed action)
        {
            OnPopupClosed -= action;
        }
    }
}