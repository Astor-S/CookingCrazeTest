using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Random = UnityEngine.Random;
using CookingPrototype.Kitchen;

namespace CookingPrototype.Controllers {
	public class CustomersController : MonoBehaviour {
		private const string CUSTOMER_PREFABS_PATH = "Prefabs/Customer";

		[SerializeField] private List<CustomerPlace> _customerPlaces = null;
		[SerializeField] private int _customersTargetNumber = 15;
		[SerializeField] private float CustomerWaitTime = 18f;
		[SerializeField] private float CustomerSpawnTime = 3f;

		private Stack<List<Order>> _orderSets;
		private float _timer = 0f;

		public static CustomersController Instance { get; private set; }

		[HideInInspector]
		public int TotalCustomersGenerated { get; private set; } = 0;

		public event Action TotalCustomersGeneratedChanged;

		public bool IsComplete =>
			TotalCustomersGenerated >= _customersTargetNumber && _customerPlaces.All(places => places.IsFree);

		private bool HasFreePlaces =>
			_customerPlaces.Any(places => places.IsFree);

		private void Awake()
		{
			if ( Instance != null )
				Debug.LogError("Another instance of CustomersController already exists!");

			Instance = this;
		}

		private void Start() {
			Init();
		}

		private void Update() {
			if ( HasFreePlaces == false ) 
				return;

			_timer += Time.deltaTime;

			if ( (TotalCustomersGenerated >= _customersTargetNumber) || ((_timer > CustomerSpawnTime) == false) )
				return;			

			SpawnCustomer();
			_timer = 0f;
		}

		private void OnDestroy() {
			if ( Instance == this )
				Instance = null;
		}

		public void Init() {

			int totalOrders = 0;
			int orderReduction = 2;
			_orderSets = new Stack<List<Order>>();

			for ( int i = 0; i < _customersTargetNumber; i++ ) {
				List<Order> orders = new List<Order>();

				int minOrdersNumber = 1;
				int maxOrdersNumber = 4;
				int ordersNumber = Random.Range(minOrdersNumber, maxOrdersNumber);

				for ( int j = 0; j < ordersNumber; j++ )
					orders.Add(GenerateRandomOrder());

				_orderSets.Push(orders);
				totalOrders += ordersNumber;
			}

			_customerPlaces.ForEach(places => places.Free());
			_timer = 0f;

			TotalCustomersGenerated = 0;
			TotalCustomersGeneratedChanged?.Invoke();

			GameplayController.Instance.OrdersTarget = totalOrders - orderReduction;
		}

		/// <summary>
		/// Отпускаем указанного посетителя
		/// </summary>
		/// <param name="customer"></param>
		public void FreeCustomer(Customer customer) {
			CustomerPlace place = _customerPlaces.Find(places => places.CurCustomer == customer);

			if ( place == null )
				return;

			place.Free();
			GameplayController.Instance.CheckGameFinish();
		}

		/// <summary>
		///  Пытаемся обслужить посетителя с заданным заказом и наименьшим оставшимся временем ожидания.
		///  Если у посетителя это последний оставшийся заказ из списка, то отпускаем его.
		/// </summary>
		/// <param name="order">Заказ, который пытаемся отдать</param>
		/// <returns>Флаг - результат, удалось ли успешно отдать заказ</returns>
		public bool ServeOrder(Order order) {
			throw new NotImplementedException("ServeOrder: this feature is not implemented.");
		}

		private void SpawnCustomer() {
			List<CustomerPlace> freePlaces = _customerPlaces.FindAll(places => places.IsFree);

			int minCount = 0;

			if ( freePlaces.Count <= minCount )
				return;

			CustomerPlace place = freePlaces[Random.Range(minCount, freePlaces.Count)];
			place.PlaceCustomer(GenerateCustomer());
			TotalCustomersGenerated++;
			TotalCustomersGeneratedChanged?.Invoke();
		}

		private Customer GenerateCustomer() {
			GameObject customerGo = Instantiate(Resources.Load<GameObject>(CUSTOMER_PREFABS_PATH));
			Customer customer = customerGo.GetComponent<Customer>();

			List<Order> orders = _orderSets.Pop();
			customer.Init(orders);

			return customer;
		}

		private Order GenerateRandomOrder() {
			var orderController = OrdersController.Instance;
			int minCount = 0;

			return orderController.Orders[Random.Range(minCount, orderController.Orders.Count)];
		}

	}
}