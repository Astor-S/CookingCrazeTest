using UnityEngine;
using UnityEngine.UI;
using CookingPrototype.Controllers;
using TMPro;

namespace CookingPrototype.UI
{
	public sealed class StartWindow : MonoBehaviour
	{
		[SerializeField] private TMP_Text _goalText;
		[SerializeField] private Button _playButton;
		[SerializeField] private PauseHandler _pauseHandler;

		private void OnEnable()
		{
			GameplayController.Instance.TotalOrdersServedChanged += UpdateGoalText;
		}

		private void OnDisable()
		{
			GameplayController.Instance.TotalOrdersServedChanged -= UpdateGoalText;
		}

		public void Show()
		{
			gameObject.SetActive(true);

			if (_playButton != null)
			{
				_playButton.onClick.RemoveAllListeners();
				_playButton.onClick.AddListener(StartGame);
			}
		}

		public void Hide() =>
			gameObject.SetActive(false);

		private void UpdateGoalText()
		{
			if ( GameplayController.Instance != null )
				_goalText.text = $"{GameplayController.Instance.OrdersTarget}";
		}

		private void StartGame()
		{
			_pauseHandler.ContinueGame();
			Hide();
		}
	}
}