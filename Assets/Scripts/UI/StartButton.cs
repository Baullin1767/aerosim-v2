using UnityEngine;
using UnityEngine.UI;

namespace UI
{
	[RequireComponent(typeof(Button))]
	public class StartButton : MonoBehaviour
	{
		[SerializeField]
		private bool _showIntro = false;

		private void Awake()
		{
			var button = GetComponent<Button>();
			button.onClick.AddListener(OnClick);
		}

		private void OnClick()
		{
			if (_showIntro)
				MissionManager.Intro();
			else
				MissionManager.Start();
		}

		private void OnRestart()
		{
			MissionManager.Start();
		}
	}
}
