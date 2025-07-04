using System;
using UnityEngine;
using UnityEngine.UI;

namespace CookingPrototype.Kitchen
{
	public sealed class CookingTimer : MonoBehaviour
	{
		[Serializable]
		public class TimerSpriteSet
		{
			[SerializeField] private Sprite _background;
			[SerializeField] private Sprite _foreground;

			public Sprite Background => _background;
			public Sprite Foreground => _foreground;
		}

		[SerializeField] private FoodPlace _place;
		[SerializeField] private Image _background;
		[SerializeField] private Image _foreground;

		[SerializeField] private TimerSpriteSet _normal;
		[SerializeField] private TimerSpriteSet _overcook;

		private TimerSpriteSet CurrentSet
		{
			set
			{
				if (value == null)
				{
					return;
				}
				if (_background)
				{
					_background.sprite = value.Background;
					_background.SetNativeSize();
				}
				if (_foreground)
				{
					_foreground.sprite = value.Foreground;
					_foreground.SetNativeSize();
				}
			}
		}

		private void Awake()
		{
			if (_place)
				_place.FoodPlaceUpdated += OnFoodPlaceUpdated;
		}

		private void Start()
		{
			OnFoodPlaceUpdated();
		}

		private void Update()
		{
			if (_place == null)
				return;

			if ( _place.IsCooking )
				_foreground.fillAmount = _place.TimerNormalized;
		}

		private void OnDestroy()
		{
			if (_place)
				_place.FoodPlaceUpdated -= OnFoodPlaceUpdated;
		}

		private void OnFoodPlaceUpdated()
		{
			if (_place.IsCooking)
			{
				gameObject.SetActive(true);
				CurrentSet = _place.CurrentFood.CurrentStatus == Food.FoodStatus.Raw ? _normal : _overcook;
			}
			else
			{
				gameObject.SetActive(false);
			}
		}
	}
}