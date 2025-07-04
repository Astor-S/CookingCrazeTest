using System.Collections.Generic;
using UnityEngine;
using CookingPrototype.Controllers;

namespace CookingPrototype.Kitchen
{
	[RequireComponent(typeof(FoodPlace))]
	public sealed class FoodServer : MonoBehaviour
	{
		private const int OrderNameListCapacity = 1;

		private FoodPlace _place;

		private void Start()
		{
			_place = GetComponent<FoodPlace>();
		}

		public bool TryServeFood()
		{
			if (_place.IsFree || (_place.CurrentFood.CurrentStatus != Food.FoodStatus.Cooked))
				return false;

			Order order = OrdersController.Instance.FindOrder(new List<string>(OrderNameListCapacity){_place.CurrentFood.Name});

			if ((order == null) || GameplayController.Instance.TryServeOrder(order) == false)
				return false;

			_place.FreePlace();
			return true;
		}
	}
}