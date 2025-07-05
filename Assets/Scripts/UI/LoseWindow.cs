using UnityEngine;
using UnityEngine.UI;
using CookingPrototype.Controllers;
using TMPro;

namespace CookingPrototype.UI
{
	public sealed class LoseWindow : MonoBehaviour
	{
		private readonly bool _isInit = false;

		[SerializeField] private Image _goalBar;
		[SerializeField] private TMP_Text _goalText;
		[SerializeField] private Button _replayButton;
		[SerializeField] private Button _exitButton;
		[SerializeField] private Button _closeButton;

		public void Show()
		{
			if (_isInit == false)
				Init();

			GameplayController gameplayController = GameplayController.Instance;

			_goalBar.fillAmount = Mathf.Clamp01((float) gameplayController.TotalOrdersServed / gameplayController.OrdersTarget);
			_goalText.text = $"{gameplayController.TotalOrdersServed}/{gameplayController.OrdersTarget}";

			gameObject.SetActive(true);
		}

		public void Hide() =>
			gameObject.SetActive(false);

		private void Init()
		{
			GameplayController gameplayController = GameplayController.Instance;

			_replayButton.onClick.AddListener(gameplayController.Restart);
			_exitButton.onClick.AddListener(gameplayController.CloseGame);
			_closeButton.onClick.AddListener(gameplayController.CloseGame);
		}
	}
}