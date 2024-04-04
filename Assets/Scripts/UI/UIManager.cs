using UnityEngine;

namespace UI
{
    public class UIManager : MonoBehaviour
    {
        [SerializeField]
        private GameMenu gameMenu;

        [SerializeField]
        private SuccessMenu successMenu;

        [SerializeField]
        private FailMenu failMenu;

        private bool _isEnd;

        private void Awake()
        {
            gameMenu.gameObject.SetActive(false);
            if (successMenu != null) successMenu.gameObject.SetActive(false);
            if (failMenu != null) failMenu.gameObject.SetActive(false);
            _isEnd = false;
        }

        public void OpenGameMenu()
        {
            gameMenu.Open();
        }

        public void OpenSuccessMenu()
        {
            if (_isEnd) return;
            _isEnd = true;
            successMenu.Open();
        }

        public void OpenFailMenu()
        {
            if (_isEnd) return;
            _isEnd = true;
            failMenu.Open();
        }
    }
}