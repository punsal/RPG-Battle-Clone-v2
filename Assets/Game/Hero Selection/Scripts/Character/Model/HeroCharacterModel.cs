using Game.Hero_Selection.Scripts.Character.Model.Abstract;
using UnityEngine;

namespace Game.Hero_Selection.Scripts.Character.Model
{
    [CreateAssetMenu(fileName = "Hero Character", menuName = "Game/Models/Characters/Create Hero", order = 0)]
    public class HeroCharacterModel : CharacterModel
    {
        public override void Initialize()
        {
            ResetCurrentHealth();
        }

        [ContextMenu("Create Character")]
        protected override void CreateCharacter()
        {
#if UNITY_EDITOR
            CharacterName = name;
            DefaultLevel = new Level();
            DefaultHealth = Random.Range(75f, 125f);
            DefaultAttackPower = Random.Range(10f, 25f);
            ResetDefaults();
#endif
        }
        
        public float IncreaseAttackPower()
        {
            var attackPowerIncrease = CurrentAttackPower * 0.1f;
            CurrentAttackPower += attackPowerIncrease;
            return attackPowerIncrease;
        }

        public float IncreaseStartingHealth()
        {
            var healthIncrease = StartingHealth * 0.1f;
            StartingHealth += healthIncrease;
            return healthIncrease;
        }
    }
}