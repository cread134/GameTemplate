using Core.AssetManagement;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;
using UnityEngine.UIElements;

namespace Core.Resources
{
    public class UiResources : IUiResources
    {
        const string SETTINGS_PATH = "Assets/UI/MainPanelSettings.asset";
        AsyncOperationHandle<PanelSettings> handle;

        ~UiResources()
        {
            if (handle.IsValid())
            {
                Addressables.Release(handle);
            }
        }

        public void OnResourceCreating()
        {
            if (_panelSettings == null)
            {
                _panelSettings = LoadPanelSettings();
            }
        }

        public PanelSettings PanelSettings
        {
            get
            {
                return _panelSettings ?? LoadPanelSettings();
            }
        }
        private PanelSettings _panelSettings;

        private PanelSettings LoadPanelSettings()
        {
            _panelSettings = AssetManager.Load<PanelSettings>(SETTINGS_PATH, out handle);
            return _panelSettings;
        }
    }
}