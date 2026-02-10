using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace S10272461_S10273422_PRG2Assignment
{
    internal class Customer
    {
        public string EmailAddress { get; set; }
        public string CustomerName { get; set; }
        private List<Order> Orders { get; set; } = new List<Order>();

        public Customer(string email, string name)
        {
            EmailAddress = email;
            CustomerName = name;
        }

        public void AddOrder(Order order)
        {
            Orders.Add(order);
        }
        public bool RemoveOrder(Order order)
        {
            return Orders.Remove(order);
        }

        public void DisplayAllOrders()
        {
            if (Orders.Count == 0)
            {
                Console.WriteLine("No orders found.");
                return;
            }

            foreach (var order in Orders)
            {
                Console.WriteLine(order.ToString());
            }
        }
        public List<Order> GetOrders()
        {
            return Orders;
        }

        public override string ToString()
        {
            return $"{CustomerName} ({EmailAddress}) - {Orders.Count} orders";
        }
    }
}
