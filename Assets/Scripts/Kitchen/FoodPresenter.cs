using System;
using UnityEngine;

namespace CookingPrototype.Kitchen
{
	public sealed class FoodPresenter : MonoBehaviour
	{
		[Serializable]
		public class FoodVisualizersSet
		{
			[SerializeField] private GameObject _empty;
			[SerializeField] private FoodVisualizer _rawVisualizer;
			[SerializeField] private FoodVisualizer _cookedVisualizer;
			[SerializeField] private FoodVisualizer _overcookedVisualizer;

			public void Hide()
			{
				if (_empty != null)
					_empty.SetActive(false);

				_rawVisualizer?.SetEnabled(false);
				_cookedVisualizer?.SetEnabled(false);
				_overcookedVisualizer?.SetEnabled(false);
			}

			public void ShowEmpty()
			{
				Hide();

				if ( _empty != null )
					_empty.SetActive(true);
			}

			public void ShowStatus(Food.FoodStatus status)
			{
				Hide();

				switch (status)
				{
					case Food.FoodStatus.Raw:
					{
						_rawVisualizer?.SetEnabled(true);
						return;
					}
					case Food.FoodStatus.Cooked:
					{
						_cookedVisualizer?.SetEnabled(true);
						return;
					}
					case Food.FoodStatus.Overcooked:
					{
						_overcookedVisualizer?.SetEnabled(true);
						return;
					}
				}
			}
		}

		[SerializeField] private string _foodName = string.Empty;
		[SerializeField] private FoodVisualizersSet _set;
		[SerializeField] private FoodPlace _place;

		private void Start()
		{
			_set?.Hide();

			if (_place)
				_place.FoodPlaceUpdated += OnFoodPlaceUpdated;
		}

		private void OnDestroy()
		{
			if (_place)
				_place.FoodPlaceUpdated -= OnFoodPlaceUpdated;
		}

		private void OnFoodPlaceUpdated()
		{
			if (_place.IsFree)
			{
				_set?.ShowEmpty();
			}
			else
			{
				if (_place.CurrentFood.Name == _foodName)
					_set?.ShowStatus(_place.CurrentFood.CurrentStatus);
				else
					_set?.Hide();
			}
		}
	}
}