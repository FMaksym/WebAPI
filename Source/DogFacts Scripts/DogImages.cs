using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Networking;
using Newtonsoft.Json.Linq;
using UnityEngine.UI;

public class DogImages : MonoBehaviour
{
    [SerializeField] private Image _image;

    private void Start()
    {
        ShowImage();
    }

    public void OnClickNextImage()
    {
        ShowImage();
    }

    public void ShowImage()
    {
        string uri = "https://dog.ceo/api/breeds/image/random";

        StartCoroutine(GetImage(uri));
    }

    IEnumerator GetImage(string uri)
    {
        UnityWebRequest webRequest = UnityWebRequest.Get(uri);
        yield return webRequest.SendWebRequest();
        switch (webRequest.result)
        {
            case UnityWebRequest.Result.ConnectionError:
                break;

            case UnityWebRequest.Result.Success:
                ParseJObject(webRequest.downloadHandler.text);
                break;

            case UnityWebRequest.Result.DataProcessingError:
                Debug.LogError(String.Format("Somethink wet wrong: {0}", webRequest.error));
                break;
        }
    }

    DodsImage ParseJObject(string json)
    {
        DodsImage image = new();
        try
        {
            dynamic obj = JObject.Parse(json);

            image.message = obj.message;
            image.status = obj.status;

            //Debug.Log(image.message);
            StartCoroutine(GetImageFromURL(image.message));
        }
        catch (Exception e)
        {
            Debug.Log(e.StackTrace);
        }
        return image;
    }

    IEnumerator GetImageFromURL(string url)
    {
        UnityWebRequest _webRequest = UnityWebRequestTexture.GetTexture(url);
        yield return _webRequest.SendWebRequest();

        Texture2D texture = ((DownloadHandlerTexture)_webRequest.downloadHandler).texture;
        _image.sprite = Sprite.Create(texture, new Rect(0, 0, texture.width, texture.height), Vector2.zero);
    }

    public class DodsImage
    {
        public string message;
        public string status;
    }
}
