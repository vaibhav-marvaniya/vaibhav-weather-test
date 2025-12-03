using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using Vaibhav.Toasts;
using Vaibhav.Weather;

public class WeatherManager : MonoBehaviour
{
    [Header("UI References")]
    [SerializeField] private Text latitudeText;
    [SerializeField] private Text longitudeText;
    [SerializeField] private Text temperatureText;
    [SerializeField] private Text statusText;
    [Header("Toast Integration")]
    [SerializeField] private ToastButton toastButton;
    [Header("Location Settings")]
    [SerializeField] private float locationTimeoutSeconds = 20f;
    [SerializeField] private double fallbackLatitude = 19.07;
    [SerializeField] private double fallbackLongitude = 72.87;
    private WeatherService _weatherService;
    private double _latitude;
    private double _longitude;

    private void Awake()
    {
        _weatherService = new WeatherService();
    }

    private void Start()
    {
        StartCoroutine(InitAndLoadWeather());
    }

    public void OnRefreshButtonClicked()
    {
        StartCoroutine(InitAndLoadWeather());
    }

    private IEnumerator InitAndLoadWeather()
    {
        SetStatus("Initializing location...");
        yield return StartCoroutine(GetLocation());
        UpdateLocationUI();
        SetStatus("Fetching weather...");
        yield return StartCoroutine(FetchWeather());
    }

    private IEnumerator GetLocation()
    {
        if (!Input.location.isEnabledByUser)
        {
            SetStatus("Location disabled, using fallback coordinates.");
            _latitude = fallbackLatitude;
            _longitude = fallbackLongitude;
            yield break;
        }
        Input.location.Start();
        float timer = 0f;
        while (Input.location.status == LocationServiceStatus.Initializing &&
               timer < locationTimeoutSeconds)
        {
            timer += Time.deltaTime;
            yield return null;
        }
        if (Input.location.status != LocationServiceStatus.Running)
        {
            SetStatus("Location failed, using fallback coordinates.");
            _latitude = fallbackLatitude;
            _longitude = fallbackLongitude;
            Input.location.Stop();
            yield break;
        }
        var last = Input.location.lastData;
        _latitude = last.latitude;
        _longitude = last.longitude;
        SetStatus("Location acquired.");
        Input.location.Stop();
    }

    private IEnumerator FetchWeather()
    {
        bool completed = false;
        float temperatureC = 0f;
        string error = null;
        yield return StartCoroutine(_weatherService.GetTodayMaxTemperature(_latitude, _longitude,t =>
            {
                completed = true;
                temperatureC = t;
            },
            e =>
            {
                completed = true;
                error = e;
            }));
        if (!completed)
        {
            yield break;
        }
        if (!string.IsNullOrEmpty(error))
        {
            Debug.LogWarning("Weather error: " + error);
            SetStatus("Failed to load weather.");
            ToastManager.Instance.ShowMessage("Failed to load weather.");
            yield break;
        }
        if (temperatureText != null)
        {
            temperatureText.text = $"{temperatureC:F1} °C";
        }
        string toastMessage = $"Today's max temperature: {temperatureC:F1} °C";
        ToastManager.Instance.ShowMessage(toastMessage);
        if (toastButton != null)
        {
            toastButton.SetMessage(toastMessage);
        }
        SetStatus("Weather updated.");
    }

    private void UpdateLocationUI()
    {
        if (latitudeText != null)
        {
            latitudeText.text = _latitude.ToString("F4");
        }
        if (longitudeText != null)
        {
            longitudeText.text = _longitude.ToString("F4");
        }
    }

    private void SetStatus(string message)
    {
        if (statusText != null)
        {
            statusText.text = message;
        }
    }
}
