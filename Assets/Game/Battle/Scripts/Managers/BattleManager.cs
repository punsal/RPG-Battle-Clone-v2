using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Framework.Abstract;
using Framework.ServiceLocator.Signals;
using Framework.ServiceLocator.Systems;
using Game.Battle.Scripts.BattleCharacter.Abstract;
using Game.Battle.Scripts.BattleCharacter.Facade;
using Game.Battle.Scripts.BattleCharacter.Signals;
using Game.Battle.Scripts.Handlers;
using Game.Battle.Scripts.UI.Menu;
using Game.Battle.Scripts.UI.Popup;
using Game.Common.Scripts.Settings;
using Game.Hero_Selection.Scripts.UI.Model;
using Game.Hero_Selection.Scripts.UI.Popup;
using UI_System.Scripts;
using UI_System.Scripts.Popup.Abstract;
using UI_System.Scripts.Popup.Signals;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Game.Battle.Scripts.Managers
{
    public class BattleManager : MonoManager
    {
        [Header("Handlers")]
        [SerializeField] private InputHandler inputHandler;
        [SerializeField] private CharacterSpawnHandler characterSpawnHandler;

        [Header("Models")]
        [SerializeField] private List<CharacterViewModel> heroCharacterViewModels;
        [SerializeField] private CharacterViewModel enemyCharacterViewModel;

        private GameSettings _gameSettings;

        private GameSettings GameSettings => _gameSettings == null
            ? _gameSettings = Resources.Load<GameSettings>("GameSettings")
            : _gameSettings;

        private List<BattleCharacterController> _heroes;
        private List<BattleCharacterController> _enemies;

        private CharacterAttributePopup _currentCharacterAttributePopup;
        
        public override void Initialize()
        {
            LoadModels();
            
            var selectedHeroes = heroCharacterViewModels
                .Where(model => model.ViewStatues.IsAvailable && model.ViewStatues.IsSelected)
                .ToList();

            _heroes = characterSpawnHandler.SpawnHeroes(selectedHeroes);

            if (enemyCharacterViewModel == null)
            {
                Debug.LogError("Enemy is missing.");
            }
            var enemy = characterSpawnHandler.SpawnEnemy(enemyCharacterViewModel);
            _enemies = new List<BattleCharacterController> {enemy};

            Debug.Log("BattleManager Initialized.");
        }

        public void SaveModels()
        {
            foreach (var heroCharacterViewModel in heroCharacterViewModels)
            {
                heroCharacterViewModel.Save();
            }
            
            enemyCharacterViewModel.Save();
        }

        private void LoadModels()
        {
            foreach (var heroCharacterViewModel in heroCharacterViewModels)
            {
                heroCharacterViewModel.Load();
            }
            
            enemyCharacterViewModel.Load();
        }

        public void OnInitializeBattle()
        {
            Debug.Log("Initialize Heroes.");
            foreach (var hero in _heroes)
            {
                hero.Model.CharacterModel.Initialize();
                hero.Reconstruct();
            }

            if (_enemies != null)
            {
                Debug.Log("Initialize Enemies.");
                foreach (var enemy in _enemies)
                {
                    enemy.Model.CharacterModel.Initialize();
                    enemy.Reconstruct();
                }
            }

            Debug.Log("Saving models.");
            SaveModels();
            
            Debug.Log("Start Player Turn.");
            OnPlayerTurnStarted();
        }
        
        public void OnPlayerTurnStarted()
        {
            //Start Input Handling.
            //If player selects a hero then attack to a random enemy.
            // After Attack end player turn.
            Debug.Log("Saving GameSettings");
            GameSettings.CurrentBattleState = GameSettings.BattleState.PlayerTurn;
            
            Debug.Log("Activate InputHandling.");
            SetPlayerHandlingActive(true);
        }

        private void SetPlayerHandlingActive(bool isActive)
        {
            if (isActive)
            {
                inputHandler.StartInputHandling();
                SignalServices.GetEvent<InputHandler.OnClickSignal>().AddListener(OnClickHandler);
                SignalServices.GetEvent<InputHandler.OnHoldSignal>().AddListener(OnHoldHandler);
            }
            else
            {
                inputHandler.StopInputHandling();
                SignalServices.GetEvent<InputHandler.OnClickSignal>().RemoveListener(OnClickHandler);
                SignalServices.GetEvent<InputHandler.OnHoldSignal>().RemoveListener(OnHoldHandler);
            }
        }

        private bool TryGetBattleCharacterFacade(Vector2 inputPosition, out BattleCharacterFacade facade)
        {
            var mainCamera = Camera.main;
            if (mainCamera == null)
            {
                throw new Exception("Main camera is not found");
            }

            var ray = mainCamera.ScreenPointToRay(inputPosition);
            var raycastHit2d = Physics2D.CircleCast(ray.origin, 0.5f, Vector2.zero);
            if (raycastHit2d.collider == null)
            {
                facade = null;
                return false;
            }

            facade = raycastHit2d.collider.GetComponent<BattleCharacterFacade>();
            return facade != null;
        }

        private void OnClickHandler(Vector2 inputPosition)
        {
            SetPlayerHandlingActive(false);
            //Attack
            if (TryGetBattleCharacterFacade(inputPosition, out var facade))
            {
                var activeEnemies = _enemies.Where(controller => controller.IsActive).ToArray();
                var randomIndex = Random.Range(0, activeEnemies.Length);
                facade.Attack(activeEnemies[randomIndex]);
                SignalServices.GetEvent<OnCharacterAttackedSignal>().AddListener(OnPlayerTurnEnded);
                SetPlayerHandlingActive(false);
                return;
            }
            
            SetPlayerHandlingActive(true);
        }

        private void OnHoldHandler(Vector2 inputPosition)
        {
            //Show Attributes popup.
            SetPlayerHandlingActive(false);
            if (TryGetBattleCharacterFacade(inputPosition, out var facade))
            {
                var popupManager = SystemServices.GetSystem<UISystem>().GetManager<BattlePopupManager>(); 
                popupManager.Show<CharacterAttributePopup>(); 
                _currentCharacterAttributePopup = popupManager.GetPopup<CharacterAttributePopup>();
                _currentCharacterAttributePopup.Construct(facade.GetCharacterModel());
                SignalServices.GetEvent<OnPopupClosedSignal>().AddListener(OnPopupClosedHandler);
            }
            SetPlayerHandlingActive(true);
        }

        private void OnPopupClosedHandler(PopupBase popupBase)
        {
            if (popupBase != _currentCharacterAttributePopup) return;
            SignalServices.GetEvent<OnPopupClosedSignal>().RemoveListener(OnPopupClosedHandler);
            SetPlayerHandlingActive(true);
        }

        private void OnPlayerTurnEnded()
        {
            //Evaluate enemies healths.
            //If all enemies died then Player won.
            //Otherwise enemy turn begins.
            //Save.
            SignalServices.GetEvent<OnCharacterAttackedSignal>().RemoveListener(OnPlayerTurnEnded);
            SaveModels();
            
            var isAllEnemiesDead = _enemies.All(enemy => !enemy.IsActive);
            if (isAllEnemiesDead)
            {
                OnPlayerWonStarted();
                return;
            }
            
            OnEnemyTurnStarted();
        }

        public void OnEnemyTurnStarted()
        {
            //Select a random enemy to attack random active hero.
            //After enemy attack ends, end enemy turn.
            GameSettings.CurrentBattleState = GameSettings.BattleState.EnemyTurn;
            
            var activeEnemies = _enemies
                .Where(enemy => enemy.IsActive)
                .ToArray();
            var activeEnemiesCount = activeEnemies.Length;
            var randomEnemy = activeEnemies[Random.Range(0, activeEnemiesCount)];
            
            var activeHeroes = _heroes
                .Where(hero => hero.IsActive)
                .ToArray();
            var activeHeroesCount = activeHeroes.Length;
            var randomHero = activeHeroes[Random.Range(0, activeHeroesCount)];
            
            randomEnemy.Facade.Attack(randomHero);
            SignalServices.GetEvent<OnCharacterAttackedSignal>().AddListener(OnEnemyTurnEnded);
        }

        private void OnEnemyTurnEnded()
        {
            //Evaluate heroes healths.
            //If all heroes died then Player lost.
            //Otherwise Player turn begins.
            //Save.
            SignalServices.GetEvent<OnCharacterAttackedSignal>().RemoveListener(OnEnemyTurnEnded);
            SaveModels();
            
            var isAllHeroesDead = _heroes.All(hero => !hero.IsActive);
            if (isAllHeroesDead)
            {
                OnPlayerLostStarted();
                return;
            }
            
            OnPlayerTurnStarted();
        }

        public void OnPlayerWonStarted()
        {
            //Find active heroes.
            //Add experience to active heroes.
            //After feedback ends, end player won.
            GameSettings.CurrentBattleState = GameSettings.BattleState.PlayerWon;
            
            var activeHeroes = _heroes.Where(hero => hero.IsActive);
            StartCoroutine(ExecuteOnPlayerWonState(activeHeroes));
        }

        private IEnumerator ExecuteOnPlayerWonState(IEnumerable<BattleCharacterController> activeHeroes)
        {
            var coroutines = new List<Coroutine>();
            foreach (var activeHero in activeHeroes)
            {
                var coroutine = StartCoroutine(activeHero.SurviveBattle());
                coroutines.Add(coroutine);
            }
            
            foreach (var coroutine in coroutines)
            {
                yield return coroutine;
            }
            
            OnPlayerWonEnded();
        }

        private void OnPlayerWonEnded()
        {
            //Open win UI.
            Debug.Log("Show PlayerWinMenu");
            SystemServices.GetSystem<UISystem>().GetManager<BattleMenuManager>().Show<PlayerWinMenu>();
        }

        public void OnPlayerLostStarted()
        {
            //Open lost UI.
            GameSettings.CurrentBattleState = GameSettings.BattleState.PlayerLost;
            Debug.Log("Show PlayerFailMenu");
            SystemServices.GetSystem<UISystem>().GetManager<BattleMenuManager>().Show<PlayerFailMenu>();
        }
    }
}