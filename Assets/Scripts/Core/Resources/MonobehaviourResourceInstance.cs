using UnityEngine;

namespace Core.Resources
{
    internal abstract class MonobehaviourResourceInstance : MonoBehaviour, IResourceInstance
    {
        public void OnResourceCreating()
        {
            DontDestroyOnLoad(gameObject);
            OnCreating();
        }

        internal abstract void OnCreating();
    }
}