using UnityEngine;
using UnityEngine.UI;
using CookingPrototype.Controllers;
using TMPro;

namespace CookingPrototype.UI
{
	public sealed class TopUI : MonoBehaviour
	{
		[SerializeField] private Image _ordersBar;
		[SerializeField] private TMP_Text _ordersCountText;
		[SerializeField] private TMP_Text _customersCountText;

		private void Start()
		{
			GameplayController.Instance.TotalOrdersServedChanged += OnOrdersChanged;
			CustomersController.Instance.TotalCustomersGeneratedChanged += OnCustomersChanged;
			OnOrdersChanged();
			OnCustomersChanged();
		}

		private void OnDestroy()
		{
			if (GameplayController.Instance)
				GameplayController.Instance.TotalOrdersServedChanged -= OnOrdersChanged;

			if (CustomersController.Instance)
				CustomersController.Instance.TotalCustomersGeneratedChanged -= OnCustomersChanged;
		}

		private void OnCustomersChanged()
		{
			CustomersController customerController = CustomersController.Instance;
			_customersCountText.text = (customerController.CustomersTargetNumber - customerController.TotalCustomersGenerated).ToString();
		}

		private void OnOrdersChanged()
		{
			GameplayController gameplayController = GameplayController.Instance;
			_ordersCountText.text = $"{gameplayController.TotalOrdersServed}/{gameplayController.OrdersTarget}";
			_ordersBar.fillAmount = (float) gameplayController.TotalOrdersServed / gameplayController.OrdersTarget;
		}
	}
}