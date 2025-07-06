using UnityEngine;
using JetBrains.Annotations;

namespace CookingPrototype.Kitchen
{
	[RequireComponent(typeof(FoodPlace))]
	public sealed class FoodTrasher : MonoBehaviour
	{
		[SerializeField] private float _doubleTapMaxDelay = 0.3f;

		private FoodPlace _place;
		private float _lastTapTime = 0f;

		private void Start()
		{
			_place = GetComponent<FoodPlace>();
		}

		[UsedImplicitly]
		public void TryTrashFood()
		{
			if (_place.CurrentFood == null)
				return;

			if (_place.CurrentFood.CurrentStatus != Food.FoodStatus.Overcooked)
				return; 

			if (Time.time - _lastTapTime < _doubleTapMaxDelay)
			{
				TrashFood();
				_lastTapTime = 0f;
			}
			else
			{
				_lastTapTime = Time.time;
			}
		}

		private void TrashFood()
		{
			Food foodToTrash = _place.ExtractFood();

			if ( foodToTrash != null && foodToTrash.GameObject != null )
				Destroy(foodToTrash.GameObject);
		}
	}
}