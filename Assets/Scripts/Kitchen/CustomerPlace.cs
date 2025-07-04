using UnityEngine;

namespace CookingPrototype.Kitchen
{
	public sealed class CustomerPlace : MonoBehaviour
	{
		public Customer CurrentCustomer { get; private set; } = null;

		public bool IsFree => CurrentCustomer == null;

		public void PlaceCustomer(Customer customer)
		{
			CurrentCustomer = customer;
			customer.transform.SetParent(transform);
			customer.transform.localPosition = Vector3.zero;
		}

		public void Free()
		{
			if (CurrentCustomer == false)
				return;

			Customer customer = CurrentCustomer;
			CurrentCustomer = null;
			Destroy(customer.gameObject);
		}
	}
}