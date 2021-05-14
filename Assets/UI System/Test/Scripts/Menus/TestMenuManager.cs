using System.Collections.Generic;
using System.Linq;
using UI_System.Scripts.Menu;
using UI_System.Scripts.Menu.Abstract;
using UnityEngine;

namespace UI_System.Test.Scripts.Menus
{
    public class TestMenuManager : MenuManager
    {
        private List<MenuBase> instantiatedMenus;

        private void Start()
        {
            instantiatedMenus = new List<MenuBase>();
        }

        private void Update()
        {
            if (Input.GetKeyUp(KeyCode.A))
            {
                foreach (var instantiatedMenu in instantiatedMenus)
                {
                    instantiatedMenu.Hide();
                }
                var menu = instantiatedMenus.FirstOrDefault(menuBase => menuBase.GetType() == typeof(TestMenuA)); 
                if (menu == null)
                {
                    var selectedMenu = Settings.menus.FirstOrDefault(menuBase => menuBase.GetType() == typeof(TestMenuA));
                    if (selectedMenu == null)
                    {
                        Debug.LogError("Could not find TestMenuA in settings.");
                        return;
                    }
                    var instantiatedMenu = Instantiate(selectedMenu, transform); 
                    instantiatedMenu.Show();
                    instantiatedMenus.Add(instantiatedMenu);
                }
                else
                {
                    menu.Show();
                }
            }
            
            if (Input.GetKeyUp(KeyCode.B))
            {
                foreach (var instantiatedMenu in instantiatedMenus)
                {
                    instantiatedMenu.Hide();
                }
                var menu = instantiatedMenus.FirstOrDefault(menuBase => menuBase.GetType() == typeof(TestMenuB)); 
                if (menu == null) 
                { 
                    var selectedMenu = Settings.menus.FirstOrDefault(menuBase => menuBase.GetType() == typeof(TestMenuB));
                    if (selectedMenu == null)
                    {
                        Debug.LogError("Could not find TestMenuB in settings.");
                        return;
                    }
                    var instantiatedMenu = Instantiate(selectedMenu, transform); 
                    instantiatedMenu.Show();
                    instantiatedMenus.Add(instantiatedMenu);
                }
                else
                {
                    menu.Show();
                }
            }
        }
    }
}
