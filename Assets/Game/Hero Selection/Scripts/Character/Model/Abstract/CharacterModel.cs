using System;
using Save_Utility.Scripts;
using Save_Utility.Scripts.Abstract;
using UnityEngine;

namespace Game.Hero_Selection.Scripts.Character.Model.Abstract
{
    public abstract class CharacterModel : SavableScriptableObject
    {
        [Serializable]
        public class Level
        {
            [SerializeField] private int experience = 0;
            [SerializeField] private int level = 0;

            public bool AddExperience(int value = 1)
            {
                experience += value;
                if (experience != 5) return false;
                experience = 0;
                level++;
                return true;
            }

            public int GetExperience()
            {
                return experience;
            }
            
            public int GetLevel()
            {
                return level;
            }
        }

        [Header("Default Values")]
        [SerializeField] private Level defaultLevel;
        [SerializeField] private float defaultHealth;
        [SerializeField] private float defaultAttackPower;
        
        protected Level DefaultLevel
        {
            get => defaultLevel;
            set => defaultLevel = value;
        }

        protected float DefaultHealth
        {
            get => defaultHealth;
            set => defaultHealth = value;
        }

        protected float DefaultAttackPower
        {
            get => defaultAttackPower;
            set => defaultAttackPower = value;
        }

        [Header("Attributes")]
        [SerializeField] private string characterName;
        [SerializeField] private float startingHealth;
        [SerializeField] private float currentHealth;
        [SerializeField] private float currentAttackPower;
        [SerializeField] private Level currentLevel;

        public string CharacterName
        {
            get => characterName;
            protected set => characterName = value;
        }

        public float StartingHealth
        {
            get => startingHealth;
            protected set => startingHealth = value;
        }
        
        public float CurrentHealth
        {
            get => currentHealth;
            protected set => currentHealth = value;
        }

        public float CurrentAttackPower
        {
            get => currentAttackPower;
            protected set => currentAttackPower = value;
        }

        public Level CurrentLevel
        {
            get => currentLevel;
            protected set => currentLevel = value;
        }
        
        public abstract void Initialize();
        protected abstract void CreateCharacter();

        public void ResetCurrentHealth()
        {
            if (startingHealth < 1f)
            {
                var health = defaultHealth;
                for (var i = 0; i < currentLevel.GetLevel(); i++)
                {
                    health *= 1.1f;
                }

                StartingHealth = health;
            }
            CurrentHealth = StartingHealth;
            Save();
        }
        
        public void TakeDamage(float damage)
        {
            CurrentHealth -= damage;
        }

        public override void Save()
        {
            SaveUtility.Save(this);
        }

        public override void Load()
        {
            if (SaveUtility.Load(this, out var result))
            {
                var testDataResult = (CharacterModel) result;
                characterName = testDataResult.characterName;

                defaultHealth = testDataResult.defaultHealth;
                defaultAttackPower = testDataResult.defaultAttackPower;
                defaultLevel = testDataResult.defaultLevel;

                startingHealth = testDataResult.startingHealth;
                currentHealth = testDataResult.currentHealth;
                
                currentAttackPower = testDataResult.currentAttackPower;
                currentLevel = testDataResult.currentLevel;
            }
            else
            {
                ResetDefaults();
            }
        }

        public override void ResetDefaults()
        {
            startingHealth = defaultHealth;
            currentHealth = defaultHealth;
            currentAttackPower = defaultAttackPower;
            currentLevel = defaultLevel;
            Save();
        }
    }
}