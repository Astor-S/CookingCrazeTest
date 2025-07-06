using UnityEngine;

namespace CookingPrototype.Kitchen
{
	[RequireComponent(typeof(OrderPlace))]
	public sealed class OrderPresenter : MonoBehaviour
	{
		[SerializeField] private OrderVisualizer _visualizer;

		private OrderPlace _orderPlace;

		private void Start()
		{
			_orderPlace = GetComponent<OrderPlace>();
			_orderPlace.CurOrderUpdated += OnOrderUpdated;
		}

		private void OnDestroy()
		{
			if (_orderPlace)
				_orderPlace.CurOrderUpdated -= OnOrderUpdated;
		}

		private void OnOrderUpdated() =>
			_visualizer.Init(_orderPlace.CurOrder);
	}
}