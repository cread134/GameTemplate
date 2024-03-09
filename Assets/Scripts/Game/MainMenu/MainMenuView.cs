using Core.Resources;
using Core.SceneManagement;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

namespace Game
{
    public class MainMenuView : MonoBehaviour
    {
        [SerializeField] private UIDocument mainMenuView;
        [SerializeField] private GameScene sampleScene;

        private ISceneLoader sceneLoader;
        
        private void Start()
        {
            mainMenuView.rootVisualElement.Q<Button>("EnterGameButton").clicked += OnGameButtonClicked;
            sceneLoader = ObjectFactory.ResolveService<ISceneLoader>();
        }

        private void OnGameButtonClicked()
        {
            sceneLoader.LoadScene(sampleScene);
        }
    }
}
