using System.Collections.Generic;
using UnityEngine;

namespace Framework.Settings
{
    [CreateAssetMenu(fileName = "FrameworkSettings", menuName = "Framework/New FrameworkSettings", order = 0)]
    public class FrameworkSettings : ScriptableObject
    {
        [Header("Editor Scene")]
        [SerializeField] private List<string> initialScenes;

        public List<string> InitialScenes => initialScenes;
    }
}