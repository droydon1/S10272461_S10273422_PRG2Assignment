using S10272461_S10273422_PRG2Assignment;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


//==========================================================
// Student Number : S10272461B
// Student Name : Droydon Goh
// Partner Name : Fan Ming
//==========================================================
internal class Menu
    {
        private string menuId;

        public string MenuId
        {
            get { return menuId; }
            set { menuId = value; }
        }

        private string menuName;

        public string MenuName
        {
            get { return menuName; }
            set { menuName = value; }
        }

        List<FoodItem> foodItems = new List<FoodItem>();

        public Menu(string menuId, string menuName)
        {
            this.menuId = menuId;
            this.menuName = menuName;
        }
        
        public void AddFoodItem(FoodItem item)
        {
            foodItems.Add(item);
        }
        public bool RemoveFoodItem(FoodItem item)
        {
            return foodItems.Remove(item);
        }
        public void DisplayFoodItems()
        {
            Console.WriteLine($"Menu: {MenuName}");
            if (foodItems.Count == 0)
            {
                Console.WriteLine("  No food items.");
                return;
            }
            foreach (var item in foodItems)
            {
                Console.WriteLine("  - " + item.ToString());
            }
        }
        public override string ToString()
        {
            return $"Menu: {menuName} (ID: {menuId}) - Total Items: {foodItems.Count}";

        }

        public List<FoodItem> GetFoodItems()
        {
            return foodItems;
        }
    }

