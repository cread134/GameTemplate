using Core.Logging;
using Core.PauseManagement;
using Core.Resources;
using Core.SceneManagement.Ui;
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
            DontDestroyOnLoad(gameObject);
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

        void UpdateLoadingBar(float progress, string message, LoadingBarView loadingBarView)
        {
            loadingBarView.SetProgress(progress, message);
        }

        LoadingBarView GetLoadingBar()
        {
            return FindFirstObjectByType<LoadingBarView>();
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
            loggingService.Log($"Loaded loading screen at buildIndex {LOADING_SCENE_INDEX}");

            float progress = 0f;
            var loadingBar = GetLoadingBar();
            UpdateLoadingBar(progress, "Started unloading", loadingBar);

            var currentScenesCount = SceneManager.sceneCount;
            Queue<Scene> scenesToUnload = new Queue<Scene>();
            for (int i = 0; i < currentScenesCount; i++)
            {
                var scene = SceneManager.GetSceneAt(i);
                if (scene.buildIndex != LOADING_SCENE_INDEX)
                {
                    scenesToUnload.Enqueue(scene);
                }
            }
            loggingService.Log($"Unloading {currentScenesCount} scenes");
            while (scenesToUnload.Count > 0)
            {
                var scene = scenesToUnload.Dequeue();
                loggingService.Log($"starting unload of {scene.name}");
                var unloadAction = SceneManager.UnloadSceneAsync(scene);
                while (!unloadAction.isDone)
                {
                    loggingService.Log($"waiting for unload of {scene.name}");
                    yield return null;
                }
                progress += 1f / currentScenesCount;
                UpdateLoadingBar(progress, $"Unloaded {scene.name}", loadingBar);
            }
            loggingService.Log($"Unloaded {currentScenesCount} scenes");
            progress = 0.5f;
            UpdateLoadingBar(progress, "Started loading new scene", loadingBar);

            //load new scene
            var newSceneLoadAction = SceneManager.LoadSceneAsync(buildIndex, LoadSceneMode.Additive);
            while (!newSceneLoadAction.isDone)
            {
                yield return null;
            }
            SceneManager.SetActiveScene(SceneManager.GetSceneByBuildIndex(buildIndex));
            UpdateLoadingBar(0.6f, "Scene loaded", loadingBar);
            PopulateHooks();
            sceneLoadedCallback?.Invoke();

            while (loadingHooks.Count > 0)
            {
                var hook = loadingHooks.Dequeue();
                UpdateLoadingBar(progress, hook.message, loadingBar);
                //update loading hook message
                yield return new WaitUntil(hook.condition);
            }
            UpdateLoadingBar(1f, "Load finished", loadingBar);
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
