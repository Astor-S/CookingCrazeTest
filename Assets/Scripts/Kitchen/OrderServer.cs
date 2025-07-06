using UnityEngine;
using CookingPrototype.Controllers;
using JetBrains.Annotations;

namespace CookingPrototype.Kitchen
{
	[RequireComponent(typeof(OrderPlace))]
	public sealed class OrderServer : MonoBehaviour
	{
		private OrderPlace _orderPlace;

		private void Start()
		{
			_orderPlace = GetComponent<OrderPlace>();
		}

		[UsedImplicitly]
		public void TryServeOrder()
		{
			Order order = OrdersController.Instance.FindOrder(_orderPlace.CurOrder);

			if ((order == null) || GameplayController.Instance.TryServeOrder(order) == false)
				return;

			_orderPlace.FreePlace();
		}
	}
}