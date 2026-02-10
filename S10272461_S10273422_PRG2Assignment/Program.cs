//==========================================================
// Student Number : S10273422H
// Student Name : FanMing
// Partner Name : Droydon Goh
//==========================================================

using S10272461_S10273422_PRG2Assignment;

List<Restaurant> restaurants = new List<Restaurant>();
List<Menu> restaurantMenus = new List<Menu>();

Customer[] customers = new Customer[100];
int customerCount = 0;

// Feature 1: Load Restaurants - Fan Ming
LoadRestaurants();
void LoadRestaurants()
{
    try
    {
        string[] restLines = File.ReadAllLines("restaurants.csv");

        for (int i = 1; i < restLines.Length; i++)
        {
            if (restLines[i].Length == 0)
                continue;

            string[] p = restLines[i].Split(',');

            Restaurant r = new Restaurant(p[0], p[1], p[2]);

            // Create default menu
            Menu m = new Menu("Main", "Main Menu");

            r.AddMenu(m);

            restaurants.Add(r);
            restaurantMenus.Add(m);   // SAVE MENU HERE
        }

        Console.WriteLine("Restaurants loaded.");
    }
    catch (Exception ex)
    {
        Console.WriteLine("Error loading restaurants: " + ex.Message);
    }
}


// Feature 1: Load Food Items - Fan Ming
LoadFoodItems();
void LoadFoodItems()
{
    try
    {
        string[] foodLines = File.ReadAllLines("fooditems.csv");

        for (int i = 1; i < foodLines.Length; i++)
        {
            if (foodLines[i].Length == 0)
                continue;

            string[] p = foodLines[i].Split(',');

            string restId = p[0];

            FoodItem f = new FoodItem(
                p[1],
                p[2],
                double.Parse(p[3]),
                ""
            );

            // Find restaurant
            for (int j = 0; j < restaurants.Count; j++)
            {
                if (restaurants[j].RestaurantId == restId)
                {
                    // Use stored menu
                    restaurantMenus[j].AddFoodItem(f);
                    break;
                }
            }
        }

        Console.WriteLine("Food items loaded.");
    }
    catch (Exception ex)
    {
        Console.WriteLine("Error loading food items: " + ex.Message);
    }
}

// Feature 2: Load Customers - Droydon Goh
LoadCustomers();
void LoadCustomers()
{
    try
    {
        string[] custLines = File.ReadAllLines("customers.csv");

        for (int i = 1; i < custLines.Length; i++)
        {
            if (custLines[i].Length == 0)
                continue;

            string[] p = custLines[i].Split('\t');

            if (p.Length < 2)
                continue;

            Customer c = new Customer(p[1], p[0]);

            customers[customerCount] = c;
            customerCount++;
        }

        Console.WriteLine("Customers loaded.");
    }
    catch (Exception ex)
    {
        Console.WriteLine("Error loading customers: " + ex.Message);
    }
}

// Feature 2: Load Orders - Droydon Goh
LoadOrders();
void LoadOrders()
{
    try
    {
        string[] lines = File.ReadAllLines("orders.csv");

        for (int i = 1; i < lines.Length; i++)
        {
            if (lines[i].Length == 0)
                continue;

            string line = lines[i];
            string items = "";

            int q = line.IndexOf('"');

            if (q != -1)
            {
                items = line.Substring(q + 1);
                items = items.Substring(0, items.Length - 1);

                line = line.Substring(0, q - 1);
            }

            string[] p = line.Split(',');

            if (p.Length < 9)
                continue;

            int id = int.Parse(p[0]);
            string email = p[1];
            string restId = p[2];

            DateTime created = DateTime.Parse(p[6]);
            string status = p[8];
            string address = p[5];
            string payment = "CC";

            Customer cust = null;
            Restaurant rest = null;

            // Find customer
            for (int j = 0; j < customerCount; j++)
            {
                if (customers[j].EmailAddress == email)
                {
                    cust = customers[j];
                    break;
                }
            }

            // Find restaurant
            for (int j = 0; j < restaurants.Count; j++)
            {
                if (restaurants[j].RestaurantId == restId)
                {
                    rest = restaurants[j];
                    break;
                }
            }

            if (cust == null || rest == null)
                continue;

            Order o = new Order(
                id,
                created,
                status,
                address,
                payment
            );

            o.Customer = cust;
            o.Restaurant = rest;

            // Load items
            if (items.Length > 0)
            {
                string[] list = items.Split('|');

                double total = double.Parse(p[7]);
                double perItem = total / list.Length;

                foreach (string it in list)
                {
                    string[] x = it.Split(',');

                    if (x.Length < 2)
                        continue;

                    int qty = int.Parse(x[1]);

                    OrderedFoodItem of =
                        new OrderedFoodItem(
                            "Unknown",
                            "",
                            perItem,
                            "",
                            qty
                        );

                    o.AddOrderedFoodItem(of);
                }
            }

            o.CalculateOrderTotal();

            cust.AddOrder(o);
        }

        Console.WriteLine("Orders loaded.");
    }
    catch
    {
        Console.WriteLine("Cannot read orders.csv");
    }
}

// Feature 3: List Restaurant and Menus - Droydon Goh
DisplayRestaurantsAndMenus();
void DisplayRestaurantsAndMenus()
{
    Console.WriteLine("===== Restaurants and Menus =====");

    foreach (Restaurant r in restaurants)
    {
        Console.WriteLine("----------------------------");
        Console.WriteLine("Restaurant ID   : " + r.RestaurantId);
        Console.WriteLine("Restaurant Name : " + r.RestaurantName);
        Console.WriteLine("Email           : " + r.RestaurantEmail);
        Console.WriteLine("----------------------------");

        r.DisplayMenu();
        Console.WriteLine();
    }
}

