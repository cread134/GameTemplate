using Core.Resources;
using System;

namespace Core.SceneManagement
{
    public interface ISceneLoader : IResourceInstance
    {
        void LoadScene(GameScene registeredScene);
        void RegisterLoadingHook(SceneLoadingHook loadingHook);
        void LoadScene(int buildIndex);

        EventHandler<GameScene> OnSceneLoaded { get; set; }
    }
}