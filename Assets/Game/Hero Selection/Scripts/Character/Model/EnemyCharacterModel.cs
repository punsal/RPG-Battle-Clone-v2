using Game.Hero_Selection.Scripts.Character.Model.Abstract;
using UnityEngine;

namespace Game.Hero_Selection.Scripts.Character.Model
{
    [CreateAssetMenu(fileName = "Enemy Character", menuName = "Game/Models/Characters/Create Enemy", order = 1)]
    public class EnemyCharacterModel : CharacterModel
    {
        public override void Initialize()
        {
            CreateCharacter();
            ResetCurrentHealth();
        }
        
        [ContextMenu("Create Character")]
        protected override void CreateCharacter()
        {
            CharacterName = name;
            DefaultLevel = new Level();
            DefaultHealth = Random.Range(150f, 200f);
            DefaultAttackPower = Random.Range(20f, 30f);
            ResetDefaults();
        }
    }
}