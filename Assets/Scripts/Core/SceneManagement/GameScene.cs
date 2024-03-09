using UnityEngine;

namespace Core.SceneManagement
{
    [CreateAssetMenu]
    public class GameScene : ScriptableObject
    {
        public string sceneName;
        public int buildIndex;
    }
}