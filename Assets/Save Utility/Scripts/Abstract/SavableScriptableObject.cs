using Save_Utility.Scripts.Interface;
using UnityEngine;

namespace Save_Utility.Scripts.Abstract
{
    public abstract class SavableScriptableObject : ScriptableObject, ISavable
    {
        [ContextMenu("Save System/Save")]
        public abstract void Save();

        [ContextMenu("Save System/Load")]
        public abstract void Load();

        [ContextMenu("Save System/Reset Defaults")]
        public abstract void ResetDefaults();
    }
}