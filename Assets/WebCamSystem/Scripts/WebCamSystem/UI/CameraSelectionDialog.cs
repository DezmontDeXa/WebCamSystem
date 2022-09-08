using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using System.Collections.Generic;

namespace WebCamSystem.UI 
{
    public class CameraSelectionDialog : MonoBehaviour
    {
        [SerializeField] private GameObject _content;
        [SerializeField] private Transform _container;
        [SerializeField] private GameObject _cameraButtonPrefab;
        private List<Button> _buttons = new List<Button>();
        private UnityAction<string> _onResultAction;

        public void ShowDialog(UnityAction<string> onResultAction)
        {
            _onResultAction = onResultAction;
            _content.SetActive(true);
            DrawCameraButtons();
        }

        private void DrawCameraButtons()
        {
            foreach (var btn in _buttons)
            {
                btn.onClick.RemoveAllListeners();
                Destroy(btn);
            }

            foreach (var cam in WebCamTexture.devices)
                _buttons.Add(InstantiateCameraButton(cam));
        }

        private Button InstantiateCameraButton(WebCamDevice cam)
        {
            var go = Instantiate(_cameraButtonPrefab, _container);
            var name = cam.name;
            var btn = go.GetComponent<Button>();
            btn.onClick.AddListener(() => SelectCamera(name));
            btn.GetComponentInChildren<TMPro.TMP_Text>().text = name;
            return btn;
        }

        private void SelectCamera(string name)
        {
            _content.SetActive(false);
            _onResultAction?.Invoke(name);
        }
    }
}
