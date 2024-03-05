using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;
using UnityEngine.ResourceManagement.AsyncOperations;
using UnityEngine.AddressableAssets;
using Extension;
using Core.Resources;

namespace Core.Debugging
{
    public class DebugController : MonoBehaviour, IDebugController
    {
        const string VIEW_PATH = "Assets/UI/DebugUi/DebugInfoView.uxml";
        AsyncOperationHandle<VisualTreeAsset> _viewHandle;

        IUiResources _uiResources;

        public void OnResourceCreating()
        {
            _uiResources = ObjectFactory.ResolveService<IUiResources>();
            DontDestroyOnLoad(gameObject);
            StartCoroutine(GenerateCanvas());
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
            var document = DocumentFactory.CreateDocument("DebugCanvas", view, transform);
            document.panelSettings = _uiResources.PanelSettings;
        }
    }
}