using System;
using Framework.Abstract;
using Framework.ServiceLocator.Systems;
using Game.Battle.Scripts.Managers;
using Game.Common.Scripts.Settings;
using Game.Common.Scripts.Systems;
using Game.Hero_Selection.Scripts.UI.Menu;
using UI_System.Scripts;
using UnityEngine;

namespace Game.Common.Scripts.Managers
{
    public class GameManager : MonoManager
    {
        private GameSettings _gameSettings;
        
        public override void Initialize()
        {
            _gameSettings = Resources.Load<GameSettings>("GameSettings");
        }

        public void StartGame()
        {
            switch (_gameSettings.CurrentState)
            {
                case GameSettings.State.CharacterSelection:
                    _gameSettings.CurrentBattleState = GameSettings.BattleState.InitializeBattle;
                    StartCharacterSelection();
                    break;
                case GameSettings.State.Battle:
                    StartBattle(_gameSettings.CurrentBattleState);
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        private void StartCharacterSelection()
        {
            SystemServices.GetSystem<UISystem>().GetManager<HeroSelectionMenuManager>().Show<HeroSelectionMenu>();
        }

        private void StartBattle(GameSettings.BattleState battleState)
        {
            var battleManager = SystemServices.GetSystem<GameSystem>().GetManager<BattleManager>();
            
            switch (battleState)
            {
                case GameSettings.BattleState.InitializeBattle: 
                    battleManager.OnInitializeBattle();
                    break;
                case GameSettings.BattleState.PlayerTurn:
                    battleManager.OnPlayerTurnStarted();
                    break;
                case GameSettings.BattleState.EnemyTurn:
                    battleManager.OnEnemyTurnStarted();
                    break;
                case GameSettings.BattleState.PlayerWon:
                    battleManager.OnPlayerWonStarted();
                    break;
                case GameSettings.BattleState.PlayerLost:
                    battleManager.OnPlayerLostStarted();
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(battleState), battleState, null);
            }
        }
    }
}