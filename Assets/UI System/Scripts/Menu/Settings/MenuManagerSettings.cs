using System.Collections.Generic;
using UI_System.Scripts.Menu.Abstract;
using UnityEngine;

namespace UI_System.Scripts.Menu.Settings
{
    public abstract class MenuManagerSettings : ScriptableObject
    {
        public List<MenuBase> menus;
    }
}