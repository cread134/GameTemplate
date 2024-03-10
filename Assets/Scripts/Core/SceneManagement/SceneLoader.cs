using Core.Logging;
using Core.PauseManagement;
using Core.Resources;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Core.SceneManagement
{
    internal class SceneLoader : MonoBehaviour, ISceneLoader
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
            loggingService.Log($"Loading scene {registeredScene.sceneName}");
            StartCoroutine(LoadSceneAsync(registeredScene.buildIndex, () => OnSceneLoaded?.Invoke(this, registeredScene)));
        }

        public void LoadScene(int buildIndex)
        {
            if (isLoadingScene)
            {
                return;
            }
            isLoadingScene = true;
            loggingService.Log($"Loading scene at buildIndex {buildIndex}");
            StartCoroutine(LoadSceneAsync(buildIndex, () => { }));
        }

        IEnumerator LoadSceneAsync(int buildIndex, Action sceneLoadedCallback)
        {
            CursorManager.LockCursor = false;
            loadingHooks.Clear();
            var loadingScreenAsync = SceneManager.LoadSceneAsync(LOADING_SCENE_INDEX, LoadSceneMode.Additive);
            while (!loadingScreenAsync.isDone)
            {
                yield return null;
            }

            var currentScenesCount = SceneManager.sceneCount;
            List<Coroutine> unloadSceneActions = new List<Coroutine>();
            for (int i = 0; i < currentScenesCount; i++)
            {
                var scene = SceneManager.GetSceneAt(i);
                if (scene.buildIndex != LOADING_SCENE_INDEX)
                {
                    var action = SceneManager.UnloadSceneAsync(buildIndex);
                    while (!action.isDone)
                    {
                        yield return null;
                    }
                    loggingService.Log($"Unloaded scene at buildIndex {buildIndex}");
                }
            }
            loggingService.Log($"Unloaded {currentScenesCount} scenes");

            //load new scene
            var asyncAction = SceneManager.LoadSceneAsync(buildIndex, LoadSceneMode.Additive);
            while (!asyncAction.isDone)
            {
                yield return null;
            }
            SceneManager.SetActiveScene(SceneManager.GetSceneByBuildIndex(buildIndex));
            PopulateHooks();
            sceneLoadedCallback?.Invoke();

            while (loadingHooks.Count > 0)
            {
                var hook = loadingHooks.Dequeue();
                //update loading hook message
                yield return new WaitUntil(hook.condition);
            }
            //finished
            var loadingScreenAsyncAction = SceneManager.UnloadSceneAsync(LOADING_SCENE_INDEX);
            while (!loadingScreenAsyncAction.isDone)
            {
                yield return null;
            }
            loggingService.Log($"Scene load finished at buildIndex {buildIndex}");
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
