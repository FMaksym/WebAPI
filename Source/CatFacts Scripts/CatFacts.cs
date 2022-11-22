using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Networking;
using TMPro;
using Newtonsoft.Json;

public class CatFacts : MonoBehaviour
{
    [SerializeField] private TMP_Text _textField;

    private void Start()
    {
        ShowFact();
    }

    public void OnClickNextFact()
    {
        ShowFact();
    }

    public void ShowFact()
    {
        string uri = "https://catfact.ninja/fact";
        StartCoroutine(GetFacts(uri));
    }

    IEnumerator GetFacts(string uri)
    {
        using UnityWebRequest webRequest = UnityWebRequest.Get(uri);
        yield return webRequest.SendWebRequest();
        switch (webRequest.result)
        {
            case UnityWebRequest.Result.ConnectionError:
                break;

            case UnityWebRequest.Result.Success:
                Fact _fact = JsonConvert.DeserializeObject<Fact>(webRequest.downloadHandler.text);
                _textField.text = _fact.fact;
                Debug.Log(_fact.fact);
                break;

            case UnityWebRequest.Result.DataProcessingError:
                Debug.LogError(String.Format("Somethink wet wrong: {0}", webRequest.error));
                break;
        }
    }



    public class Fact
    {
        public string fact;
        public int lenght;
    }
}
