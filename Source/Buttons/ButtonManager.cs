using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ButtonManager : MonoBehaviour
{
    public void OnClickMenu()
    {
        SceneManager.LoadScene(0);
    }

    public void OnClickWeather()
    {
        SceneManager.LoadScene(1);
    }

    public void OnClickCamera()
    {
        SceneManager.LoadScene(2);
    }

    public void OnClickCat()
    {
        SceneManager.LoadScene(3);
    }

    public void OnClickDog()
    {
        SceneManager.LoadScene(4);
    }
}
