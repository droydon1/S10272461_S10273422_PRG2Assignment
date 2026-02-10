using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;

namespace S10272461_S10273422_PRG2Assignment
{
    public class Order
    {

        private int orderId;

        public int OrderId
        {
            get { return orderId; }
            set { orderId = value; }
        }
        private DateTime orderDateTime;

        public DateTime OrderDateTime
        {
            get { return orderDateTime; }
            set { orderDateTime = value; }
        }

        private double orderTotal;

        public double OrderTotal
        {
            get { return orderTotal; }
            set { orderTotal = value; }
        }

        private string orderStatus;

        public string OrderStatus
        {
            get { return orderStatus; }
            set { orderStatus = value; }
        }

        private DateTime deliveryDateTime;

        public DateTime DeliveryDateTime
        {
            get { return deliveryDateTime; }
            set { deliveryDateTime = value; }
        }

        private string deliveryAddress;

        public string DeliveryAddress
        {
            get { return deliveryAddress; }
            set { deliveryAddress = value; }
        }

        private string orderPaymentMethod;

        public string OrderPaymentMethod
        {
            get { return orderPaymentMethod; }
            set { orderPaymentMethod = value; }
        }
        private bool orderPaid;

        public bool OrderPaid
        {
            get { return orderPaid; }
            set { orderPaid = value; }
        }
        private OrderedFoodItem[] orderedFoodItems;

        public OrderedFoodItem[] OrderedFooditems
        {
            get { return orderedFoodItems; }
            set { orderedFoodItems = value; }
        }
        private SpecialOffer specialOffer;

        public SpecialOffer SpecialOffer
        {
            get { return specialOffer; }
            set { specialOffer = value; }
        }
        private int itemCount;

        public int ItemCount
        {
            get { return itemCount; }
            set { itemCount = value; }
        }


        //Constructor
        public Order(int orderId, DateTime orderDateTime, string orderStatus, DateTime deliveryDateTime, string deliveryAddress, string orderPaymentMethod)
        {
            OrderId = orderId;
            OrderDateTime = orderDateTime;
            OrderStatus = orderStatus;

            DeliveryDateTime = deliveryDateTime;
            DeliveryAddress = deliveryAddress;

            OrderPaymentMethod = orderPaymentMethod;

            OrderTotal = 0.0;
            OrderPaid = false;

            orderedFoodItems = new OrderedFoodItem[100];
            itemCount = 0;
        }

        //Methods
        public void AddOrderedFoodItem(OrderedFoodItem orderedFoodItem)
        {
            if (itemCount < orderedFoodItems.Length)
            {
                orderedFoodItems[itemCount] = orderedFoodItem;
                itemCount++;
                CalculateOrderTotal();
                Console.WriteLine("Food item added to order!");
            }
            else
            {
                Console.WriteLine("Cannot add more items to order!");
            }
        }
        public bool RemoveOrderedFoodItem(OrderedFoodItem orderedFoodItem)
        {
            for (int i = 0; i < itemCount; i++)
            {
                if (orderedFoodItems[i] == orderedFoodItem)
                {
                    for (int j = i; j < itemCount - 1; j++)
                    {
                        orderedFoodItems[j] = orderedFoodItems[j + 1];
                    }
                    orderedFoodItems[itemCount - 1] = null;
                    itemCount--;
                    CalculateOrderTotal();
                    Console.WriteLine("Food item removed from order!");
                    return true;
                }
            }
            Console.WriteLine("Food item not found in order!");
            return false;
        }

        public double CalculateOrderTotal()
        {
            OrderTotal = 0.0;
            for (int i = 0; i < itemCount; i++)
            {
                OrderTotal += orderedFoodItems[i].SubTotal;
            }
            return OrderTotal;
        }

        public void DisplayOrderedFoodItems()
        {
            Console.WriteLine("Items for Order " + OrderId);
            if (itemCount == 0)
            {
                Console.WriteLine("No items in this order.");
                return;
            }
            for (int i = 0; i < itemCount; i++)
            {
                Console.WriteLine((i + 1) + ". " + orderedFoodItems[i]);
            }
        }


        public OrderedFoodItem[] GetOrderedFoodItems()
        {
            OrderedFoodItem[] result = new OrderedFoodItem[itemCount];
            for (int i = 0; i < itemCount; i++)
            {
                result[i] = orderedFoodItems[i];
            }
            return result;
        }
        public override string ToString()
        {
            return $"Order {{ OrderId={OrderId}, OrderDateTime={OrderDateTime}, OrderTotal={OrderTotal}, OrderStatus='{OrderStatus}', DeliveryAddress='{DeliveryAddress}', OrderPaid={OrderPaid} }}";
        }
        public void UpdateDeliveryAddress(string newAddress)
        {
            DeliveryAddress = newAddress;
        }
        public void UpdateDeliveryDateTime(DateTime newDateTime)
        {
            DeliveryDateTime = newDateTime;
        }
    }
}

