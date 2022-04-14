using Framework.ServiceLocator.Systems;
using Game.Battle.Scripts.Managers;
using Game.Common.Scripts.Managers;
using Game.Common.Scripts.Settings;
using Game.Common.Scripts.Systems;
using UI_System.Scripts.Menu;
using UnityEngine;

namespace Game.Battle.Scripts.UI.Menu
{
    public class BattleMenuManager : MenuManager
    {
        protected override void OnInitialized()
        {
            
        }

        public void ReturnToHeroSelection()
        {
            var gameSettings = Resources.Load<GameSettings>("GameSettings");
            SystemServices.GetSystem<GameSystem>().GetManager<BattleManager>().SaveModels();
            SystemServices.GetSystem<LevelSystem>().GetManager<LevelManager>().Load(GameSettings.State.CharacterSelection);
            gameSettings.GamePlayed();
        }
    }
}