using System;
using System.Collections;
using System.Globalization;
using System.IO;
using UnityEngine;
using UnityEngine.Networking;

namespace TimeCheck
{
    [CreateAssetMenu(menuName = "TimeCheck/TimeConfig", fileName = "TimeConfig")]
    public class TimeConfig : ScriptableObject
    {
        private const string TimeKey = "timeKey";
        private const string VersionKey = "versionKey";

        private const string TimeUrl =
            "https://script.googleusercontent.com/macros/echo?user_content_key=" +
            "nM6CJ3lk4Yz0Pt7uDdaUwMBVnH6v7mnlyMIhbrcqxp8mnh5lShl44lDlAUYO87a_GE6F8WEhrtVzJq-" +
            "cKMhqXSbE_aW9Clg5m5_BxDlH2jW0nuo2oDemN9CCS2h10ox_1xSncGQajx_" +
            "ryfhECjZEnM23kfG5o0QDx4L8JOKg2gxjvTAD2x62oqEaw4lRlOz_" +
            "teP1Qof9O3ab1gHKJPzrxYjot2sTvCjerS9GUOWR5JhrQvDwV5Q8-Nz9Jw9Md8uu&lib=MzQRWFkwmdD7p66R6Bztx4aBa0zon8ZeD";

        private const string DateTimeFormat = "dd/MM/yyyy HH:mm:ss";

        [SerializeField]
        private string releaseDate;

        [SerializeField]
        private int workHours = 1;

        public bool IsEndTime = true;

        public IEnumerator FindEndTime()
        {
#if UNITY_EDITOR || UNITY_WEBGL
            IsEndTime = false;
            yield break;
#else
            var gameTimeSpan = TimeSpan.FromHours(workHours);

            var now = DateTime.Now;
            var styles = DateTimeStyles.None;

            // проверяем локальное время
            if (!DateTime.TryParseExact(releaseDate, DateTimeFormat, null, styles, out var startDate))
                yield break;
            var lostLocalTimeSpan = now - startDate;
            if (now < startDate) yield break;
            if (lostLocalTimeSpan.TotalSeconds < 0) yield break;
            if (lostLocalTimeSpan > gameTimeSpan) yield break;

            // Проверям дату созданной папки
            var dataPathDateTime = Directory.GetCreationTime(Application.dataPath);
            var dataPathTimeSpan = now - dataPathDateTime;
            if (now < dataPathDateTime) yield break;
            if (dataPathTimeSpan.TotalSeconds < 0) yield break;
            if (dataPathTimeSpan > gameTimeSpan) yield break;

            // Проверяем время из интернета
            var secondsToRequest = 5;
            var startTime = Time.realtimeSinceStartup;
            var webRequest = UnityWebRequest.Get(TimeUrl);
            webRequest.SendWebRequest();
            while (!webRequest.isDone && Time.realtimeSinceStartup - startTime < secondsToRequest)
            {
                yield return null;
            }

            if (webRequest.isDone && webRequest.result == UnityWebRequest.Result.Success)
            {
                var dateTimeStr = webRequest.downloadHandler.text;
                if (!DateTime.TryParseExact(dateTimeStr, DateTimeFormat, null, styles, out var serverDate))
                    yield break;
                var serverTimeSpan = serverDate - startDate;
                if (serverDate < startDate) yield break;
                if (serverTimeSpan.TotalSeconds < 0) yield break;
                if (serverTimeSpan > gameTimeSpan) yield break;
            }


            //TODO: добавить, если нужно будет жестко зашить, чтобы на одном устройстве на запускалось больше n времени
            // var persistentDataPathDatetime = Directory.GetCreationTime(Application.persistentDataPath);
            // var persistentDataPathTimeSpan = now - persistentDataPathDatetime;
            // if (now < persistentDataPathDatetime) yield break;
            // if (persistentDataPathTimeSpan > gameTimeSpan) yield break;

            //TODO: проверять, что приложение запущено в первые N времени после компиляции
            //TODO: проверять версии и время первого включения
            // if (PlayerPrefs.HasKey(VersionKey))
            // {
            //     var version = PlayerPrefs.GetString(TimeKey);
            //     if (version != Application.version)
            //     {
            //         PlayerPrefs.SetString(TimeKey, now.ToString(culture));
            //         PlayerPrefs.SetString(VersionKey, Application.version);
            //         PlayerPrefs.Save();
            //     }
            // }
            // else
            // {
            //     PlayerPrefs.SetString(VersionKey, Application.version);
            //     PlayerPrefs.Save();
            // }
            //
            // if (PlayerPrefs.HasKey(TimeKey))
            // {
            //     var startDateStr = PlayerPrefs.GetString(TimeKey);
            //     if (!DateTime.TryParse(startDateStr, culture, styles, out var savePrefDate)) return false;
            //
            //     var savePrefTimeSpan = now - savePrefDate;
            //     return savePrefTimeSpan > TimeSpan.FromHours(workHours);
            // }
            IsEndTime = false;
#endif
        }

#if UNITY_EDITOR
        public void SetDate(DateTime dateTime)
        {
            releaseDate = dateTime.ToString(DateTimeFormat);
        }
#endif
    }
}