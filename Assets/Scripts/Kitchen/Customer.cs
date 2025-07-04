using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using CookingPrototype.Controllers;
using JetBrains.Annotations;

namespace CookingPrototype.Kitchen
{
	public sealed class Customer : MonoBehaviour
	{
		private const string ORDERS_PREFABS_PATH = "Prefabs/Orders/{0}";

		[SerializeField] private Image _customerImage;
		[SerializeField] private List<Sprite> _customerSprites;
		[SerializeField] private Image _timerBar;
		[SerializeField] private List<CustomerOrderPlace> _orderPlaces;
		[SerializeField] private float _timerReductionValue = 6f;

		private List<Order> _orders = null;
		private float _timer = 0f;
		private bool _isActive = false;

		public List<Order> Orders => _orders;
		public float WaitTime => CustomersController.Instance.CustomerWaitTime - _timer;
		public bool IsComplete => _orders.Count == 0;

		private void Update()
		{
			if (_isActive == false)
				return;

			_timer += Time.deltaTime;
			_timerBar.fillAmount = WaitTime / CustomersController.Instance.CustomerWaitTime;

			if (WaitTime <= 0f)
				CustomersController.Instance.FreeCustomer(this);
		}

		public void Init(List<Order> orders)
		{
			_orders = orders;

			if (_orders.Count > _orderPlaces.Count)
			{
				Debug.LogError("There's too many orders for one customer");
				return;
			}

			_orderPlaces.ForEach(places => places.Complete());

			for (int i = 0; i < _orders.Count; i++)
			{
				Order order = _orders[i];
				CustomerOrderPlace place = _orderPlaces[i];
				Instantiate(Resources.Load<GameObject>(string.Format(ORDERS_PREFABS_PATH, order.Name)), place.transform, false);
				place.Init(order);
			}

			SetRandomSprite();

			_isActive = true;
			_timer = 0f;
		}

		[UsedImplicitly]
		public bool ServeOrder(Order order)
		{
			const float minTimerValue = 0f;

			CustomerOrderPlace place = _orderPlaces.Find(places => places.CurOrder == order);

			if (place == false)
				return false;

			_orders.Remove(order);
			place.Complete();
			_timer = Mathf.Max(minTimerValue, _timer - _timerReductionValue);

			return true;
		}

		[ContextMenu("Set random sprite")]
		private void SetRandomSprite()
		{
			int minCount = 0;
			_customerImage.sprite = _customerSprites[Random.Range(minCount, _customerSprites.Count)];
			_customerImage.SetNativeSize();
		}
	}
}