using Framework.Abstract;
using Game.Battle.Scripts.Systems.Command_System.Data;
using Game.Battle.Scripts.Systems.Command_System.Handler;
using UnityEngine;

namespace Game.Battle.Scripts.Systems.Command_System
{
    public class CommandSystem : MonoSystem
    {
        protected override void OnInitialized()
        {
            
        }

        protected override void OnManagersInitialized()
        {
            
        }

        protected override void OnTerminated()
        {
            
        }

        public CommandHandler CreateCommand(string commandName, float duration, CommandActionData[] actions)
        {
            var commandData = new CommandData();
            commandData.SetTime(duration);
            
            var temp = new GameObject($"Command - {commandName}");
            var handler = temp.AddComponent<CommandHandler>();
            handler.Initialize(commandData, actions);

            return handler;
        }
    }
}
