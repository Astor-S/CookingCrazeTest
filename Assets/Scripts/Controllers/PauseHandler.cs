using UnityEngine;

namespace CookingPrototype.Controllers
{
	public class PauseHandler : MonoBehaviour
	{
		public void PauseGame() =>
			Time.timeScale = 0f;

		public void ContinueGame() =>
			Time.timeScale = 1f;
	}
}