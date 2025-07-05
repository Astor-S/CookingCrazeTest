using System;
using UnityEngine;
using CookingPrototype.Kitchen;
using CookingPrototype.UI;
using JetBrains.Annotations;

namespace CookingPrototype.Controllers
{
	public sealed class GameplayController : MonoBehaviour
	{
		[SerializeField] private GameObject _tapBlock;
		[SerializeField] private WinWindow  _winWindow;
		[SerializeField] private LoseWindow _loseWindow;
		[SerializeField] private StartWindow _startWindow;
		[SerializeField] private PauseHandler _pauseHandler;

		private int _ordersTarget = 0;
		private bool _gameStarted = false;

		public event Action TotalOrdersServedChanged;

		public static GameplayController Instance {get; private set;}

		public int TotalOrdersServed {get; private set;} = 0;

		public int OrdersTarget
		{
			get {return _ordersTarget;}
			set
			{
				_ordersTarget = value;
				TotalOrdersServedChanged?.Invoke();
			}
		}

		private void Awake()
		{
			if (Instance != null)
				Debug.LogError("Another instance of GameplayController already exists");

			Instance = this;
		}

		private void Start()
		{
			Init();
			OnGameStart();
		}

		private void OnDestroy()
		{
			if (Instance == this)
				Instance = null;
		}

		public void CheckGameFinish()
		{
			if (CustomersController.Instance.IsComplete)
				EndGame(TotalOrdersServed >= OrdersTarget);
		}

		[UsedImplicitly]
		public bool TryServeOrder(Order order)
		{
			if (CustomersController.Instance.ServeOrder(order) == false)
				return false;

			TotalOrdersServed++;
			TotalOrdersServedChanged?.Invoke();
			CheckGameFinish();

			return true;
		}

		public void Restart()
		{
			Init();
			CustomersController.Instance.Init();
			HideWindows();
			_startWindow.Show();

			foreach (AbstractFoodPlace place in FindObjectsByType<AbstractFoodPlace>(FindObjectsSortMode.None))
				place.FreePlace();
		}

		public void CloseGame()
		{
#if UNITY_EDITOR
			UnityEditor.EditorApplication.isPlaying = false;
#else
			Application.Quit();
#endif
		}

		private void Init()
		{
			TotalOrdersServed = 0;
			_pauseHandler.PauseGame();
			TotalOrdersServedChanged?.Invoke();
			_gameStarted = false;
		}

		private void OnGameStart()
		{
			if (_gameStarted == false)
			{
				_startWindow.Show();
				_gameStarted = true;
			}
		}

		private void EndGame(bool win)
		{
			_pauseHandler.PauseGame();
			_tapBlock?.SetActive(true);

			if (win)
				_winWindow.Show();
			else
				_loseWindow.Show();
		}

		private void HideWindows()
		{
			_tapBlock?.SetActive(false);
			_winWindow?.Hide();
			_loseWindow?.Hide();
		}
	}
}