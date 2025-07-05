using UnityEngine;

namespace CookingPrototype.Kitchen
{
	public sealed class CustomerOrderPlace : MonoBehaviour
	{
		public Order CurOrder { get; private set; } = null;

		public bool IsActive => CurOrder != null;

		public void Init(Order order)
		{
			CurOrder = order;
			gameObject.SetActive(true);
		}

		public void Complete()
		{
			CurOrder = null;
			gameObject.SetActive(false);
		}
	}
}
