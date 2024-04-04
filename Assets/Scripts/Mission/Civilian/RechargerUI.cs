using Cysharp.Threading.Tasks;
using Extensions;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

namespace Mission.Civilian
{
    public class RechargerUI : MonoBehaviour
    {
        [SerializeField]
        private Button balloonButton;

        [SerializeField]
        private Button medKitButton;

        [SerializeField]
        private GameObject balloonButtonSelected;
        
        [SerializeField]
        private GameObject medKitButtonSelected;

        private UniTaskCompletionSource<Recharger.ChargerType> _chargeSource;

        private bool _balloonSelected;

        private void Awake()
        {
            balloonButton.SetClickListener(OnBalloonButton);
            medKitButton.SetClickListener(OnMedKitButton);

            SelectBalloons();
        }

        public async UniTask<Recharger.ChargerType> GetChargerType()
        {
            gameObject.SetActive(true);
            _chargeSource = new UniTaskCompletionSource<Recharger.ChargerType>();
            return await _chargeSource.Task;
        }

        public void Hide()
        {
            _chargeSource?.TrySetResult(Recharger.ChargerType.None);
            gameObject.SetActive(false);
        }

        private void OnBalloonButton()
        {
            _chargeSource.TrySetResult(Recharger.ChargerType.BalloonСо2);
            gameObject.SetActive(false);
        }

        private void OnMedKitButton()
        {
            _chargeSource.TrySetResult(Recharger.ChargerType.MedKit);
            gameObject.SetActive(false);
        }

        private void SelectBalloons()
        {
            balloonButtonSelected.SetActive(true);
            medKitButtonSelected.SetActive(false);
            balloonButton.gameObject.SetActive(false);
            medKitButton.gameObject.SetActive(true);
        }
        
        private void SelectMedkit()
        {
            balloonButtonSelected.SetActive(false);
            medKitButtonSelected.SetActive(true);
            balloonButton.gameObject.SetActive(true);
            medKitButton.gameObject.SetActive(false);
        }

        private void OnDrop()
        {
            if (_balloonSelected)
            {
                OnBalloonButton();
            }
            else
            {
                OnMedKitButton();
            }
        }
        
        private void OnYaw(InputValue value)
        {
            var yaw = value.Get<float>();
        }
    }
}