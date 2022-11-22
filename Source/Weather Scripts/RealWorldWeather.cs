using System;
using System.Collections;
using Newtonsoft.Json.Linq;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;
using TMPro;

public class RealWorldWeather : MonoBehaviour 
{
	[SerializeField] private TMP_InputField _nameOfTheCity;
	[SerializeField] private TMP_Text _countryAndCity;
	[SerializeField] private TMP_Text _infoAboutWether;
	[SerializeField] private Image _weatherImage;
	[SerializeField] private Image _weatherImageBorder;
	[SerializeField] private string _apiKey = "xURSBN8w3zrxKQM+jOOHPA==D1ZeXHDthopJgW3H";


	[SerializeField] private Sprite _clearSprite;
	[SerializeField] private Sprite _rainSprite;
	[SerializeField] private Sprite _cloudsSprite;
	[SerializeField] private Sprite _snowSprite;
	//[SerializeField] private Sprite _sunnySprite;
	//[SerializeField] private Sprite _sunnySprite;


	private string _time;
	private string _city;
	private DateTime _dateTime;

    public void OnClickGetWeather () {
		string uri = "api.openweathermap.org/data/2.5/weather?";
		_city = _nameOfTheCity.text;
		uri += "q=" + _city + "&appid=" + _apiKey +"&units=metric" + "&lang=ua,ru,en";
        StartCoroutine(GetWeatherCoroutine(uri));
    }

    [Obsolete]
    IEnumerator GetWeatherCoroutine (string uri) {
        using UnityWebRequest webRequest = UnityWebRequest.Get(uri); 
		yield return webRequest.SendWebRequest();
        if (webRequest.isNetworkError)
        {
            Debug.Log("Web request error: " + webRequest.error);
        }else{
            ParseJson(webRequest.downloadHandler.text);
        }
    }

	DateTime LastUpdateTime()
    {
		_dateTime = DateTime.Now;
		return _dateTime;
    }

	WeatherStatus ParseJson (string json) {
		WeatherStatus weather = new();
		try {
			dynamic obj = JObject.Parse(json);

			weather.country = obj.sys.country;
			weather.city = obj.name;
			
			weather.weatherId = obj.weather[0].id;
		    weather.main = obj.weather[0].main;
			weather.description = obj.weather[0].description;

			weather.temperature = obj.main.temp;
			weather.temperatureFeelsLike = obj.main.feels_like;
			weather.pressure = obj.main.pressure;
			weather.humidity = obj.main.humidity;

			weather.windSpeed = obj.wind.speed;
			weather.windDirectionValue = obj.wind.deg;

			weather.data = obj.dt;
			weather.timeZone = obj.timezone / 3600;
			weather.visibility = obj.visibility;

			_time = Convert.ToString(weather.timeZone);
		} catch (Exception e) {
			Debug.Log (e.StackTrace);
		}

		if (weather.country == null)
		{
			_countryAndCity.text = "???";
			_infoAboutWether.text = "Error! Not found. Try agin.";
        }else{
			_countryAndCity.text = weather.country + "," + weather.city;
			_infoAboutWether.text = "Last Update: " + LastUpdateTime() + "\n" +
									"TimeZone: " + weather.timeZone + " h \n" +
									"Temp in °C: " + weather.temperature + " °C \n" +
									"Feels Like: " + weather.temperatureFeelsLike + " °C \n" +

									"Weather id: " + weather.weatherId + "\n" +
									"Weather main: " + weather.main + "\n" +
									"Weather description: " + weather.description + "\n" +

									"Wind speed: " + weather.windSpeed + " m/s \n" +
									"Wind direction: " + weather.windDirectionValue + "° \n" +

									"Pressure: " + weather.pressure + " hPa \n" +
									"Humidity: " + weather.humidity + " %\n" +
									"Visibility: " + weather.visibility + " metres \n";

			WeatherImages(weather.main);

		}
		return weather;
	}

    public void WeatherImages(string status)//Clear, Clouds, Rain, Snow
    {
		
        if (status == "Clear")
        {
			_weatherImage.sprite = _clearSprite;
        }
        else if (status == "Clouds")
        {
			_weatherImage.sprite = _cloudsSprite;
		}
        else if (status == "Rain")
        {
			_weatherImage.sprite = _rainSprite;
		}
		else if (status == "Snow")
		{
			_weatherImage.sprite = _snowSprite;
		}

		_weatherImageBorder.gameObject.SetActive(true);
		_weatherImage.gameObject.SetActive(true);
	}

    public class WeatherStatus
	{
		public string country;
		public string city;
		public string id;
		public int data;
		public int timeZone;
		public int weatherId;
		public string main;
		public string description;
		public int temperature;
		public int temperatureFeelsLike;
		public float pressure;
		public float windSpeed;
		public float windDirectionValue;
		public float windDirection;
		public float humidity;
		public float visibility;

		public float CelsiusTemp()
		{
			return (temperature - 273.15f) * 1;
		}

		public float CelsiusTempFeels()
		{
			return (temperatureFeelsLike - 273.15f) * 1;
		}

		public string Data(int epoch)
		{
			return new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc).AddSeconds(epoch).ToShortDateString();
		}
	}
}