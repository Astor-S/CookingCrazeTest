using System;
using UnityEngine;

namespace CookingPrototype.Kitchen
{
	public class FoodPlace : AbstractFoodPlace
	{
		[SerializeField] private bool _сook = false;
		[SerializeField] private float _cookTime = 0f;
		[SerializeField] private float _overcookTime = 0f;

		private float _timer = 0f;

		public event Action FoodPlaceUpdated;

		public Food CurrentFood { get; private set; } = null;
		public bool IsCooking { get; private set; } = false;

		public bool IsFree => CurrentFood == null;

		public float TimerNormalized
		{
			get
			{
				if (IsFree || _сook == false || IsCooking == false)
					return 0f;
				if (CurrentFood.CurrentStatus == Food.FoodStatus.Raw)
					return _timer / _cookTime;

				return _timer / _overcookTime;
			}
		}

		private void Update()
		{
			if (IsFree || _сook == false || IsCooking == false)
				return;

			_timer += Time.deltaTime;

			switch (CurrentFood.CurrentStatus)
			{
				case Food.FoodStatus.Raw:
				case Food.FoodStatus.Cooked:
					HandleFoodCooking();
					break;
			}
		}

		public override bool TryPlaceFood(Food food)
		{
			if (IsFree == false)
				return false;

			CurrentFood = food;

			if (_сook && CurrentFood.CurrentStatus != Food.FoodStatus.Overcooked)
				IsCooking = true;

			FoodPlaceUpdated?.Invoke();

			return true;
		}

		public Food ExtractFood()
		{
			Food foodToExtract = CurrentFood;
			CurrentFood = null;

			FoodPlaceUpdated?.Invoke();

			return foodToExtract;
		}

		public override void FreePlace()
		{
			CurrentFood = null;
			_timer = 0f;
			IsCooking = false;
			FoodPlaceUpdated?.Invoke();
		}

		private void HandleFoodCooking()
		{
			if ( _timer > (CurrentFood.CurrentStatus == Food.FoodStatus.Raw ? _cookTime : _overcookTime))
			{
				CurrentFood.CookStep();
				_timer = 0f;
				IsCooking = (_overcookTime > 0f && CurrentFood.CurrentStatus == Food.FoodStatus.Cooked);
				FoodPlaceUpdated?.Invoke();
			}
		}
	}
}