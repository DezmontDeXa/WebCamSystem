using UnityEngine;
using WebCamSystem;
using WebCamSystem.UI;
using WebCamSystem.Core;

public class ShowDialogSample : MonoBehaviour
{
    [SerializeField] private WebCamera _webCam;
    private void Awake()
    {
        var dialog = GetComponent<CameraSelectionDialog>();
        dialog.ShowDialog(SelectCamera);
    }

    private void SelectCamera(string cameraName)
    {
        _webCam.WebCameraName = new WebCameraName(cameraName);
        _webCam.gameObject.SetActive(true);
    }
}
