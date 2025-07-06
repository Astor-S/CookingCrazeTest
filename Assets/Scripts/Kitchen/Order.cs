using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace CookingPrototype.Kitchen
{
	public sealed class Order
	{
		public class OrderFood
		{
			public string Name  { get; } = null;
			public string Needs { get; } = null;

			public OrderFood(string name, string needs)
			{
				Name  = name;
				Needs = needs;
			}
		}

		private readonly string _name;

		private readonly List<OrderFood> _foods;

		public ReadOnlyCollection<OrderFood> Foods => _foods.AsReadOnly();
		public string Name => _name;

		public Order(string name, List<OrderFood> foods)
		{
			_name   = name;
			_foods = foods;
		}
	}
}