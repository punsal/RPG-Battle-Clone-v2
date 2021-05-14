using System;
using System.Collections;
using System.Threading.Tasks;
using UnityEngine;

namespace UI_System.Scripts.Menu.Abstract
{
    [RequireComponent(typeof(CanvasGroup))]
    public abstract class MenuBase : MonoBehaviour
    {
        [Serializable]
        public class Settings
        {
            [SerializeField] private float appearDuration = 0.5f;
            public float AppearDuration => appearDuration;
        }

        [Header("Menu Settings")]
        [SerializeField] private Settings settings;
        
        private CanvasGroup _canvasGroup;
        protected CanvasGroup canvasGroup => _canvasGroup == null 
            ? _canvasGroup = GetComponent<CanvasGroup>() 
            : _canvasGroup;

        private Coroutine actionCoroutine;

        public void Show()
        {
            if (actionCoroutine != null)
            {
                StopCoroutine(actionCoroutine);
            }
            actionCoroutine = StartCoroutine(Appear());
        }

        public void Hide()
        {
            if (actionCoroutine != null)
            {
                StopCoroutine(actionCoroutine);
            }
            actionCoroutine = StartCoroutine(Disappear());
        }
        
        private IEnumerator Appear()
        {
            var timer = 0f;
            while (timer < settings.AppearDuration)
            {
                canvasGroup.alpha = timer / settings.AppearDuration;
                yield return null;
                timer += Time.deltaTime;
            }

            canvasGroup.alpha = 1f;
            
            OnAppeared();
        }

        private IEnumerator Disappear()
        {
            var timer = 0f;
            while (timer < settings.AppearDuration)
            {
                canvasGroup.alpha = 1 - timer / settings.AppearDuration;
                yield return null;
                timer += Time.deltaTime;
            }

            canvasGroup.alpha = 0f;
            
            OnDisappeared();
        }

        protected abstract void OnAppeared();
        protected abstract void OnDisappeared();
    }
}
