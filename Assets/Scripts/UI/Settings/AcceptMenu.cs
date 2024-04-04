using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace UI.Settings
{
    public class AcceptMenu : MonoBehaviour
    {
        [SerializeField]
        private Button closeButton;

        [SerializeField]
        private Button cancelButton;

        [SerializeField]
        private Button acceptButton;

        private UnityAction _onClose;
        private UnityAction _onCancel;
        private UnityAction _onAccept;

        public void Initialize(UnityAction onClose, UnityAction onCancel, UnityAction onAccept)
        {
            _onClose = onClose;
            _onCancel = onCancel;
            _onAccept = onAccept;
            gameObject.SetActive(true);
        }

        private void Awake()
        {
            closeButton.onClick.RemoveAllListeners();
            closeButton.onClick.AddListener(OnCloseButton);

            cancelButton.onClick.RemoveAllListeners();
            cancelButton.onClick.AddListener(OnCancelButton);

            acceptButton.onClick.RemoveAllListeners();
            acceptButton.onClick.AddListener(OnAcceptButton);
        }

        private void OnCloseButton()
        {
            _onClose?.Invoke();
            gameObject.SetActive(false);
        }

        private void OnCancelButton()
        {
            _onCancel?.Invoke();
            gameObject.SetActive(false);
        }

        private void OnAcceptButton()
        {
            _onAccept?.Invoke();
            gameObject.SetActive(false);
        }
    }
}