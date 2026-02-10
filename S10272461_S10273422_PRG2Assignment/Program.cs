//==========================================================
// Student Number : S10273422H
// Student Name : FanMing
// Partner Name : Droydon Goh
//==========================================================

using S10272461_S10273422_PRG2Assignment;

Customer[] customers = new Customer[100];
int customerCount = 0;

List<Restaurant> restaurants = new List<Restaurant>();

// Feature 1: Load Restaurants - Fan Ming
void LoadRestaurants()
{
    try
    {
        string[] restLines = File.ReadAllLines("restaurants.csv");

        for (int i = 1; i < restLines.Length; i++) // skip header
        {
            try
            {
                string[] p = restLines[i].Split(',');
                string id = p[0];
                string name = p[1];
                string email = p[2];

                Restaurant r = new Restaurant(id, name, email);

                // Create one default menu for each restaurant
                Menu defaultMenu = new Menu("Default", "Default Menu");
                r.AddMenu(defaultMenu);

                restaurants.Add(r);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error parsing restaurant line {i + 1}: {ex.Message}");
            }
        }

        Console.WriteLine($"Loaded {restaurants.Count} restaurants.");
    }
    catch (Exception ex)
    {
        Console.WriteLine("Error reading restaurants.csv: " + ex.Message);
    }
}

// Feature 1: Load Food Items - Fan Ming
void LoadFoodItems()
{
    try
    {
        string[] foodLines = File.ReadAllLines("fooditems.csv");

        for (int i = 1; i < foodLines.Length; i++) // skip header
        {
            try
            {
                string[] p = foodLines[i].Split(',');
                string restId = p[0];
                string itemName = p[1];
                string desc = p[2];
                double price = Convert.ToDouble(p[3]);

                FoodItem f = new FoodItem(itemName, desc, price, "None"); // simple default customize

                // Find the restaurant and add food item to its first menu
                for (int j = 0; j < restaurants.Count; j++)
                {
                    if (restaurants[j].RestaurantId == restId)
                    {
                        restaurants[j].Menus[0].AddFoodItem(f);
                        break; // found the restaurant, no need to continue loop
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error parsing food item line {i + 1}: {ex.Message}");
            }
        }

        Console.WriteLine("Food items loaded and assigned to restaurants.");
    }
    catch (Exception ex)
    {
        Console.WriteLine("Error reading fooditems.csv: " + ex.Message);
    }
}


// Feature 2 - Droydon Goh
LoadAllData();
void LoadAllData()
{
    string[] custLines = File.ReadAllLines("customers.csv");

    for (int i = 1; i < custLines.Length; i++)
    {
        string line = custLines[i];

   
        if (line.Length == 0)
            continue;

        string[] p = line.Split('\t');

  
        if (p.Length < 2)
            continue;

        Customer c = new Customer(p[1], p[0]);

        customers[customerCount] = c;
        customerCount++;
    }


    string[] orderLines = File.ReadAllLines("orders.csv");

    for (int i = 1; i < orderLines.Length; i++)
    {
        string line = orderLines[i];

        if (line.Length == 0)
            continue;

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


   
        for (int j = 0; j < customerCount; j++)
        {
            if (customers[j].EmailAddress == email)
            {
                cust = customers[j];
                break;
            }
        }

     
        for (int j = 0; j < restaurantCount; j++)
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

                OrderedFoodItem of = new OrderedFoodItem("Unknown","",perItem,"",qty);

                o.AddOrderedFoodItem(of);
            }
        }

        o.CalculateOrderTotal();

        cust.AddOrder(o);
    }

    Console.WriteLine("Customers and Orders loaded successfully");
}
