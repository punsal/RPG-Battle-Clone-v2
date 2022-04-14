using UI_System.Scripts.Menu;
using UnityEngine;

namespace UI_System.Test.Scripts.Menus
{
    public class TestMenuManager : MenuManager
    {
        private void Update()
        {
            if (Input.GetKeyUp(KeyCode.A))
            {
                Show<TestMenuA>();
            }
            
            if (Input.GetKeyUp(KeyCode.B))
            {
                Show<TestMenuB>();
            }
        }

        protected override void OnInitialized()
        {
            
        }
    }
}
