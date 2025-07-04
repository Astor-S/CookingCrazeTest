using System.Collections.Generic;
using UnityEngine;
using JetBrains.Annotations;

namespace CookingPrototype.Kitchen
{
	[RequireComponent(typeof(FoodPlace))]
	public sealed class FoodTransfer : MonoBehaviour
	{
		[SerializeField] private bool _onlyTransferCooked = true;
		[SerializeField] private List<AbstractFoodPlace> _destPlaces = new List<AbstractFoodPlace>();

		private FoodPlace _place;

		private void Start()
		{
			_place = GetComponent<FoodPlace>();
		}

		[UsedImplicitly]
		public void TryTransferFood()
		{
			Food food = _place.CurrentFood;

			if ( food == null )
				return;

			if (_onlyTransferCooked && (food.CurrentStatus != Food.FoodStatus.Cooked))
			{
				_place.TryPlaceFood(food);

				return;
			}
			foreach (AbstractFoodPlace place in _destPlaces)
			{
				if (place.TryPlaceFood(food) == false )
					continue;

				_place.FreePlace();

				return;
			}
		}
	}
}