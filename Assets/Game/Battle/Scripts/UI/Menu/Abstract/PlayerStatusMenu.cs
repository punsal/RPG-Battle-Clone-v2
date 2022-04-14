using TMPro;
using UI_System.Scripts.Menu.Abstract;
using UnityEngine;
using UnityEngine.UI;

namespace Game.Battle.Scripts.UI.Menu.Abstract
{
    public abstract class PlayerStatusMenu : MenuBase
    {
        private BattleMenuManager _battleMenuManager;

        protected BattleMenuManager MenuManager => _battleMenuManager == null
            ? _battleMenuManager = GetComponentInParent<BattleMenuManager>()
            : _battleMenuManager;

        [Header("UI Elements")]
        [SerializeField] private TextMeshProUGUI statusText;
        [SerializeField] private Button returnToSelectionMenuButton;
        
        protected override void OnAppeared()
        {
            ShowMessage(statusText);
            returnToSelectionMenuButton.onClick.AddListener(MenuManager.ReturnToHeroSelection);
        }

        protected override void OnDisappeared()
        {
            statusText.text = string.Empty;
            returnToSelectionMenuButton.onClick.RemoveListener(MenuManager.ReturnToHeroSelection);
        }

        protected abstract void ShowMessage(TextMeshProUGUI textMeshProUGUI);
    }
}