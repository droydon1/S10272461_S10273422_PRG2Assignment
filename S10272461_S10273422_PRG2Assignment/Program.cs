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
void LoadRestaurants()
{
    int restCount = 0;
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
            restCount++;
        }
        
        Console.WriteLine($"Loaded {restCount} restaurants.");
    }
    catch (Exception ex)
    {
        Console.WriteLine("Error loading restaurants: " + ex.Message);
    }
}
LoadRestaurants(); //Calling load restaurants function

// Feature 1: Load Food Items - Fan Ming
LoadFoodItems();
void LoadFoodItems()
{
    int foodCount = 0;
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
                    foodCount++;
                    break;
                }
            }
        }

        Console.WriteLine($"Loaded {foodCount} food items.");
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
    int custLoaded = 0;

    try
    {
        string[] custLines = File.ReadAllLines("customers.csv");

        for (int i = 1; i < custLines.Length; i++)
        {
            string line = custLines[i].Trim();
            if (line.Length == 0) continue;

            string[] p = line.Split(',');

            if (p.Length < 2) continue;

            string name = p[0].Trim();
            string email = p[1].Trim();

            Customer c = new Customer(email, name);

            customers[customerCount] = c;
            customerCount++;
            custLoaded++;
        }

        Console.WriteLine($"Loaded {custLoaded} customers.");
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
    int orderCount = 0;

    try
    {
        string[] lines = File.ReadAllLines("orders.csv");

        for (int i = 1; i < lines.Length; i++)
        {
            string raw = lines[i].Trim();
            if (raw.Length == 0) continue;

            // Extract items part (inside quotes) if exists
            string items = "";
            int q = raw.IndexOf('"');

            string line = raw;
            if (q != -1)
            {
                items = raw.Substring(q + 1);
                items = items.Substring(0, items.Length - 1); // remove last quote
                line = raw.Substring(0, q - 1); // remove comma before quote
            }

            string[] p = line.Split(',');

            // After removing Items, we expect 9 columns (0..8)
            if (p.Length < 9) continue;

            int id = int.Parse(p[0].Trim());
            string email = p[1].Trim();
            string restId = p[2].Trim();

            string deliveryDate = p[3].Trim(); // dd/MM/yyyy
            string deliveryTime = p[4].Trim(); // HH:mm
            string deliveryAddress = p[5].Trim();

            DateTime created = DateTime.Parse(p[6].Trim());
            double totalAmount = double.Parse(p[7].Trim());
            string status = p[8].Trim();

            // Find customer
            Customer cust = null;
            for (int j = 0; j < customerCount; j++)
            {
                if (customers[j].EmailAddress == email)
                {
                    cust = customers[j];
                    break;
                }
            }

            // Find restaurant
            Restaurant rest = null;
            for (int j = 0; j < restaurants.Count; j++)
            {
                if (restaurants[j].RestaurantId == restId)
                {
                    rest = restaurants[j];
                    break;
                }
            }

            if (cust == null || rest == null) continue;

            // Create order using YOUR constructor (don’t change constructor)
            Order o = new Order(id, created, status, deliveryAddress, "CC");
            o.Customer = cust;
            o.Restaurant = rest;

            // Set extra fields based on CSV format
            o.DeliveryDateTime = DateTime.Parse(deliveryDate + " " + deliveryTime);
            o.OrderTotal = totalAmount;

            // Load ordered items (your class uses name/desc/price/customise/qty)
            if (items.Length > 0)
            {
                string[] list = items.Split('|');

                foreach (string it in list)
                {
                    string itemPart = it.Trim(); // e.g. "Chicken Rice,1"
                    if (itemPart.Length == 0) continue;

                    string[] x = itemPart.Split(',');

                    if (x.Length < 2) continue;

                    string itemName = x[0].Trim();
                    int qty = int.Parse(x[1].Trim());

                    // We can’t easily get true price without searching menu,
                    // so just put 0 or a simple estimate.
                    OrderedFoodItem of = new OrderedFoodItem(itemName, "", 0, "", qty);

                    o.AddOrderedFoodItem(of);
                }
            }

            o.OrderTotal = double.Parse(p[7]);



            cust.AddOrder(o);
            orderCount++;
        }

        Console.WriteLine($"Loaded {orderCount} orders.");
    }
    catch (Exception ex)
    {
        Console.WriteLine("Error loading orders: " + ex.Message);
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

// Feature 4: List All Orders - Fan Ming
ListAllOrders();
void ListAllOrders()
{
    try
    {
        Console.WriteLine("All Orders");
        Console.WriteLine("==========");

        Console.WriteLine(
            "{0,-8} {1,-14} {2,-16} {3,-20} {4,-12} {5,-10}",
            "OrderID",
            "Customer",
            "Restaurant",
            "Delivery Date/Time",
            "Amount",
            "Status"
        );

        Console.WriteLine(
            "{0,-8} {1,-14} {2,-16} {3,-20} {4,-12} {5,-10}",
            "--------",
            "--------------",
            "----------------",
            "--------------------",
            "------------",
            "----------"
        );

        for (int i = 0; i < customerCount; i++)
        {
            Customer c = customers[i];

            foreach (Order o in c.GetOrders())
            {
                Console.WriteLine(
                    "{0,-8} {1,-14} {2,-16} {3,-20} {4,-10:C2} {5,-10}",
                    o.OrderId,
                    c.CustomerName,
                    o.Restaurant.RestaurantName,
                    o.OrderDateTime.ToString("dd/MM/yyyy HH:mm"),
                    o.OrderTotal,
                    o.OrderStatus
                );
            }
        }
    }
    catch (Exception ex)
    {
        Console.WriteLine("Error listing orders: " + ex.Message);
    }
}

// Feature 5: Create New Order - Droydon
CreateNewOrder();
// Feature 5: Create New Order
void CreateNewOrder()
{
    Console.WriteLine("\n===== Create New Order =====");

    // -----------------------------
    // Get Customer
    // -----------------------------
    Customer cust = null;

    while (cust == null)
    {
        Console.Write("Enter Customer Email: ");
        string email = Console.ReadLine();

        for (int i = 0; i < customerCount; i++)
        {
            if (customers[i].EmailAddress == email)
            {
                cust = customers[i];
                break;
            }
        }

        if (cust == null)
            Console.WriteLine("❌ Customer not found. Try again.");
    }


    // -----------------------------
    // Get Restaurant
    // -----------------------------
    Restaurant rest = null;

    while (rest == null)
    {
        Console.Write("Enter Restaurant ID: ");
        string restId = Console.ReadLine();

        foreach (Restaurant r in restaurants)
        {
            if (r.RestaurantId == restId)
            {
                rest = r;
                break;
            }
        }

        if (rest == null)
            Console.WriteLine("❌ Restaurant not found. Try again.");
    }


    // -----------------------------
    // Get Delivery Date & Time
    // -----------------------------
    DateTime deliveryDT;

    while (true)
    {
        try
        {
            Console.Write("Enter Delivery Date (dd/MM/yyyy): ");
            string d = Console.ReadLine();

            Console.Write("Enter Delivery Time (HH:mm): ");
            string t = Console.ReadLine();

            deliveryDT = DateTime.Parse($"{d} {t}");
            break;
        }
        catch
        {
            Console.WriteLine("❌ Invalid date/time format. Try again.");
        }
    }


    // -----------------------------
    // Get Address
    // -----------------------------
    Console.Write("Enter Delivery Address: ");
    string address = Console.ReadLine();


    // -----------------------------
    // Create Order ID
    // -----------------------------
    int newId = 1000;

    foreach (Customer c in customers)
    {
        if (c == null) continue;

        foreach (Order o in c.GetOrders())
        {
            if (o.OrderId >= newId)
                newId = o.OrderId + 1;
        }
    }


    // -----------------------------
    // Create Order
    // -----------------------------
    Order order = new Order(
        newId,
        DateTime.Now,
        "Pending",
        address,
        ""
    );

    order.Customer = cust;
    order.Restaurant = rest;
    order.DeliveryDateTime = deliveryDT;


    // -----------------------------
    // Show Menu
    // -----------------------------
    Menu menu = restaurantMenus[restaurants.IndexOf(rest)];
    List<FoodItem> foodList = menu.GetFoodItems();

    Console.WriteLine("\nAvailable Food Items:");

    for (int i = 0; i < foodList.Count; i++)
    {
        Console.WriteLine($"{i + 1}. {foodList[i].ItemName} - ${foodList[i].ItemPrice:F2}");
    }


    // -----------------------------
    // Select Items
    // -----------------------------
    while (true)
    {
        Console.Write("\nEnter item number (0 to finish): ");
        int choice = int.Parse(Console.ReadLine());

        if (choice == 0)
            break;

        if (choice < 1 || choice > foodList.Count)
        {
            Console.WriteLine("❌ Invalid item number.");
            continue;
        }

        Console.Write("Enter quantity: ");
        int qty = int.Parse(Console.ReadLine());

        if (qty <= 0)
        {
            Console.WriteLine("❌ Invalid quantity.");
            continue;
        }

        FoodItem f = foodList[choice - 1];

        OrderedFoodItem of = new OrderedFoodItem(
            f.ItemName,
            f.ItemDesc,
            f.ItemPrice,
            "",
            qty
        );

        order.AddOrderedFoodItem(of);
    }


    // -----------------------------
    // Special Request
    // -----------------------------
    Console.Write("Add special request? (Y/N): ");
    string special = Console.ReadLine().ToUpper();

    if (special == "Y")
    {
        Console.Write("Enter special request: ");
        string req = Console.ReadLine();

        foreach (OrderedFoodItem item in order.GetOrderedItems())
        {
            item.Customize = req;
        }
    }


    // -----------------------------
    // Calculate Total
    // -----------------------------
    double total = order.CalculateOrderTotal();
    double deliveryFee = 5.00;

    Console.WriteLine($"\nOrder Total: ${total:F2} + ${deliveryFee:F2} (delivery) = ${(total + deliveryFee):F2}");


    // -----------------------------
    // Payment
    // -----------------------------
    Console.Write("Proceed to payment? (Y/N): ");
    string pay = Console.ReadLine().ToUpper();

    if (pay != "Y")
    {
        Console.WriteLine("❌ Order cancelled.");
        return;
    }

    string payment = "";

    while (true)
    {
        Console.WriteLine("\nPayment Method:");
        Console.Write("[CC] Credit Card / [PP] PayPal / [CD] Cash on Delivery: ");
        payment = Console.ReadLine().ToUpper();

        if (payment == "CC" || payment == "PP" || payment == "CD")
            break;

        Console.WriteLine("❌ Invalid payment option.");
    }

    order.OrderPaymentMethod = payment;


    // -----------------------------
    // Save Order
    // -----------------------------
    cust.AddOrder(order);


    // Append to CSV
    string csvLine =
        $"{order.OrderId}," +
        $"{cust.EmailAddress}," +
        $"{rest.RestaurantId}," +
        $"{order.DeliveryDateTime:dd/MM/yyyy}," +
        $"{order.DeliveryDateTime:HH:mm}," +
        $"{address}," +
        $"{DateTime.Now}," +
        $"{order.OrderTotal}," +
        $"{order.OrderStatus}," +
        $"\"\"";

    File.AppendAllText("orders.csv", "\n" + csvLine);


    // -----------------------------
    // Done
    // -----------------------------
    Console.WriteLine($"\n✅ Order {order.OrderId} created successfully! Status: Pending");
}
