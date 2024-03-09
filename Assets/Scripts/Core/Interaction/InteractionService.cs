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
    internal class InteractionService : MonoBehaviour, IInteractionService
    {
        private bool initialised;
        private Dictionary<string, InputAction> inputActions;
        private Dictionary<string, InteractionMappingInstance> actionCallbacks;
        private ILoggingService loggingService;

        AsyncOperationHandle<InteractionConfiguration> handle;

        const string BASE_INTERACTION_PATH = "Assets/RuntimeAssets/Configurations/DefaultInteractionMappings.asset";
        const string INPUT_MODULE_PATH = "Assets/Prefabs/Management/InputModule.prefab";

        public void OnResourceCreating()
        {
            DontDestroyOnLoad(gameObject);
            GenerateInputModule();
            this.loggingService = ObjectFactory.ResolveService<ILoggingService>();
            CreateInitialActions();
            initialised = true;
        }

        void GenerateInputModule()
        {
            AssetManager.Instantiate<InputModule>(INPUT_MODULE_PATH, (inputModule) =>
            {
                loggingService.Log("Input module created");
            });
        }

        void CreateInitialActions()
        {
            inputActions = new Dictionary<string, InputAction>();
            actionCallbacks = new Dictionary<string, InteractionMappingInstance>();

            //load actions
            var defaultConfiguration = AssetManager.Load<InteractionConfiguration>(BASE_INTERACTION_PATH, out handle);
            foreach (var action in defaultConfiguration.InteractionMappings)
            {
                RegisterInputAction(action.Name, action);
            }
        }

        public void RegisterInputAction(string name, InteractionMapping inputMapping)
        {
            if (inputActions.ContainsKey(name))
            {
                loggingService.LogError($"Input action {name} already exists");
            }
            else
            {
                var mappingInstance = new InteractionMappingInstance(inputMapping);
                var inputAction = inputMapping.ActionMapping;
                inputAction.Enable();
                switch (inputMapping.ActionCallbackType)
                {
                    case InteractionMapping.MappingType.upDownAction:
                        inputAction.performed += (context) => OnAction(mappingInstance, context);
                        inputAction.canceled += (context) => OnAction(mappingInstance, context);
                        break;
                    case InteractionMapping.MappingType.triggered:
                        inputAction.started += (context) => OnAction(mappingInstance, context);
                        break;
                    case InteractionMapping.MappingType.performed:
                        inputAction.performed += (context) => OnAction(mappingInstance, context);
                        break;
                    case InteractionMapping.MappingType.canceled:
                        inputAction.canceled += (context) => OnAction(mappingInstance, context);
                        break;
                    case InteractionMapping.MappingType.value:
                        inputAction.performed += (context) => OnAction(mappingInstance, context);
                        inputAction.canceled += (context) => OnAction(mappingInstance, context);
                        break;
                }
                inputActions.Add(name, inputAction);
                actionCallbacks.Add(name, mappingInstance);
                loggingService.Log($"Registered input action {name} of type {inputMapping.ActionCallbackType}");
            }
        }

        #region event subscription
        public void SubscribeToInput(string inputName, Action callback)
        {
            if (actionCallbacks.ContainsKey(inputName))
            {
                actionCallbacks[inputName].ActionEvent += (obj, args) =>
                {
                    callback();
                };
            }
            else
            {
                loggingService.LogError($"Input action {inputName} not found");
            }
        }

        public void SubscribeToInput(string inputName, Action onActionCallback, Action onActionUpCallback)
        {
            if (actionCallbacks.ContainsKey(inputName))
            {
                if (actionCallbacks[inputName].Mapping.ActionCallbackType != InteractionMapping.MappingType.upDownAction)
                {
                    loggingService.LogError($"Input action {inputName} is not an up down action");
                    return;
                }
                actionCallbacks[inputName].ActionEvent += (obj, args) =>
                {
                    if (args.performed)
                    {
                        onActionCallback();
                    }
                    if (args.canceled)
                    {
                        onActionUpCallback();
                    }
                };
            }
            else
            {
                loggingService.LogError($"Input action {inputName} not found");
            }
        }

        public void SubscribeToInput(string inputName, Action<InputAction.CallbackContext> callback)
        {
            if (actionCallbacks.ContainsKey(inputName))
            {
                actionCallbacks[inputName].ActionEvent += (obj, args) =>
                {
                    callback(args);
                };
            }
            else
            {
                loggingService.LogError($"Input action {inputName} not found");
            }
        }

        public void SubscribeToInput<T>(string inputName, Action<T> callback) where T : struct
        {
            if (actionCallbacks.ContainsKey(inputName))
            {
                actionCallbacks[inputName].ActionEvent += (obj, args) =>
                {
                    var value = actionCallbacks[inputName].GetValueCallback<T>(args)();
                    callback(value);
                };
            } 
            else
            {
                loggingService.LogError($"Input action {inputName} not found");
            }
        }
        #endregion

        void OnAction(InteractionMappingInstance actionInstance, InputAction.CallbackContext context)
        {
            actionInstance.ActionEvent?.Invoke(this, context);
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
