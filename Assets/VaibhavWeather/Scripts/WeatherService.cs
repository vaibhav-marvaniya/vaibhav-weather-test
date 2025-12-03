using System;
using System.Collections;
using System.Globalization;
using UnityEngine;
using UnityEngine.Networking;

namespace Vaibhav.Weather
{
    [Serializable]
    public class OpenMeteoDaily
    {
        public string[] time;
        public float[] temperature_2m_max;
    }

    [Serializable]
    public class OpenMeteoResponse
    {
        public OpenMeteoDaily daily;
    }

    public class WeatherService
    {
        private const string BaseUrl = "https://api.open-meteo.com/v1/forecast";
        public string BuildUrl(double latitude, double longitude, string timezone = "auto")
        {
            string lat = latitude.ToString(CultureInfo.InvariantCulture);
            string lon = longitude.ToString(CultureInfo.InvariantCulture);
            return $"{BaseUrl}?latitude={lat}&longitude={lon}&timezone={timezone}&daily=temperature_2m_max";
        }

        public IEnumerator GetTodayMaxTemperature(double latitude, double longitude, Action<float> onSuccess, Action<string> onError)
        {
            var url = BuildUrl(latitude, longitude);
            using (var request = UnityWebRequest.Get(url))
            {
                yield return request.SendWebRequest();
#if UNITY_2020_2_OR_NEWER
                if (request.result != UnityWebRequest.Result.Success)
#else
                if (request.isNetworkError || request.isHttpError)
#endif
                {
                    onError?.Invoke(request.error);
                    yield break;
                }
                var json = request.downloadHandler.text;
                if (string.IsNullOrEmpty(json))
                {
                    onError?.Invoke("Empty weather response");
                    yield break;
                }
                OpenMeteoResponse response;
                try
                {
                    response = JsonUtility.FromJson<OpenMeteoResponse>(json);
                }
                catch (Exception e)
                {
                    onError?.Invoke("Failed to parse weather JSON: " + e.Message);
                    yield break;
                }
                if (response == null || response.daily == null || response.daily.temperature_2m_max == null || response.daily.temperature_2m_max.Length == 0)
                {
                    onError?.Invoke("No temperature data in weather response");
                    yield break;
                }
                float todayMax = response.daily.temperature_2m_max[0];
                onSuccess?.Invoke(todayMax);
            }
        }

        public float ParseTodayMaxTemperature(string json)
        {
            var response = JsonUtility.FromJson<OpenMeteoResponse>(json);
            if (response == null || response.daily == null || response.daily.temperature_2m_max == null || response.daily.temperature_2m_max.Length == 0)
            {
                throw new System.Exception("No temperature data");
            }
            return response.daily.temperature_2m_max[0];
        }
    }
}
