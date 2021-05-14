using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace UI_System.Scripts.Popup.Abstract
{
    public abstract class PopupBase : MonoBehaviour
    {
        [Header("Base UI Elements")]
        [SerializeField] private Transform contentsTransform;
        [SerializeField] private Button buttonClose;

        private UnityAction _onCloseAction;

        protected Transform ContentsTransform => contentsTransform;
        
        private void OnEnable()
        {
            RegisterCloseAction(Close);
            Initialize();
        }

        private void OnDisable()
        {
            UnregisterCloseAction(Close);
            Terminate();
        }

        private void Initialize()
        {
            buttonClose.onClick.AddListener(_onCloseAction);
            OnInitialized();
        }

        private void Terminate()
        {
            if (_onCloseAction != null)
            {
                buttonClose.onClick.RemoveListener(_onCloseAction);
            }
            
            OnTerminated();
        }

        private void Close()
        {
            Destroy(gameObject);
        }

        protected void RegisterCloseAction(UnityAction action)
        {
            _onCloseAction += action;
        }

        protected void UnregisterCloseAction(UnityAction action)
        {
            _onCloseAction -= action;
        }

        protected abstract void OnInitialized();
        protected abstract void OnTerminated();
    }
}
