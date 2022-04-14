using Game.Battle.Scripts.BattleCharacter.Abstract;
using UnityEngine;
using UnityEngine.UI;

namespace Game.Battle.Scripts.BattleCharacter.UI
{
    public class HealthBar : MonoBehaviour
    {
        [Header("UI Elements")]
        [SerializeField] private Image fillImage;

        private BattleCharacterController _battleCharacterController;
        
        public void Construct(BattleCharacterController battleCharacterController)
        {
            Debug.Log("HealthBar constructed with " + battleCharacterController.name);
            _battleCharacterController = battleCharacterController;
            _battleCharacterController.OnHealthChanged.AddListener(OnHealthChangedHandler);
            
            OnHealthChangedHandler(
                _battleCharacterController.Model.CharacterModel.CurrentHealth,
                _battleCharacterController.Model.CharacterModel.StartingHealth);
        }

        private void OnDisable()
        {
            _battleCharacterController.OnHealthChanged.RemoveListener(OnHealthChangedHandler);
        }

        private void OnHealthChangedHandler(float currentHealth, float startingHealth)
        {
            Debug.Log($"{_battleCharacterController.name} - HealthBar listened health change. ({currentHealth},{startingHealth})");
            fillImage.fillAmount = currentHealth / startingHealth;
        }
    }
}