using System.Collections.Generic;
using System.Linq;
using Framework.ServiceLocator.Systems;
using Game.Common.Scripts.Managers;
using Game.Common.Scripts.Settings;
using Game.Common.Scripts.Systems;
using Game.Hero_Selection.Scripts.UI.Menu.View;
using Game.Hero_Selection.Scripts.UI.Model;
using UI_System.Scripts.Menu.Abstract;
using UnityEngine;
using UnityEngine.UI;

namespace Game.Hero_Selection.Scripts.UI.Menu
{
    public class HeroSelectionMenu : MenuBase
    {
        [Header("Models")] 
        [SerializeField] private List<CharacterViewModel> characterModels;

        [Header("Prefabs")]
        [SerializeField] private HeroViewItem heroViewItemPrefab;

        [Header("UI Elements")]
        [SerializeField] private Transform contents;
        [SerializeField] private Button battleButton;

        private List<HeroViewItem> _instantiatedViews;

        protected override void OnAppeared()
        {
            _instantiatedViews = new List<HeroViewItem>();
            
            foreach (var characterModel in characterModels)
            {
                characterModel.Load();
            }
            
            var availableModels = characterModels
                .Where(model => model.ViewStatues.IsAvailable)
                .ToList();

            var gameSettings = Resources.Load<GameSettings>("GameSettings");
            var mustAvailableCount = 3 + gameSettings.CurrentTotalGamePlayed / 5;
            if (availableModels.Count < mustAvailableCount)
            {
                availableModels = new List<CharacterViewModel>();
                for (var i = 0; i < mustAvailableCount; i++)
                {
                    var model = characterModels[i];
                    model.ViewStatues.ChangeAvailability(true);
                    availableModels.Add(model);
                }
            }

            foreach (var availableModel in availableModels)
            {
                var instantiatedView = Instantiate(heroViewItemPrefab, contents);
                instantiatedView.ShowView(availableModel);
                _instantiatedViews.Add(instantiatedView);
            }
        }

        protected override void OnDisappeared()
        {
            if (_instantiatedViews == null)
            {
                return;
            }

            foreach (var instantiatedView in _instantiatedViews)
            {
                Destroy(instantiatedView.gameObject);
            }
        }

        public void SetBattleButtonActive(bool isActive)
        {
            if (battleButton == null)
            {
                Debug.LogError("BattleButton is missing in HeroSelectionMenu");
            }
            else
            {
                battleButton.interactable = isActive;
            }            
        }

        public void OnBattleButtonClicked()
        {
            SystemServices.GetSystem<LevelSystem>().GetManager<LevelManager>().Load(GameSettings.State.Battle);
        }
    }
}
