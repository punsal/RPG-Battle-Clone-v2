using UI_System.Scripts.Menu.Settings;
using UnityEngine;

namespace UI_System.Scripts.Menu
{
    public abstract class MenuManager : MonoBehaviour
    {
        [Header("Settings")]
        [SerializeField] private MenuManagerSettings settings;
        public MenuManagerSettings Settings => settings;
    }
}
