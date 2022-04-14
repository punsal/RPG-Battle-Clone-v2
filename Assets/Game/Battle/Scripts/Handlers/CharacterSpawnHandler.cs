using System;
using System.Collections.Generic;
using Game.Battle.Scripts.BattleCharacter.Abstract;
using Game.Hero_Selection.Scripts.UI.Model;
using UnityEngine;

namespace Game.Battle.Scripts.Handlers
{
    public class CharacterSpawnHandler : MonoBehaviour
    {
        [Header("Prefabs")]
        [SerializeField] private BattleCharacterController heroBattleCharacterControllerPrefab;
        [SerializeField] private BattleCharacterController enemyBattleCharacterControllerPrefab;
        
        [Header("Character Spawn Points")]
        [SerializeField] private List<Transform> enemySpawnPoints;
        [SerializeField] private List<Transform> heroSpawnPoints;

        public List<BattleCharacterController> SpawnHeroes(IEnumerable<CharacterViewModel> heroModels)
        {
            var spawnedCharacters = new List<BattleCharacterController>();
            var index = 0;
            foreach (var characterViewModel in heroModels)
            {
                spawnedCharacters.Add(SpawnCharacter(heroBattleCharacterControllerPrefab, heroSpawnPoints[index], characterViewModel));
                index++;
            }

            return spawnedCharacters;
        }

        public BattleCharacterController SpawnEnemy(CharacterViewModel characterViewModel)
        {
            Debug.Log("Try "+ characterViewModel.CharacterModel.CharacterName + " spawning");
            var result = SpawnCharacter(enemyBattleCharacterControllerPrefab, enemySpawnPoints[0], characterViewModel);

            return result;
        }

        private BattleCharacterController SpawnCharacter(BattleCharacterController controllerPrefab, Transform parent, CharacterViewModel characterViewModel)
        {
            var instantiatedCharacter = Instantiate(controllerPrefab, parent);
            instantiatedCharacter.Construct(characterViewModel);
            return instantiatedCharacter;
        }
    }
}