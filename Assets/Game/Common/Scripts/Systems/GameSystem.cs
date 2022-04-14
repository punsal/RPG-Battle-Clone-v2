using Framework.Abstract;
using Game.Common.Scripts.Managers;

namespace Game.Common.Scripts.Systems
{
    public class GameSystem : MonoSystem
    {
        protected override void OnInitialized()
        {
            
        }

        protected override void OnManagersInitialized()
        {
            GetManager<GameManager>().StartGame();
        }

        protected override void OnTerminated()
        {
            
        }
    }
}