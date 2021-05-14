using TMPro;
using UnityEngine;

namespace UI_System.Scripts.Popup.Header
{
    public class HeaderView : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI textHeader;

        public void SetText(string text)
        {
            textHeader.text = text;
        }
    }
}
