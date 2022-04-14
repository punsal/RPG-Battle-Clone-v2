using Game.Battle.Scripts.UI.Menu.Abstract;
using TMPro;
using UI_System.Scripts.Menu.Abstract;
using UnityEngine;

namespace Game.Battle.Scripts.UI.Menu
{
    public class PlayerWinMenu : PlayerStatusMenu
    {
        protected override void ShowMessage(TextMeshProUGUI textMeshProUGUI)
        {
            textMeshProUGUI.text = "Victory";
            textMeshProUGUI.color = Color.green;
        }
    }
}