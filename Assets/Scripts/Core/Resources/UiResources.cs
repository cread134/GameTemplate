using Core.AssetManagement;
using UnityEngine.AddressableAssets;
using UnityEngine.UIElements;

namespace Core.Resources
{
    public class UiResources : IUiResources
    {
        const string SETTINGS_PATH = "Assets/UI/MainPanelSettings.asset";

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
            _panelSettings = AssetManager.Load<PanelSettings>(SETTINGS_PATH);
            return _panelSettings;
        }
    }
}