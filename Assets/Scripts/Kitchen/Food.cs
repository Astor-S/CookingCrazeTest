using UnityEngine;

namespace CookingPrototype.Kitchen
{
	public sealed class Food
	{
		public enum FoodStatus
		{
			Raw,
			Cooked,
			Overcooked,
		}

		public FoodStatus CurrentStatus { get; private set; }
		public GameObject GameObject { get; private set; }
		public string Name { get; }

		public Food(string name, FoodStatus status = FoodStatus.Raw)
		{
			Name = name;
			CurrentStatus = status;
		}

		public void CookStep()
		{
			switch (CurrentStatus)
			{
				case FoodStatus.Raw:
				{
					CurrentStatus = FoodStatus.Cooked;
					return;
				}
				case FoodStatus.Cooked:
				{
					CurrentStatus = FoodStatus.Overcooked;
					return;
				}
				default:
				{
					return;
				}
			}
		}
	}
}