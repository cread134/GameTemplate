using Core.Logging;
using Core.Resources;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Core.SceneManagement
{
    public interface ILoadedObject
    {
        void OnLoadedIntoScene();
        List<SceneLoadingHook> GetLoadHooks();
    }

    public class SceneLoadingHook
    {
        public string message;
        public Func<bool> condition;
    }

    public class SceneLoader : MonoBehaviour, ISceneLoader
    {
        const int LOADING_SCENE_INDEX = 0;
        ILoggingService loggingService;
        public void OnResourceCreating()
        {
            loggingService = ObjectFactory.ResolveService<ILoggingService>();
        }
        public void LoadScene(GameScene registeredScene)
        {
            if(isLoadingScene)
            {
                return;
            }
            isLoadingScene = true;
            StartCoroutine(LoadSceneAsync(registeredScene));
        }

        IEnumerator LoadSceneAsync(GameScene registeredScene)
        {
            loggingService.Log($"Loading scene {registeredScene.sceneName}");
            loadingHooks.Clear();
            SceneManager.LoadScene(LOADING_SCENE_INDEX, LoadSceneMode.Additive);

            var currentScenesCount = SceneManager.sceneCount;
            for (int i = 0; i < currentScenesCount; i++)
            {
                var scene = SceneManager.GetSceneAt(i);
                if (scene.buildIndex != LOADING_SCENE_INDEX)
                {
                    yield return UnloadSceneAsync(scene.buildIndex);
                }
            }

            //load new scene
            yield return SceneManager.LoadSceneAsync(registeredScene.buildIndex, LoadSceneMode.Additive);
            SceneManager.SetActiveScene(SceneManager.GetSceneByBuildIndex(registeredScene.buildIndex));
            PopulateHooks();
            OnSceneLoaded?.Invoke(this, registeredScene);

            while (loadingHooks.Count > 0)
            {
                var hook = loadingHooks.Dequeue();
                //update loading hook message
                yield return new WaitUntil(hook.condition);
            }
            //finished
            yield return UnloadSceneAsync(LOADING_SCENE_INDEX);

        }

        IEnumerator UnloadSceneAsync(int buildIndex)
        {
            yield return null;
        }

        Queue<SceneLoadingHook> loadingHooks = new Queue<SceneLoadingHook>();

        void PopulateHooks()
        {
            var allLoadHooks = FindObjectsOfType<MonoBehaviour>(true).OfType<ILoadedObject>();
            foreach (var loadedObject in allLoadHooks)
            {
                loadedObject.OnLoadedIntoScene();
                var hooks = loadedObject.GetLoadHooks();
                foreach (var hook in hooks)
                {
                    RegisterLoadingHook(hook);
                }
            }
        }

        public void RegisterLoadingHook(SceneLoadingHook loadingHook)
        {
            loadingHooks.Enqueue(loadingHook);
        }

        bool isLoadingScene = false;

        public EventHandler<GameScene> OnSceneLoaded { get; set; }
    }
}
