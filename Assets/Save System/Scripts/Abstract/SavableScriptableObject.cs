using Save_System.Scripts.Interface;
using UnityEngine;

namespace Save_System.Scripts.Abstract
{
    public abstract class SavableScriptableObject : ScriptableObject, ISavable
    {
        [ContextMenu("Save System/Save")]
        public abstract void Save();

        [ContextMenu("Save System/Load")]
        public abstract bool Load(out object result);
    }
}