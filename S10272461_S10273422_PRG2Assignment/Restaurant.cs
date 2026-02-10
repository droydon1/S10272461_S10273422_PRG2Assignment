using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace S10272461_S10273422_PRG2Assignment
{
    internal class Restaurant
    {
        public string RestaurantId { get; set; }
        public string RestaurantName { get; set; }
        public string RestaurantEmail { get; set; }
        private List<Menu> Menus { get; set; } = new List<Menu>();
        private List<SpecialOffer> Offers { get; set; } = new List<SpecialOffer>();
        private Queue<Order> Orders { get; set; } = new Queue<Order>();
        // constructor
        public Restaurant(string id, string name, string email)
        {
            RestaurantId = id;
            RestaurantName = name;
            RestaurantEmail = email;
        }
        // methods

        public void AddMenu(Menu menu)
        {
            Menus.Add(menu);
        }

        public bool RemoveMenu(Menu menu)
        {
            return Menus.Remove(menu);
        }

        public void DisplayMenu()
        {
            if (Menus.Count == 0)
            {
                Console.WriteLine("No menus available.");
                return;
            }
            foreach (var menu in Menus)
            {
                menu.DisplayFoodItems();
            }
        }


        public void DisplaySpecialOffers()
        {
            if (Offers.Count == 0)
            {
                Console.WriteLine("No special offers.");
                return;
            }

            foreach (var offer in Offers)
            {
                Console.WriteLine(offer.ToString());
            }
        }


        public void DisplayOrders()
        {
            if (Orders.Count == 0)
            {
                Console.WriteLine("No orders received.");
                return;
            }

            foreach (var order in Orders)
            {
                Console.WriteLine(order.ToString());
            }
        }

        public override string ToString()
        {
            return $"{RestaurantName} ({RestaurantId}) - {Menus.Count} menus, {Offers.Count} offers, {Orders.Count} orders";
        }
    }
}
