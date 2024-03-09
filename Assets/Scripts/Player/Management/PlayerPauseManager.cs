using Core.PauseManagement;
using Player.Interaction;
using Player.PlayerResources;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

namespace Player.Management
{
    public class PlayerPauseManager : MonoBehaviour, IPlayerBehaviour
    {
        [SerializeField] public UIDocument pauseMenu;

        private bool initialized = false;
        private IPlayerMouseLook playerMouseLook;

        public void OnBehaviourInit(IPlayerController playerController, IPlayerResources playerResources)
        {
            playerController.OnPauseButton += OnPauseButton;
            playerMouseLook = playerResources.GetBehaviourResource<PlayerMouseLook>();

            InitMenu();
            initialized = true;
        }

        void InitMenu()
        {
            pauseMenu.rootVisualElement.style.display = DisplayStyle.None;

            var returnToGameButton = pauseMenu.rootVisualElement.Q<Button>("ReturnToGameButton");
            returnToGameButton.clicked += EndPause;
            var mainMenuButton = pauseMenu.rootVisualElement.Q<Button>("MainMenuButton");
            mainMenuButton.clicked += OnMainMenuButton;
        }

        private void OnMainMenuButton()
        {
            throw new NotImplementedException();
        }

        private void OnPauseButton(object sender, EventArgs e)
        {
            if (!initialized)
            {
                return;
            }
            bool shouldPause = !PauseManager.IsPaused;
            if (shouldPause)
            {
                StartPause();
            }
            else
            {
                EndPause();
            }

        }
        public void StartPause()
        {
            PauseManager.SetPause();
            playerMouseLook.SetCursorLock(false);
            pauseMenu.rootVisualElement.style.display = DisplayStyle.Flex;
        }
        public void EndPause()
        {
            PauseManager.UnsetPause();
            playerMouseLook.SetCursorLock(true);
            pauseMenu.rootVisualElement.style.display = DisplayStyle.None;
        }

        public void StartBehaviour()
        {
        }
    }
}
