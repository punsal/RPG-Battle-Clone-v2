using Game.Battle.Scripts.UI.Menu.Abstract;
using TMPro;
using UnityEngine;

namespace Game.Battle.Scripts.UI.Menu
{
    public class PlayerFailMenu : PlayerStatusMenu
    {
        protected override void ShowMessage(TextMeshProUGUI textMeshProUGUI)
        {
            textMeshProUGUI.text = "Lost";
            textMeshProUGUI.color = Color.red;
        }
    }
}