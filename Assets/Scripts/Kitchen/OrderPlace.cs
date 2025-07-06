using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using CookingPrototype.Controllers;

namespace CookingPrototype.Kitchen
{
	public sealed class OrderPlace : AbstractFoodPlace
	{
		private readonly List<string> _curOrder = new List<string>();
		private readonly List<Order> _possibleOrders = new List<Order>();

		[SerializeField] private List<FoodPlace> _places = new List<FoodPlace>();

		public event Action CurOrderUpdated;

		public List<string> CurOrder => _curOrder;

		private void Start()
		{
			_possibleOrders.AddRange(OrdersController.Instance.Orders);
		}

		public override bool TryPlaceFood(Food food)
		{
			if (CanAddFood(food) == false)
				return false;

			foreach (FoodPlace place in _places)
			{
				if (place.TryPlaceFood(food) == false)
					continue;

				_curOrder.Add(food.Name);
				UpdatePossibleOrders();
				CurOrderUpdated?.Invoke();

				return true;
			}

			return false;
		}

		public override void FreePlace()
		{
			_possibleOrders.Clear();
			_possibleOrders.AddRange(OrdersController.Instance.Orders);

			_curOrder.Clear();

			foreach (FoodPlace place in _places)
				place.FreePlace();

			CurOrderUpdated?.Invoke();
		}

		private bool CanAddFood(Food food)
		{
			if (_curOrder.Contains(food.Name))
				return false;

			foreach(Order order in _possibleOrders)
				foreach (Order.OrderFood orderFood in order.Foods.Where(foodItem => foodItem.Name == food.Name))
					if (string.IsNullOrEmpty(orderFood.Needs) || _curOrder.Contains(orderFood.Needs))
						return true;

			return false;
		}

		private void UpdatePossibleOrders()
		{
			List<Order> ordersToRemove = new List<Order>();

			int shiftIndex = 1;
			int lastFoodIndex = _curOrder.Count - shiftIndex;

			foreach (Order order in _possibleOrders)
				if (order.Foods.Where(foodItem => foodItem.Name == _curOrder[lastFoodIndex]).Count() == 0)
					ordersToRemove.Add(order);

			_possibleOrders.RemoveAll(possibleOrder => ordersToRemove.Contains(possibleOrder));
		}
	}
}