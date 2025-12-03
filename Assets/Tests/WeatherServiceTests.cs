using NUnit.Framework;
using Vaibhav.Weather;

public class WeatherServiceTests
{
    [Test]
    public void ParseTodayMaxTemperature_ReturnsFirstDailyValue()
    {
        string json = @"
        {
            ""daily"": 
            {
                ""time"": [""2025-12-03""],
                ""temperature_2m_max"": [28.4]
            }
        }";

        var service = new WeatherService();
        float temp = service.ParseTodayMaxTemperature(json);
        Assert.AreEqual(28.4f, temp);
    }
}
