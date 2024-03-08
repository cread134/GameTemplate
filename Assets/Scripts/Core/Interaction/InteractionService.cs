using Core.AssetManagement;
using Core.Logging;
using Core.Resources;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.InputSystem;
using UnityEngine.ResourceManagement.AsyncOperations;

namespace Core.Interaction
{
    public class InteractionService : MonoBehaviour, IInteractionService
    {
        private bool initialised;
        private Dictionary<string, InputAction> inputActions;
        private Dictionary<string, EventHandler> actionCallbacks;
        private ILoggingService loggingService;

        AsyncOperationHandle<InteractionConfiguration> handle;
        const string BASE_INTERACTION_PATH = "Assets/RuntimeAssets/Configurations/DefaultInteractionMappings.asset";

        public void OnResourceCreating()
        {
            this.loggingService = ObjectFactory.ResolveService<ILoggingService>();
            DontDestroyOnLoad(gameObject);
            CreateInitialActions();
            initialised = true;
        }

        void CreateInitialActions()
        {
            inputActions = new Dictionary<string, InputAction>();
            actionCallbacks = new Dictionary<string, EventHandler>();

            //load actions
            var defaultConfiguration = AssetManager.Load<InteractionConfiguration>(BASE_INTERACTION_PATH, out handle);
            foreach (var action in defaultConfiguration.InteractionMappings)
            {
                RegisterInputAction(action.Name, action.ActionMapping);
            }
        }

        public void RegisterInputAction(string name, InputAction inputAction)
        {
            if (inputActions.ContainsKey(name))
            {
                loggingService.LogError($"Input action {name} already exists");
            }
            else
            {
                inputAction.Enable();
                inputAction.performed += (context) => OnActionTriggered(name);
                inputActions.Add(name, inputAction);
                actionCallbacks.Add(name, (obj, args) => { });
                loggingService.Log($"Registered input action {name}");
            }
        }

        public void SubscribeToInput(string inputName, Action callback)
        {
            if (actionCallbacks.ContainsKey(inputName))
            {
                actionCallbacks[inputName] += (obj, args) => { callback(); };
            } 
            else
            {
                loggingService.LogError($"Input action {inputName} not found");
            }
        }

        void OnActionTriggered(string actionName)
        {
            actionCallbacks[actionName]?.Invoke(this, EventArgs.Empty);
        }

        private void OnDestroy()
        {
            if (initialised)
            {
                foreach (var action in inputActions)
                {
                    action.Value.Disable();
                }
            }
            if(handle.IsValid())
            {
                Addressables.Release(handle);
            }
        }
    }
}
