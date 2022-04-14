using System.Collections;
using System.Linq;
using Framework.ServiceLocator.Signals;
using Framework.ServiceLocator.Signals.Interface;
using UnityEngine;

namespace Game.Battle.Scripts.Handlers
{
    public class InputHandler : MonoBehaviour
    {
        // ReSharper disable once ClassNeverInstantiated.Global
        public class OnClickSignal : ISignal<OnClickSignal.Click>
        {
            public delegate void Click(Vector2 position);
            private event Click OnClick;

            public void RaiseOnClick(Vector2 position)
            {
                var message = OnClick.GetInvocationList().Aggregate("", (current, invoker) => current + (invoker.Method.Name + " - "));
                Debug.Log($"CurrentListeners: {message}");
                OnClick?.Invoke(position);
            }

            public void AddListener(Click action)
            {
                OnClick = action;
            }

            public void RemoveListener(Click action)
            {
                //Do nothing.
            }
        }
        
        // ReSharper disable once ClassNeverInstantiated.Global
        public class OnHoldSignal : ISignal<OnHoldSignal.Hold>
        {
            public delegate void Hold(Vector2 position);
            private event Hold OnHold;

            public void RaiseOnHold(Vector2 position)
            {
                OnHold?.Invoke(position);
            }

            public void AddListener(Hold action)
            {
                OnHold += action;
            }

            public void RemoveListener(Hold action)
            {
                OnHold -= action;
            }
        }

        private Coroutine _inputCoroutine;

        public void StartInputHandling()
        {
            if (_inputCoroutine != null) return;

            _inputCoroutine = StartCoroutine(HandleInput());
        }

        public void StopInputHandling()
        {
            if (_inputCoroutine == null) return;

            StopCoroutine(_inputCoroutine);
            _inputCoroutine = null;
        }
        
        private IEnumerator HandleInput()
        {
            while (true)
            {
                yield return new WaitUntil(() => Input.GetMouseButtonDown(0));
                var initialInputPosition = Input.mousePosition;
                var currentInputPosition = initialInputPosition;
                var timer = 0f;
                while (true)
                {
                    if (Input.GetMouseButtonUp(0))
                    {
                        if (timer > 2f)
                        {
                            SignalServices.GetEvent<InputHandler.OnHoldSignal>().RaiseOnHold(currentInputPosition);
                            break;
                        }

                        SignalServices.GetEvent<InputHandler.OnClickSignal>().RaiseOnClick(initialInputPosition);
                        break;
                    }

                    timer += Time.deltaTime;
                    currentInputPosition = Input.mousePosition;
                    yield return null;
                }
            }
        }
    }
}