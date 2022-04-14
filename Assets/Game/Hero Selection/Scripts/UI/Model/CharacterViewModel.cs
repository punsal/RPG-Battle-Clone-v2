using System;
using Game.Hero_Selection.Scripts.Character.Model.Abstract;
using Save_Utility.Scripts;
using Save_Utility.Scripts.Abstract;
using UI_System.Scripts.Popup.Abstract;
using UnityEngine;

namespace Game.Hero_Selection.Scripts.UI.Model
{
    [CreateAssetMenu(fileName = "CharacterView Model", menuName = "Game/View Models/Characters/Create CharacterView")]
    public class CharacterViewModel : SavableScriptableObject
    {
        [Serializable]
        public class ViewStatue
        {
            [SerializeField] private bool isAvailable = false;
            [SerializeField] private bool isSelected = false;

            public bool IsAvailable => isAvailable;
            public bool IsSelected => isSelected;

            public void Select(bool isSelected)
            {
                this.isSelected = isSelected;
            }

            public void ChangeAvailability(bool availability)
            {
                isAvailable = availability;
            }
        }
        
        [Header("Models")]
        [SerializeField] private CharacterModel characterModel;

        [Header("Views")]
        [SerializeField] private PopupBase characterAttributePopup;
        
        [Header("Attributes")]
        [SerializeField] private Sprite characterSprite;
        [SerializeField] private ViewStatue viewStatues;

        public CharacterModel CharacterModel => characterModel;
        public Sprite CharacterSprite => characterSprite;

        public PopupBase CharacterAttributePopup => characterAttributePopup;
        
        public ViewStatue ViewStatues => viewStatues;

        public override void Save()
        {
            characterModel.Save();
            SaveUtility.Save(this);
        }

        public override void Load()
        {
            
            if (SaveUtility.Load(this, out var result))
            {
                var characterResult = (CharacterViewModel) result;
                characterModel.Load();
                characterSprite = characterResult.characterSprite;
                viewStatues = characterResult.viewStatues;
            }
            else
            {
                ResetDefaults();
            }
        }

        public override void ResetDefaults()
        {
            if (characterModel == null)
            {
                Debug.LogError("CharacterModel is missing for" + name);
                return;
            }

            if (characterSprite == null)
            {
                Debug.LogError("CharacterSprite is missing for" + name);
                return;
            }
            viewStatues = new ViewStatue();
            Save();
        }
    }
}