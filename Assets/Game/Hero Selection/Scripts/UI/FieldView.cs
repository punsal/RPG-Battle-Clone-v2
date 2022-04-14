using TMPro;
using UnityEngine;

namespace Game.Hero_Selection.Scripts.UI
{
    public class FieldView : MonoBehaviour
    {
        [Header("UI Elements")]
        [SerializeField] private TextMeshProUGUI textLabel;
        [SerializeField] private TextMeshProUGUI textValue;

        public void SetLabel(string text)
        {
            if (textLabel == null)
            {
                Debug.LogError($"{gameObject.name} has missing reference for \"textLabel\".");
                return;
            }

            textLabel.text = text + " : ";
        }

        public void SetValue(string text)
        {
            if (textValue == null)
            {
                Debug.LogError($"{gameObject.name} has missing reference for \"textValue\".");
                return;
            }

            textValue.text = text;
        }
    }
}
