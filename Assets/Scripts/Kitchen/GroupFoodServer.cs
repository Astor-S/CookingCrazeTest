using System.Collections.Generic;
using UnityEngine;
using JetBrains.Annotations;

namespace CookingPrototype.Kitchen
{
	public sealed class GroupFoodServer : MonoBehaviour
	{
		[SerializeField] private List<FoodServer> _servers;

		[UsedImplicitly]
		public void TryServe()
		{
			foreach (FoodServer server in _servers)
				if (server.TryServeFood())
					return;
		}
	}
}