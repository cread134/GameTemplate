using UnityEngine;
using UnityEngine.UIElements;

namespace Core.SceneManagement.Ui
{
    [RequireComponent(typeof(UIDocument))]
    public class LoadingBarView : MonoBehaviour
    {
        private UIDocument document;

        private void Awake()
        {
            this.document = GetComponent<UIDocument>();
        }

        private ProgressBar _progressBar;
        ProgressBar ProgressBar
        {
            get
            {
                return _progressBar ?? (_progressBar = document.rootVisualElement.Q<ProgressBar>("Loading_Progress")); 
            }
        }
        public void SetProgress(float progress, string message)
        {
            ProgressBar.value = progress;
            ProgressBar.title = message;
        }
    }
}