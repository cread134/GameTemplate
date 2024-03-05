using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;
using UnityEngine.ResourceManagement.AsyncOperations;
using UnityEngine.AddressableAssets;
using Extension;
using Core.Resources;
using System;
using System.Text;
using UnityEngine.InputSystem;

namespace Core.Debugging
{
    public class DebugController : MonoBehaviour, IDebugController
    {
        const string VIEW_PATH = "Assets/UI/DebugUi/DebugInfoView.uxml";

        AsyncOperationHandle<VisualTreeAsset> _viewHandle;
        IUiResources _uiResources;
        Label _debugInfo;
        Dictionary<string, Func<string>> _subscribedViews = new Dictionary<string, Func<string>>();

        bool initialised = false;

        private UIDocument document;

        public void OnResourceCreating()
        {
            _uiResources = ObjectFactory.ResolveService<IUiResources>();
            var eventService = ObjectFactory.ResolveService<IEventService>();
            eventService.OnLateUpdate += (sender, args) => RenderView();

            DontDestroyOnLoad(gameObject);
            StartCoroutine(GenerateCanvas());
        }

        public void SubscribeView(string name, Func<string> message)
        {
            _subscribedViews ??= new Dictionary<string, Func<string>>();
            if(_subscribedViews.ContainsKey(name))
            {
                _subscribedViews[name] = message;
                return;
            }
            _subscribedViews.Add(name, message);

        }

        public void Unsubscribe(string name)
        {
            if (_subscribedViews == null) return;
            if(_subscribedViews.ContainsKey(name))
            {
                _subscribedViews.Remove(name);
            }
        }

        void RenderView()
        {
            if (!initialised) return;
            var sb = new StringBuilder();
            foreach (var view in _subscribedViews)
            {
                sb.AppendLine($"{view.Key} : {view.Value()}");
            }
            _debugInfo.text = sb.ToString();
        }

        IEnumerator GenerateCanvas()
        {
            yield return LoadViewAsync();
            CreateCanvas(_viewHandle.Result);
        }

        IEnumerator LoadViewAsync()
        {
            var handle = Addressables.LoadAssetAsync<VisualTreeAsset>(VIEW_PATH);
            yield return handle;
            _viewHandle = handle;
        }

        void CreateCanvas(VisualTreeAsset view)
        {
            document = DocumentFactory.CreateDocument("DebugCanvas", view, transform);
            document.panelSettings = _uiResources.PanelSettings;

            _debugInfo = document.rootVisualElement.Q<Label>("debug-info");
            initialised = true;   
        }
    }
}