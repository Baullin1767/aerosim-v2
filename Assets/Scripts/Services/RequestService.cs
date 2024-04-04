using System;
using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.Networking;

namespace Services
{
    public class RequestService : MonoBehaviour
    {
        private const int RequestTimeout = 10;
        private const string MainUrl = "https://platform-bpla.ru";
        private const string ApiPArt = "/api/v1/";

        private string _appAccessToken;

        public async UniTask<string> SendWebRequest(UnityWebRequest webRequest)
        {
            if (!string.IsNullOrEmpty(_appAccessToken))
                webRequest.SetRequestHeader("x-app-access-token", _appAccessToken);

            await webRequest.SendWebRequest();
            if (webRequest.result != UnityWebRequest.Result.Success) 
                throw new Exception("Request failed");
            
            var json = webRequest.downloadHandler.text;
            return json;
        }
    }
}