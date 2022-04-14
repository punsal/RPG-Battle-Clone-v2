using Framework.Interface;
using UnityEngine;

namespace Framework.Abstract
{
    public abstract class MonoManager : MonoBehaviour, IInitializable
    {
        public abstract void Initialize();
    }
}