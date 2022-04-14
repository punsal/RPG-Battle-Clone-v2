using System.Collections;
using Framework.ServiceLocator.Signals;
using Framework.ServiceLocator.Signals.Interface;
using Game.Battle.Scripts.BattleCharacter.Attributes;
using Game.Battle.Scripts.BattleCharacter.Facade;
using Game.Battle.Scripts.BattleCharacter.Feedback;
using Game.Battle.Scripts.BattleCharacter.Signals;
using Game.Battle.Scripts.BattleCharacter.UI;
using Game.Hero_Selection.Scripts.UI.Model;
using UnityEngine;

namespace Game.Battle.Scripts.BattleCharacter.Abstract
{
    public abstract class BattleCharacterController : MonoBehaviour
    {
        public class OnHealthChangedSignal : ISignal<OnHealthChangedSignal.HealthChanged>
        {
            public delegate void HealthChanged(float currentHealth, float startingHealth); 
            private event HealthChanged OnHealthChanged;
            
            public void RaiseOnHealthChanged(float currenthealth, float startinghealth)
            {
                OnHealthChanged?.Invoke(currenthealth, startinghealth);
            }

            public void AddListener(HealthChanged action)
            {
                OnHealthChanged += action;
            }

            public void RemoveListener(HealthChanged action)
            {
                OnHealthChanged -= action;
            }
        }

        [Header("Model")] 
        [SerializeField] private CharacterViewModel characterViewModel;

        public CharacterViewModel Model => characterViewModel;

        [Header("Graphics")]
        [SerializeField] private SpriteRenderer iconSpriteRenderer;

        [Header("Detection")]
        [SerializeField] private BattleCharacterFacade battleCharacterFacade;
        public BattleCharacterFacade Facade => battleCharacterFacade;

        protected BattleCharacterFacade BattleCharacterFacade => battleCharacterFacade;
        
        [Header("UI Elements")]
        [SerializeField] private HealthBar healthBar;
        [SerializeField] private TextFeedback feedback;

        protected TextFeedback Feedback => feedback;
        
        public bool IsActive => gameObject.activeInHierarchy;

        private bool isAttacking = false;
        
        private OnHealthChangedSignal _onHealthChangedSignal;
        
        // ReSharper disable once MergeConditionalExpression
        public OnHealthChangedSignal OnHealthChanged => _onHealthChangedSignal == null
            ? _onHealthChangedSignal = new OnHealthChangedSignal()
            : _onHealthChangedSignal;

        public abstract IEnumerator SurviveBattle();

        public virtual void Construct(CharacterViewModel model)
        {
            characterViewModel = model;

            iconSpriteRenderer.sprite = characterViewModel.CharacterSprite;
            healthBar.Construct(this);
            feedback.Hide();

            gameObject.SetActive(!(characterViewModel.CharacterModel.CurrentHealth <= 0f));
        }

        public void Reconstruct()
        {
            Construct(characterViewModel);
        }

        public IEnumerator ExecuteAttack(BattleCharacterController opponent, float duration)
        {
            if (isAttacking)
            {
                yield break;
            }

            isAttacking = true;
            var halfDuration = duration * 0.5f;
            var initialPosition = transform.position;
            
            var timer = 0f;
            while (timer <= halfDuration)
            {
                var nextPosition = Vector3.Lerp(
                    transform.position, 
                    opponent.transform.position, 
                    timer / halfDuration);

                transform.position = nextPosition;
                
                timer += Time.deltaTime;

                yield return null;
            }
            
            opponent.TakeDamage(Model.CharacterModel.CurrentAttackPower);
            
            timer = 0f;
            while (timer <= halfDuration)
            {
                var nextPosition = Vector3.Lerp(
                    transform.position, 
                    initialPosition, 
                    timer / halfDuration);

                transform.position = nextPosition;
                
                timer += Time.deltaTime;

                yield return null;
            }
            
            transform.localPosition = Vector3.zero;

            SignalServices.GetEvent<OnCharacterAttackedSignal>().RaiseOnCharacterAttacked();
            isAttacking = false;
        }

        private void TakeDamage(float damage)
        {
            characterViewModel.CharacterModel.TakeDamage(damage);
            feedback.Show($"HP: {damage:F2}", Color.red, 1f, gameObject.name);
            StartCoroutine(TakeDamageVisually(0.5f));
            OnHealthChanged.RaiseOnHealthChanged(
                characterViewModel.CharacterModel.CurrentHealth, 
                characterViewModel.CharacterModel.StartingHealth);
        }

        private IEnumerator TakeDamageVisually(float duration)
        {
            var timer = 0f;
            var fromColor = Color.red;
            var toColor = Color.white;

            iconSpriteRenderer.color = fromColor;

            var isDead = characterViewModel.CharacterModel.CurrentHealth <= 0f;
            if (isDead)
            {
                toColor = Color.black;
                toColor.a = 0f;
                duration *= 0.5f;
            }
            
            while (timer <= duration)
            {
                timer += Time.deltaTime;
                var nextAlpha = Color.Lerp(fromColor, toColor, timer / duration);
                iconSpriteRenderer.color = nextAlpha;

                yield return null;
            }

            if (isDead)
            {
                gameObject.SetActive(false);
            }
        }

        [TestMethodButton("Construct")]
        protected void TestConstruct()
        {
            Reconstruct();
        }

        [TestMethodButton("Reset Health")]
        protected void ResetHealth()
        {
            characterViewModel.CharacterModel.ResetDefaults();
        }

        [TestMethodButton("Construct HealthBar")]
        protected void TestHealthBarConstruct()
        {
            healthBar.Construct(this);
        }

        [TestMethodButton("Take Damage")]
        protected void TestTakeDamage()
        {
            TakeDamage(5f);
        }
    }
}