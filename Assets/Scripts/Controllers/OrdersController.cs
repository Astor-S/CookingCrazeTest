using System.Collections.Generic;
using System.Linq;
using System.IO;
using System.Xml;
using UnityEngine;
using CookingPrototype.Kitchen;

namespace CookingPrototype.Controllers
{
	public sealed class OrdersController : MonoBehaviour
	{
		private static OrdersController _instance;
		private readonly List<Order> _orders = new List<Order>();

		private bool _isInit = false;

		public static OrdersController Instance
		{
			get
			{
				if (_instance == false)
					_instance = FindAnyObjectByType<OrdersController>();
				if (_instance && _instance._isInit == false)
					_instance.Init();

				return _instance;
			}
			set => _instance = value;
		}

		public List<Order> Orders => _orders;

		private void Awake()
		{
			if ((_instance != null) && (_instance != this))
				Debug.LogError("Another instance of OrdersController already exists!");

			Instance = this;
		}

		private void Start()
		{
			Init();
		}

		private void OnDestroy()
		{
			if (Instance == this)
				Instance = null;
		}

		public Order FindOrder(List<string> foods)
		{
			return _orders.Find(order =>
			{
				if (order.Foods.Count != foods.Count)
					return false;

				foreach (Order.OrderFood orderFood in order.Foods)
					if (order.Foods.Count(foodItem => foodItem.Name == orderFood.Name) != foods.Count(foodName => foodName == orderFood.Name))
						return false;

				return true;
			});
		}

		private void Init()
		{
			if (_isInit)
				return;

			TextAsset ordersConfig = Resources.Load<TextAsset>("Configs/Orders");
			XmlDocument ordersXml = new XmlDocument();
			using (StringReader reader = new StringReader(ordersConfig.ToString()))
				ordersXml.Load(reader);

			XmlElement rootElem = ordersXml.DocumentElement;

			foreach (XmlNode node in rootElem.SelectNodes("order"))
			{
				Order order = ParseOrder(node);
				_orders.Add(order);
			}

			_isInit = true;
		}

		private Order ParseOrder(XmlNode node)
		{
			List<Order.OrderFood> foods = new List<Order.OrderFood>();

			foreach (XmlNode foodNode in node.SelectNodes("food"))
				foods.Add(new Order.OrderFood(foodNode.InnerText, foodNode.SelectSingleNode("@needs")?.InnerText));

			return new Order(node.SelectSingleNode("@name").Value, foods);
		}
	}
}