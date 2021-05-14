using System;
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

        public void Show()
        {
            Task.Run(Appear);
        }

        public void Hide()
        {
            Task.Run(Disappear);
        }
        
        private async Task Appear()
        {
            var timer = 0f;
            while (timer < settings.AppearDuration)
            {
                canvasGroup.alpha = timer / settings.AppearDuration;
                await Task.Delay(TimeSpan.FromMilliseconds(20));
                timer += (float) TimeSpan.FromMilliseconds(20).TotalSeconds;
            }

            canvasGroup.alpha = 1f;
            
            OnAppeared();
        }

        private async Task Disappear()
        {
            var timer = 0f;
            while (timer < settings.AppearDuration)
            {
                canvasGroup.alpha = 1 - timer / settings.AppearDuration;
                await Task.Delay(TimeSpan.FromMilliseconds(20));
                timer += (float) TimeSpan.FromMilliseconds(20).TotalSeconds;
            }

            canvasGroup.alpha = 0f;
            
            OnDisappeared();
        }

        protected abstract void OnAppeared();
        protected abstract void OnDisappeared();
    }
}
