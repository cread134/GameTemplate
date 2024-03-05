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
            var handle = Addressables.LoadAssetAsync<PanelSettings>(SETTINGS_PATH);
            handle.Completed += (op) =>
            {
                _panelSettings = op.Result;
            };
            handle.WaitForCompletion();
            Addressables.Release(handle);
            return _panelSettings;
        }
    }
}