using System.IO;
using UnityEngine;
using UnityEngine.UI;

public class CameraController : MonoBehaviour
{
    [SerializeField] private int _cameraIndex = 0;
    [SerializeField] private RawImage _rawImage;

    private WebCamTexture _texture;

    private void Awake()
    {
        if (_cameraIndex == 0)
        {
            CameraRotator();
            GetTexture();
        } 
    }

    public void FlipCamera()
    {
        if (WebCamTexture.devices.Length > 0)
        {
            _cameraIndex += 1;
            _cameraIndex %= WebCamTexture.devices.Length;

            CameraRotator();
            GetTexture();

        }
    }


    private void CameraRotator()
    {
        if (_cameraIndex == 0)
        {
            _rawImage.transform.rotation = Quaternion.Euler(0, 0, -90);
        }
        else if (_cameraIndex == 1)
        {
            _rawImage.transform.rotation = Quaternion.Euler(0, 0, 90);
        }
    }

    public void MakeFoto()
    {
        Texture2D texture = new Texture2D(_rawImage.texture.width, _rawImage.texture.height, TextureFormat.ARGB32, false);

        texture.SetPixels(_texture.GetPixels());
        texture.Apply();

        byte[] bytes = texture.EncodeToJPG();

        File.WriteAllBytes("/storage/emulated/0/DCIM/Camera/" + "IMG_" + name + ".jpg", bytes);
    }

    private void GetTexture()
    {
        WebCamDevice device = WebCamTexture.devices[_cameraIndex];
        _texture = new WebCamTexture(device.name);
        _rawImage.texture = _texture;

        _texture.Play();
    }
}
