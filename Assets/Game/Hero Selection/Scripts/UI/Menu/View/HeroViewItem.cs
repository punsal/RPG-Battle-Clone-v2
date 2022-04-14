using System.Collections;
using Framework.ServiceLocator.Signals;
using Framework.ServiceLocator.Systems;
using Game.Hero_Selection.Scripts.UI.Menu.View.Signals;
using Game.Hero_Selection.Scripts.UI.Model;
using Game.Hero_Selection.Scripts.UI.Popup;
using UI_System.Scripts;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Game.Hero_Selection.Scripts.UI.Menu.View
{
    public class HeroViewItem : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
    {
        [Header("Model")]
        [SerializeField] private CharacterViewModel viewModel;

        [Header("UI Elements")]
        [SerializeField] private Image imageHeroIcon;
        [SerializeField] private Image imageSelectionBackground;

        private bool _isClicked = false;
        
        public void ShowView(CharacterViewModel characterViewModel)
        {
            viewModel = characterViewModel;
            imageHeroIcon.sprite = viewModel.CharacterSprite;
            Select(viewModel.ViewStatues.IsSelected);
        }

        private void ShowAttributes()
        {
            var popupManager = SystemServices.GetSystem<UISystem>().GetManager<HeroSelectionPopupManager>();
            popupManager.Show<CharacterAttributePopup>();
            var popup = popupManager.GetPopup<CharacterAttributePopup>();
            popup.Construct(viewModel.CharacterModel);
        } 
        
        public void Select(bool isSelected)
        {
            viewModel.ViewStatues.Select(isSelected);
            viewModel.Save();

            imageSelectionBackground.color = isSelected ? Color.green : Color.black;
            SignalServices.GetEvent<OnHeroViewSelectedSignal>().RaiseOnHeroViewSelected(this, isSelected);
        }

        public void OnPointerDown(PointerEventData eventData)
        {
            _isClicked = true;
            StartCoroutine(ClickDetection());
        }

        public void OnPointerUp(PointerEventData eventData)
        {
            _isClicked = false;
        }

        private IEnumerator ClickDetection()
        {
            var timer = 0f;
            while (_isClicked)
            {
                timer += Time.deltaTime;
                yield return null;
            }

            if (timer > 2f)
            {
                ShowAttributes();
            }
            else
            {
                Select(!viewModel.ViewStatues.IsSelected);
            }
        }
    }
}