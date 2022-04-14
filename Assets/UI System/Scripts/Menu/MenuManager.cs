using System;
using System.Collections.Generic;
using System.Linq;
using Framework.Abstract;
using UI_System.Scripts.Menu.Abstract;
using UI_System.Scripts.Menu.Settings;
using UnityEngine;

namespace UI_System.Scripts.Menu
{
    public abstract class MenuManager : MonoManager
    {
        [Header("Settings")]
        [SerializeField] private MenuManagerSettings settings;
        public MenuManagerSettings Settings => settings;
        
        private List<MenuBase> _instantiatedMenus;

        // ReSharper disable once MergeConditionalExpression
        public List<MenuBase> InstantiatedMenus => _instantiatedMenus == null
            ? _instantiatedMenus = new List<MenuBase>()
            : _instantiatedMenus;

        public override void Initialize()
        {
            _instantiatedMenus = new List<MenuBase>();
            OnInitialized();
        }

        protected abstract void OnInitialized();

        public void Show<T>() where T : MenuBase
        {
            var isMenuInSettings = Settings.menus.Exists(menuBase => menuBase.GetType() == typeof(T));
            if (!isMenuInSettings)
            {
                throw new Exception($"{typeof(T).Name} cannot show {typeof(T).Name} because {typeof(T).Name} does not exists in {Settings.name}");
            }

            var isMenuAlreadyInstantiated = InstantiatedMenus.Exists(menuBase => menuBase.GetType() == typeof(T));
            
            foreach (var instantiatedMenu in InstantiatedMenus)
            {
                instantiatedMenu.Hide();
            }

            if (isMenuAlreadyInstantiated)
            {
                foreach (var instantiatedMenu in InstantiatedMenus.Where(instantiatedMenu => instantiatedMenu.GetType() == typeof(T)))
                {
                    instantiatedMenu.Show();
                }
            }
            else
            {
                var selectedMenu = Settings.menus.FirstOrDefault(menuBase => menuBase.GetType() == typeof(T));
                if (selectedMenu == null)
                {
                    Debug.LogError($"Could not find {typeof(T).Name} in settings.");
                    return;
                }
                var instantiatedMenu = Instantiate(selectedMenu, transform); 
                instantiatedMenu.Show();
                InstantiatedMenus.Add(instantiatedMenu);
            }
        }
    }
}
