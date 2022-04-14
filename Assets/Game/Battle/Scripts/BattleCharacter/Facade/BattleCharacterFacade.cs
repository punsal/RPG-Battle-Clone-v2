using Framework.ServiceLocator.Systems;
using Game.Battle.Scripts.BattleCharacter.Abstract;
using Game.Battle.Scripts.Systems.Command_System;
using Game.Battle.Scripts.Systems.Command_System.Data;
using Game.Battle.Scripts.Systems.Command_System.Type;
using Game.Hero_Selection.Scripts.Character.Model.Abstract;
using UnityEngine;

namespace Game.Battle.Scripts.BattleCharacter.Facade
{
    public class BattleCharacterFacade : MonoBehaviour
    {
        [Header("Controller")]
        [SerializeField] private BattleCharacterController battleCharacterController;

        [Header("Behaviour")]
        [SerializeField] private float attackDuration = 1f;

        public void Attack(BattleCharacterController opponent)
        {
            SystemServices.GetSystem<CommandSystem>().CreateCommand(battleCharacterController.Model.CharacterModel.CharacterName + " Attack", attackDuration, new[]
            {
                new CommandActionData()
                {
                    CommandType = CommandActionType.OnStart,
                    CommandAction = () =>
                    {
                        StartCoroutine(battleCharacterController.ExecuteAttack(opponent, attackDuration));
                    }
                }
            });
        }

        public CharacterModel GetCharacterModel()
        {
            return battleCharacterController.Model.CharacterModel;
        }
    }
}
