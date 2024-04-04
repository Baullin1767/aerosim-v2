using System;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.Networking;

namespace Services
{
    public class AuthService : MonoBehaviour
    {
        [SerializeField]
        private bool active = true;

        [SerializeField]
        private GameObject loginPopup;
        
        [SerializeField]
        private Canvas parentCanvas;
        
        [SerializeField]
        private RequestService requestService;

        private static AuthService _instance;
        
        private GameObject _loginPopupInstance;

        public static AuthService Instance
        {
            get
            {
                if (_instance == null) _instance = FindObjectOfType<AuthService>();
                return _instance;
            }
        }

        private void Awake()
        {
            _instance = this;
            if(active) ShowLoginPopup();
        }
        
        private void ShowLoginPopup()
        {
            if (_loginPopupInstance == null)
            {
                _loginPopupInstance = Instantiate(loginPopup, parentCanvas.transform);
            }

            _loginPopupInstance.transform.SetAsLastSibling();
            _loginPopupInstance.gameObject.SetActive(true);
        }

        public async UniTask Register(
            string login,
            string password,
            string passwordConfirmation,
            string email,
            string userName,
            string userSurname,
            string city)

        {
            var registerURL = "/register123";
            var fields = new Dictionary<string, string>
            {
                {"login", login},
                {"password", password},
                {"password_confirmation", passwordConfirmation},
                {"email", email},
                {"name", userName},
                {"surname", userSurname},
                {"city", city}
            };
            var webRequest = UnityWebRequest.Post(registerURL, fields);

            await requestService.SendWebRequest(webRequest);
        }
    }
}