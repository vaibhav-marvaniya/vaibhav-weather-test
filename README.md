# CleverTap Unity Weather App

This repository contains the **Weather App** part of the CleverTap Unity SDK test.

The app:

- Gets latitude and longitude (using Unity `Input.location`).
- Calls the **Open-Meteo** API to fetch today’s maximum temperature.
- Shows the result in the UI and via the **toast package** GameObject.

The toast functionality itself is developed in a separate repo (CleverTap Unity Toast Package) and is imported here as a Unity package / folder under `Assets/VaibhavToast`.

---

## How to Run

1. Open this project in **Unity**.
2. Open the main scene, e.g. `Assets/VaibhavWeather/Scenes/TestScene.unity`.
3. Press **Play**:
   - Lat / Lon / Temp are shown on screen.
   - Press the **Toast** button to show the temperature via the toast system
     (in Editor this logs, on device it uses native Android/iOS).

To test on a real device:

1. Switch platform to **Android** or **iOS** in Build Settings.
2. Build and run on a device with internet and location services enabled.

---

## Architecture (Short)

- **Weather**
  - `WeatherManager`:
    - Gets location.
    - Calls `WeatherService` to fetch weather.
    - Updates UI.
    - Sends message to the toast system.
  - `WeatherService`:
    - Builds Open-Meteo URL.
    - Parses the weather JSON.

- **Toast integration**
  - Uses the `ToastManager` / `ToastButton` from the toast package
    to show messages like “Today’s max temperature: 28.4 °C”.

---

## Unit Tests

This project uses the Unity Test Framework (EditMode tests).

- `WeatherServiceTests`:
  - Verifies JSON parsing for the Open-Meteo response.

Run via: **Window → General → Test Runner → EditMode → Run All**.
