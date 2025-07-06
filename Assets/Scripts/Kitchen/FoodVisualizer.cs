using System.Collections.Generic;
using UnityEngine;

namespace CookingPrototype.Kitchen
{
	public sealed class FoodVisualizer : MonoBehaviour
	{
		[SerializeField] private string _name;
		[SerializeField] private List<GameObject> _auxObjects = new List<GameObject>();

		public string Name => _name;

		public void SetEnabled(bool enabled)
		{
			gameObject.SetActive(enabled);
			_auxObjects.ForEach(auxObject => auxObject.SetActive(enabled));
		}
	}
}