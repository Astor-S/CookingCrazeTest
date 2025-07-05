using System.Collections.Generic;
using UnityEngine;
using JetBrains.Annotations;

namespace CookingPrototype.Kitchen
{
	public sealed class FoodPlacer : MonoBehaviour
	{
		[SerializeField] private string _foodName = string.Empty;
		[SerializeField] private List<AbstractFoodPlace> _places = new List<AbstractFoodPlace>();

		[UsedImplicitly]
		public void TryPlaceFood()
		{
			foreach (AbstractFoodPlace place in _places)
				if (place.TryPlaceFood(new Food(_foodName)))
					return;
		}
	}
}