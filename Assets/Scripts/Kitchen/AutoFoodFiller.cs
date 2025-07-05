using System.Collections.Generic;
using UnityEngine;

namespace CookingPrototype.Kitchen
{
	public sealed class AutoFoodFiller : MonoBehaviour
	{
		[SerializeField] private string _foodName;
		[SerializeField] private List<AbstractFoodPlace> _places = new List<AbstractFoodPlace>();

		private void Update()
		{
			foreach (AbstractFoodPlace place in _places)
				place.TryPlaceFood(new Food(_foodName));
		}
	}
}