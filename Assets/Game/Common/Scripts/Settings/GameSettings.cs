using Save_Utility.Scripts;
using Save_Utility.Scripts.Abstract;
using UnityEngine;

namespace Game.Common.Scripts.Settings
{
    [CreateAssetMenu(fileName = "GameSettings", menuName = "Game/Create Settings", order = 0)]
    public class GameSettings : SavableScriptableObject
    {
        public enum State
        {
            CharacterSelection,
            Battle
        }

        public enum BattleState
        {
            InitializeBattle,
            PlayerTurn,
            EnemyTurn,
            PlayerWon,
            PlayerLost
        }

        [Header("Defaults")]
        [SerializeField] private State defaultState = State.CharacterSelection;
        [SerializeField] private BattleState defaultBattleState = BattleState.PlayerTurn;
        [SerializeField] private int defaultTotalGamePlayed = 0;

        [Header("Current Values")]
        [SerializeField] private State currentState = State.CharacterSelection;
        [SerializeField] private BattleState currentBattleState = BattleState.PlayerTurn;
        [SerializeField] private int currentTotalGamePlayed = 0;

        public State CurrentState
        {
            get => currentState;
            set
            {
                currentState = value;
                Debug.Log("CurrentState changed: " + value);
                Save();
            }
        }

        public BattleState CurrentBattleState
        {
            get => currentBattleState;
            set
            {
                currentBattleState = value;
                Debug.Log("CurrentBattleState changed: " + value);
                Save();
            }
        }

        public int CurrentTotalGamePlayed => currentTotalGamePlayed;

        public void GamePlayed()
        {
            currentTotalGamePlayed++;
            Save();
        }
        
        public override void Save()
        {
            SaveUtility.Save(this);
        }

        public override void Load()
        {
            if (SaveUtility.Load(this, out var result))
            {
                var gameSettingsResult = (GameSettings) result;
                currentState = gameSettingsResult.currentState;
                currentBattleState = gameSettingsResult.currentBattleState;
                currentTotalGamePlayed = gameSettingsResult.currentTotalGamePlayed;
            }
            else
            {
                ResetDefaults();
            }
        }

        public override void ResetDefaults()
        {
            currentState = defaultState;
            currentBattleState = defaultBattleState;
            currentTotalGamePlayed = defaultTotalGamePlayed;
            Save();
        }
    }
}