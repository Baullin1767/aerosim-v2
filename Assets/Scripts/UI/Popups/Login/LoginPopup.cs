using System;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using Extensions;
using Services;
using UnityEngine;
using UnityEngine.UI;

namespace UI.Popups.Login
{
    public class LoginPopup : MonoBehaviour
    {
        [SerializeField]
        private Button authorizationButton;

        [SerializeField]
        private Button registrationButton;

        [SerializeField]
        private Button buyLicenceButton;

        [SerializeField]
        private Button supportButton;

        [SerializeField]
        private GameObject authorizationBlock;

        [SerializeField]
        private GameObject registrationBlock;

        private void Awake()
        {
            // authorizationButton.SetClickListener(OnAuthorizationButton);
            // registrationButton.SetClickListener(OnRegistrationButton);
            buyLicenceButton.SetClickListener(OnBuyLicenceButton);
            //supportButton.SetClickListener(OnSupportButton);
        }

        private void OnAuthorizationButton()
        {
            authorizationBlock.SetActive(true);
            registrationBlock.SetActive(false);
        }

        private void OnRegistrationButton()
        {
            authorizationBlock.SetActive(false);
            registrationBlock.SetActive(true);
        }

        private void OnBuyLicenceButton()
        {
            SendRegister().Forget();


            async UniTask SendRegister()
            {
                try
                {
                    await AuthService.Instance.Register("qwe",
                        "qwe",
                        "qwe", "qwe",
                        "qwe", "qwe",
                        "qwe");
                }
                catch (Exception e)
                {
                    Debug.Log("EXCEPTION");
                    Console.WriteLine(e);
                    throw;
                }
            }
        }

        private void OnSupportButton()
        {
            Application.OpenURL("https://t.me/air_tek");
        }

        // [TextArea]
        // public string cities;
        //
        // public int num = 1;
        //
        // private void OnValidate()
        // {
        //     if (num != 7) return;
        //     Debug.Log(cities);
        //     var raws = cities.Split("\n");
        //     var nn = "";
        //     foreach (var raw in raws)
        //     {
        //         var newW = raw.Replace("<option value=\"", "");
        //         newW = newW.Replace("</option>", "");
        //         newW = newW.Replace("\">", "");
        //         newW = newW.Replace(" ", "");
        //         newW = newW.Substring(0, newW.Length / 2);
        //         nn += $"\"{newW}\",\n";
        //     }
        //
        //     Debug.Log(nn);
        // }
    }

    public class RegistrationBlock : MonoBehaviour
    {
        [SerializeField]
        private CheckableInputField nickNameField;

        [SerializeField]
        private CheckableInputField emailField;

        [SerializeField]
        private CheckableInputField passwordField;

        [SerializeField]
        private CheckableInputField passwordRepeatField;
    }

    public class AuthorizationBlock
    {
    }
}