using System;
using System.Collections.Generic;
using System.Globalization;
using Cysharp.Threading.Tasks;
using Newtonsoft.Json;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

namespace UI.Key
{
    public class KeyPopup : MonoBehaviour
    {
        private const string LicenseKey = "LicenseKey";
        private const string LicenseDate = "LicenseDate";
        private const string DateTimeFormat = "dd/MM/yyyy HH:mm:ss";
        private const string LicenceUrl = "https://platform-bpla.ru";
        private const string MainKeyStr = "ИДЕТ ПРОВЕРКА КЛЮЧЕЙ...";
        private const string TimeOutStr = "ПРОВЕРЬТЕ СОЕДИНЕНИЕ С ИНТЕРНЕТОМ";

        [SerializeField]
        private InputField inputField;

        [SerializeField]
        private Button confirmButton;

        [SerializeField]
        private Button pasteButton;

        [SerializeField]
        private Text message;

        [SerializeField]
        private GameObject mainImage;

        [SerializeField]
        private Text mainImageText;

        [SerializeField]
        private bool internetCheck;

        private const string AppAccessToken = "JwbYv4WYnr8cGBBMot1UaGg8EQvrKkdyG6uhjj5dlhzXu7RcTzACPf9X0SRJa" +
                                              "6L04bUzhbMydYUCklj7dlOzVsSrgCKskuypY0ttfDdfDDfcW0KUee1sHemofz" +
                                              "1HJ2fO3x80SDwgb6TgjnziThBnhqy7oTcUY15YQp2m0b3dTX8GCp0uPFhY8Kk" +
                                              "uwOISgrMCZdVtlMWSUw2YWy4Q3cphSgb04XfwwLYlyyVTrO9SMe0WGakihUGT" +
                                              "qVk0adbVJW5";

        private void Awake()
        {
            confirmButton.onClick.RemoveAllListeners();
            confirmButton.onClick.AddListener(OnConfirmButton);

            pasteButton.onClick.RemoveAllListeners();
            pasteButton.onClick.AddListener(OnPasteButton);
        }

        public void CheckLicense()
        {
            if (internetCheck)
            {
                CheckCurrentLicense().Forget();
            }
            else
            {
                gameObject.SetActive(false);
            }
            //gameObject.SetActive(GetLicenseDate() < DateTime.Now);
        }

        public DateTime GetLicenseDate()
        {
            if (!PlayerPrefs.HasKey(LicenseKey)) return DateTime.MinValue;

            var dateTimeStr = PlayerPrefs.GetString(LicenseKey);
            var styles = DateTimeStyles.None;
            return !DateTime.TryParseExact(dateTimeStr, DateTimeFormat, null, styles, out var savePrefDate)
                ? DateTime.MinValue
                : savePrefDate;
        }

        private void OnConfirmButton()
        {
            // OnRegister("{\"id\":1,\"created_at\":\"2024-02-07T08:50:02.000000Z\"," +
            //            "\"updated_at\":\"2024-02-07T08:50:02.000000Z\",\"deleted_at\":null," +
            //            "\"fingerprint\":\"68c8712f2f24c5865b7ff0769dd51e243b6f5b31\"," +
            //            "\"key\":{\"id\":5,\"created_at\":\"2024-02-07T08:42:52.000000Z\"," +
            //            "\"updated_at\":\"2024-02-07T08:50:02.000000Z\",\"deleted_at\":null," +
            //            "\"license_id\":1,\"key\":\"c5417002-2ce4-413c-a28e-3167506ab3bb\"," +
            //            "\"used\":1,\"expiry_date\":\"2024-06-01T08:37:57.000000Z\"}}");
            Register().Forget();
            // if (!_keys.Contains(inputField.text))
            // {
            //     message.text = "НЕВЕРНЫЙ КЛЮЧ";
            //     return;
            // }
            //
            // var licenceDate = DateTime.Now + TimeSpan.FromDays(365);
            // PlayerPrefs.SetString(LicenseKey, licenceDate.ToString(DateTimeFormat));
            // PlayerPrefs.Save();
            // gameObject.SetActive(false);
        }

        private async UniTask Register()
        {
            // check first
            //var key = "3b020e84-9192-46bc-9bd6-779e30dd678f";
            mainImageText.text = MainKeyStr;
            var key = inputField.text;
            if (string.IsNullOrEmpty(key)) key = "AAAAA";
            var hasLicence = await HasLicense(key);
            if (hasLicence)
            {
                PlayerPrefs.SetString(LicenseKey, key);
                PlayerPrefs.Save();
                gameObject.SetActive(false);
                return;
            }

            var fields = new Dictionary<string, string>
            {
                {"key", inputField.text},
                {"fingerprint", SystemInfo.deviceUniqueIdentifier}
            };
            var uri = $"{LicenceUrl}/api/v1/licenses/registration";
            var webRequest = UnityWebRequest.Post(uri, fields);
            webRequest.SetRequestHeader("x-app-access-token", AppAccessToken);
            try
            {
                await webRequest.SendWebRequest();

                if (webRequest.result == UnityWebRequest.Result.Success)
                {
                    var json = webRequest.downloadHandler.text;
                    Debug.Log($"JSON: {json}");
                    OnRegister(json);
                }
                else
                {
                    Debug.Log($"Error: {webRequest.result} {webRequest.error}");
                    message.text = "НЕВЕРНЫЙ КЛЮЧ";
                }
            }
            catch (Exception e)
            {
                Debug.Log($"Exception: {e}");
                message.text = "НЕВЕРНЫЙ КЛЮЧ";
            }
        }
        
        private void OnCheckLicense(string json)
        {
            var registerObj = JsonConvert.DeserializeObject<CheckResponse>(json);
            var licenceDate = DateTime.Parse(registerObj.License.Key.ExpiryDate);
            Debug.Log($"{registerObj.License.Key.Key} {registerObj.License.Key.ExpiryDate} {licenceDate}");
            PlayerPrefs.SetString(LicenseDate, licenceDate.ToString(DateTimeFormat));
            PlayerPrefs.SetString(LicenseKey, registerObj.License.Key.Key);
            PlayerPrefs.Save();
            gameObject.SetActive(false);
            mainImage.SetActive(false);
        }

        private void OnRegister(string json)
        {
            var registerObj = JsonConvert.DeserializeObject<License>(json);
            var licenceDate = DateTime.Parse(registerObj.Key.ExpiryDate);
            Debug.Log($"{registerObj.Key.Key} {registerObj.Key.ExpiryDate} {licenceDate}");
            PlayerPrefs.SetString(LicenseDate, licenceDate.ToString(DateTimeFormat));
            PlayerPrefs.SetString(LicenseKey, registerObj.Key.Key);
            PlayerPrefs.Save();
            gameObject.SetActive(false);
            mainImage.SetActive(false);
        }

        private void OnPasteButton()
        {
            inputField.text = GUIUtility.systemCopyBuffer;
        }

        private async UniTask CheckCurrentLicense()
        {
            mainImageText.text = MainKeyStr;
            mainImage.SetActive(true);
            gameObject.SetActive(true);
            var key = PlayerPrefs.GetString(LicenseKey, "AAAAA");
            await HasLicense(key);
        }

        private async UniTask<bool> HasLicense(string key)
        {
            var success = false;
            var fields = new Dictionary<string, string>
            {
                {"key", key},
                {"fingerprint", SystemInfo.deviceUniqueIdentifier}
            };
            var uri = $"{LicenceUrl}/api/v1/licenses/check";
            var webRequest = UnityWebRequest.Post(uri, fields);
            webRequest.SetRequestHeader("x-app-access-token", AppAccessToken);
            webRequest.timeout = 10;
            try
            {
                await webRequest.SendWebRequest();

                if (webRequest.result == UnityWebRequest.Result.Success)
                {
                    var json = webRequest.downloadHandler.text;
                    Debug.Log($"JSON: {json}");
                    Debug.Log("success");
                    OnCheckLicense(json);
                    success = true;
                }
                else
                {
                    mainImageText.text = TimeOutStr;
                    Debug.Log($"Error: {webRequest.result} {webRequest.error}");
                }
            }
            catch (Exception e)
            {
                mainImageText.text = TimeOutStr;
                Debug.Log($"Exception: {e}");
            }

            return success;
        }
    }

    [Serializable]
    public record CheckResponse(License License)
    {
        public License License { get; } = License;
    }

    [Serializable]
    public record License(
        int Id,
        string CreatedAt,
        string UpdatedAt,
        string DeletedAt,
        string Fingerprint,
        RegisterKey Key
    )
    {
        public int Id { get; } = Id;
        public string CreatedAt { get; } = CreatedAt;
        public string UpdatedAt { get; } = UpdatedAt;
        public string DeletedAt { get; } = DeletedAt;
        public string Fingerprint { get; } = Fingerprint;
        public RegisterKey Key { get; } = Key;
    }

    [Serializable]
    public record RegisterKey(
        int Id,
        string CreatedAt,
        string UpdatedAt,
        string DeletedAt,
        int LicenseId,
        string Key,
        int Used,
        string ExpiryDate
    )
    {
        public int Id { get; } = Id;
        public string CreatedAt { get; } = CreatedAt;
        public string UpdatedAt { get; } = UpdatedAt;
        public string DeletedAt { get; } = DeletedAt;
        public int LicenseId { get; } = LicenseId;
        public string Key { get; } = Key;
        public int Used { get; } = Used;

        [JsonProperty("expiry_date")]
        public string ExpiryDate { get; } = ExpiryDate;
    }
}