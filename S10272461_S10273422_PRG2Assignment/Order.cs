using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;

namespace S10272461_S10273422_PRG2Assignment
{
    internal class Order

    {

        public int OrderId { get; set; }

        public DateTime OrderDateTime { get; set; }

        public double OrderTotal { get; set; }

        public string OrderStatus { get; set; }

        public DateTime DeliveryDateTime { get; set; }

        public string DeliveryAddress { get; set; }

        public string OrderPaymentMethod { get; set; }

        public bool OrderPaid { get; set; }

        public SpecialOffer Offer { get; set; }

        private List<OrderedFoodItem> OrderedItems { get; set; } = new List<OrderedFoodItem>();

        public Customer Customer { get; set; }

        public Restaurant Restaurant { get; set; }

        public Order(int orderId, DateTime orderDateTime, string status, string deliveryAddress, string paymentMethod)

        {

            OrderId = orderId;

            OrderDateTime = orderDateTime;

            OrderStatus = status;

            DeliveryAddress = deliveryAddress;

            OrderPaymentMethod = paymentMethod;

            OrderPaid = false;

        }

        // methods

        public void AddOrderedFoodItem(OrderedFoodItem item)

        {

            OrderedItems.Add(item);

            CalculateOrderTotal();

        }

        public bool RemoveOrderedFoodItem(OrderedFoodItem item)

        {

            bool removed = OrderedItems.Remove(item);

            if (removed) CalculateOrderTotal();

            return removed;

        }

        public void DisplayOrderedFoodItems()

        {

            if (OrderedItems.Count == 0)

            {

                Console.WriteLine("No ordered items.");

                return;

            }

            foreach (var item in OrderedItems)

            {

                Console.WriteLine(item.ToString());

            }

        }

        public double CalculateOrderTotal()

        {

            double total = 0;

            foreach (var item in OrderedItems)

            {

                total += item.CalculateSubTotal();

            }

            if (Offer != null)

            {

                total *= (1 - Offer.Discount);

            }

            OrderTotal = total;

            return total;

        }

        public override string ToString()

        {

            return $"Order {OrderId} | Status: {OrderStatus} | Total: ${OrderTotal:F2} | Delivery: {DeliveryDateTime} | Items: {OrderedItems.Count}";

        }

        public List<OrderedFoodItem> GetOrderedItems()

        {

            return OrderedItems;

        }

    }

}
 
}

