using System.Collections.Generic;
using UnityEngine;

namespace CookingPrototype.Kitchen
{
	public sealed class OrderVisualizer : MonoBehaviour
	{
		[SerializeField] private List<FoodVisualizer> _visualizers = new List<FoodVisualizer>();

		private void Start()
		{
			Clear();
		}

		public void Init(List<string> foods)
		{
			Clear();

			foreach (FoodVisualizer visualizer in _visualizers)
				if (foods.Contains(visualizer.Name))
					visualizer.SetEnabled(true);
		}

		private void Clear() =>
			_visualizers.ForEach(visualizer => visualizer.SetEnabled(false));
	}
}