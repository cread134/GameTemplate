using System.Collections.Generic;

namespace Core.SceneManagement
{
    public interface ILoadedObject
    {
        void OnLoadedIntoScene();
        List<SceneLoadingHook> GetLoadHooks();
    }
}
