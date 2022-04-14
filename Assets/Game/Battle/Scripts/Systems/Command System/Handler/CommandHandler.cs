using System.Collections;
using Game.Battle.Scripts.Systems.Command_System.Data;
using Game.Battle.Scripts.Systems.Command_System.Type;
using UnityEngine;

namespace Game.Battle.Scripts.Systems.Command_System.Handler
{
    public class CommandHandler : MonoBehaviour
    {
        [SerializeField] private CommandData commandData;
        private CommandActionData[] _commandActions;

        public void Initialize(CommandData data, CommandActionData[] actions)
        {
            commandData.SetTime(data);
            _commandActions = actions;
            StartCoroutine(TimeCounter());
        }

        public float GetRemainingTime() => commandData.GetTime();

        private IEnumerator TimeCounter()
        {
            var waiter = new WaitForSeconds(Time.deltaTime);
            
            foreach (var action in _commandActions)
            {
                if (action.CommandType == CommandActionType.OnStart)
                {
                    action.CommandAction.Invoke();
                }
            }
            
            while (commandData.GetTime() > 0f)
            {
                commandData.Tick(Time.deltaTime);

                foreach (var action in _commandActions)
                {
                    if (action.CommandType == CommandActionType.OnTick)
                    {
                        action.CommandAction.Invoke();
                    }
                }
                
                yield return waiter;
            }
            
            foreach (var action in _commandActions)
            {
                if (action.CommandType == CommandActionType.OnEnd)
                {
                    action.CommandAction.Invoke();
                }
            }
            
            Destroy(gameObject);
        }
    }
}