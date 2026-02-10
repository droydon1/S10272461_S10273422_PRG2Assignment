using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace S10272461_S10273422_PRG2Assignment
{
	public class Customer
	{
		private string emailAddress;

		public string EmailAddress
		{
			get { return emailAddress; }
			set { emailAddress = value; }
		}
		private string customerName;

		public string CustomerName
		{
			get { return customerName; }
			set { customerName = value; }
		}


		private Order[] orders;

		public Order[] Orders
		{
			get { return orders; }
			set { orders = value; }
		}
		private int orderCount;

		public int OrderCount
		{
			get { return orderCount; }
			set { orderCount = value; }
		}



		public Customer(string emailAddress, string customerName)
		{
			EmailAddress = emailAddress;
			CustomerName = customerName;
			orders = new Order[100];
			orderCount = 0;
		}

		public void AddOrder(Order order)
		{
			if (orderCount < orders.Length)
			{
				orders[orderCount] = order;
				orderCount++;
				Console.WriteLine("Order added successfully!");
			}
			else
			{
				Console.WriteLine("Cannot add more orders!");
			}
		}
		public bool RemoveOrder(Order order)
		{
			for (int i = 0; i < orderCount; i++)
			{
				if (orders[i] == order)
				{
					for (int j = i; j < orderCount - 1; j++)
					{
						orders[j] = orders[j + 1];
					}
					orders[orderCount - 1] = null;
					orderCount--;
					Console.WriteLine("Order removed successfully!");
					return true;
				}
			}
			Console.WriteLine("Order not found!");
			return false;
		}
		public void DisplayAllOrders()
		{
			Console.WriteLine("Orders for " + CustomerName);
			if (orderCount == 0)
			{
				Console.WriteLine("No orders found.");
				return;
			}
			for (int i = 0; i < orderCount; i++)
			{
				Console.WriteLine(orders[i]);
			}
		}
		public Order[] GetOrders()
		{
			Order[] result = new Order[orderCount];
			for (int i = 0; i < orderCount; i++)
			{
				result[i] = orders[i];
			}
			return result;
		}

		public override string ToString()
		{
			return $"Customer {{ EmailAddress='{EmailAddress}', CustomerName='{CustomerName}', OrderCount={orderCount} }}";
		}

		public Order[] GetPendingOrders()
		{
			int count = 0;

			for (int i = 0; i < orderCount; i++)
			{
				if (orders[i].OrderStatus == "Pending")
					count++;
			}

			Order[] result = new Order[count];
			int index = 0;

			for (int i = 0; i < orderCount; i++)
			{
				if (orders[i].OrderStatus == "Pending")
				{
					result[index] = orders[i];
					index++;
				}
			}

			return result;
		}

		public Order GetOrderById(int id)
		{
			for (int i = 0; i < orderCount; i++)
			{
				if (orders[i].OrderId == id)
					return orders[i];
			}

			return null;
		}

	}
}
