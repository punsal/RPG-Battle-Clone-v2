using Game.Hero_Selection.Scripts.Character.Model.Abstract;
using UI_System.Scripts.Popup.Abstract;
using UnityEngine;

namespace Game.Hero_Selection.Scripts.UI.Popup
{
    public class CharacterAttributePopup : PopupBase
    {
        [Header("Model")]
        [SerializeField] private CharacterModel characterModel;
        
        [Header("Custom Views")]
        [SerializeField] private FieldView nameAttributeView;
        [SerializeField] private FieldView levelAttributeView;
        [SerializeField] private FieldView attackPowerAttributeView;
        [SerializeField] private FieldView experienceAttributeView;
        
        protected override void OnInitialized()
        {
            nameAttributeView.SetLabel("Name");
            levelAttributeView.SetLabel("Level");
            attackPowerAttributeView.SetLabel("Attack Power");
            experienceAttributeView.SetLabel("Experience");
        }

        protected override void OnTerminated()
        {
            
        }

        public void Construct(CharacterModel model)
        {
            SetCharacterModel(model);
            Show();
        }
        
        private void SetCharacterModel(CharacterModel model)
        {
            characterModel = model;
        }

        private void Show()
        {
            nameAttributeView.SetValue(characterModel.CharacterName);
            levelAttributeView.SetValue(characterModel.CurrentLevel.GetLevel().ToString());
            attackPowerAttributeView.SetValue(characterModel.CurrentAttackPower.ToString("F2"));
            experienceAttributeView.SetValue(characterModel.CurrentLevel.GetExperience().ToString());
        }

        [ContextMenu("Test/Set CharacterModel")]
        private void TestCharacterModel()
        {
            Show();
        }
    }
}
