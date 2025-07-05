using UnityEngine;
using UnityEngine.UI;
using CookingPrototype.Controllers;
using TMPro;

namespace CookingPrototype.UI
{
	public sealed class WinWindow : MonoBehaviour
	{
		private readonly bool _isInit = false;

		[SerializeField] private Image _goalBar;
		[SerializeField] private TMP_Text _goalText;
		[SerializeField] private Button _okButton;
		[SerializeField] private Button _closeButton;

		public void Show()
		{
			if (_isInit == false)
				Init();

			GameplayController gameplayContoller = GameplayController.Instance;

			_goalText.text = $"{gameplayContoller.TotalOrdersServed}/{gameplayContoller.OrdersTarget}";
			_goalBar.fillAmount = Mathf.Clamp01((float) gameplayContoller.TotalOrdersServed / gameplayContoller.OrdersTarget);

			gameObject.SetActive(true);
		}

		public void Hide() =>
			gameObject.SetActive(false);

		private void Init()
		{
			GameplayController gameplayContoller = GameplayController.Instance;

			_okButton.onClick.AddListener(gameplayContoller.CloseGame);
			_closeButton.onClick.AddListener(gameplayContoller.CloseGame);
		}
	}
}