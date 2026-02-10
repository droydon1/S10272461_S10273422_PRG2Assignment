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


LoadRestaurants();
LoadFoodItems();
LoadCustomers();
LoadOrders();

Console.WriteLine();
Console.WriteLine("Welcome to the Gruberoo Food Delivery System");
Console.WriteLine($"{restaurants.Count} restaurants loaded!");
Console.WriteLine($"{restaurantMenus.Sum(m => m.GetFoodItems().Count)} food items loaded!");
Console.WriteLine($"{customerCount} customers loaded!");

int totalOrders = 0;
foreach (Customer c in customers)
{
    if (c != null)
        totalOrders += c.GetOrders().Count;
}
Console.WriteLine($"{totalOrders} orders loaded!");
Console.WriteLine();

RunMenu();


void RunMenu()
{
    while (true)
    {
        Console.WriteLine("\n===== Gruberoo Food Delivery System =====");
        Console.WriteLine("1. List all restaurants and menu items");
        Console.WriteLine("2. List all orders");
        Console.WriteLine("3. Create a new order");
        Console.WriteLine("4. Process an order");
        Console.WriteLine("5. Modify an order");
        Console.WriteLine("6. Delete an order");
        Console.WriteLine("7. Bulk process pending orders (Advanced)");
        Console.WriteLine("8. Display total order amounts (Advanced)");
        Console.WriteLine("0. Exit");
        Console.Write("Enter choice: ");

        string input = Console.ReadLine();

        Console.WriteLine();

        switch (input)
        {
            case "1":
                DisplayRestaurantsAndMenus();
                break;

            case "2":
                ListAllOrders();
                break;

            case "3":
                CreateNewOrder();
                break;

            case "4":
                ProcessOrders();
                break;

            case "5":
                ModifyOrder();
                break;

            case "6":
                DeleteOrder();
                break;

            case "7":
                BulkProcessPendingOrders();
                break;

            case "8":
                DisplayTotalOrderAmounts();
                break;

            case "0":
                Console.WriteLine("Thank you for using Gruberoo. Goodbye!");
                return;

            default:
                Console.WriteLine("Invalid choice. Please try again.");
                break;
        }
    }
}


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

// Feature 1: Load Food Items - Fan Ming
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

            string items = "";
            int q = raw.IndexOf('"');

            string line = raw;
            if (q != -1)
            {
                items = raw.Substring(q + 1);
                items = items.Substring(0, items.Length - 1); 
                line = raw.Substring(0, q - 1); 
            }

            string[] p = line.Split(',');

            if (p.Length < 9) continue;

            int id = int.Parse(p[0].Trim());
            string email = p[1].Trim();
            string restId = p[2].Trim();

            string deliveryDate = p[3].Trim(); 
            string deliveryTime = p[4].Trim(); 
            string deliveryAddress = p[5].Trim();

            DateTime created = DateTime.Parse(p[6].Trim());
            double totalAmount = double.Parse(p[7].Trim());
            string status = p[8].Trim();

            Customer cust = null;
            for (int j = 0; j < customerCount; j++)
            {
                if (customers[j].EmailAddress == email)
                {
                    cust = customers[j];
                    break;
                }
            }

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

            Order o = new Order(id, created, status, deliveryAddress, "CC");
            o.Customer = cust;
            o.Restaurant = rest;

            o.DeliveryDateTime = DateTime.Parse(deliveryDate + " " + deliveryTime);
            o.OrderTotal = totalAmount;

            if (items.Length > 0)
            {
                string[] list = items.Split('|');

                foreach (string it in list)
                {
                    string itemPart = it.Trim(); 
                    if (itemPart.Length == 0) continue;

                    string[] x = itemPart.Split(',');

                    if (x.Length < 2) continue;

                    string itemName = x[0].Trim();
                    int qty = int.Parse(x[1].Trim());

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
// Feature 3: Display Restaurant and Menus - Droydon Goh
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

void CreateNewOrder()
{
    Console.WriteLine("\n===== Create New Order =====");

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
            Console.WriteLine("Customer not found. Try again.");
    }

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
            Console.WriteLine("Restaurant not found. Try again.");
    }

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
            Console.WriteLine("Invalid date/time format. Try again.");
        }
    }

    Console.Write("Enter Delivery Address: ");
    string address = Console.ReadLine();
    
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

    Menu menu = restaurantMenus[restaurants.IndexOf(rest)];
    List<FoodItem> foodList = menu.GetFoodItems();

    Console.WriteLine("\nAvailable Food Items:");

    for (int i = 0; i < foodList.Count; i++)
    {
        Console.WriteLine($"{i + 1}. {foodList[i].ItemName} - ${foodList[i].ItemPrice:F2}");
    }

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

    double total = order.CalculateOrderTotal();
    double deliveryFee = 5.00;

    Console.WriteLine($"\nOrder Total: ${total:F2} + ${deliveryFee:F2} (delivery) = ${(total + deliveryFee):F2}");

    Console.Write("Proceed to payment? (Y/N): ");
    string pay = Console.ReadLine().ToUpper();

    if (pay != "Y")
    {
        Console.WriteLine("Order cancelled.");
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

        Console.WriteLine("Invalid payment option.");
    }

    order.OrderPaymentMethod = payment;
    
    cust.AddOrder(order);

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

    Console.WriteLine($"\nOrder {order.OrderId} created successfully! Status: Pending");
}

// Feature 6: Process an Order - Fan Ming
void ProcessOrders()
{
    Console.WriteLine("\n===== Process Orders =====");

    Console.Write("Enter Restaurant ID: ");
    string restId = Console.ReadLine().Trim();

    Restaurant rest = null;
    foreach (Restaurant r in restaurants)
    {
        if (r.RestaurantId == restId)
        {
            rest = r;
            break;
        }
    }

    if (rest == null)
    {
        Console.WriteLine(" Restaurant not found.");
        return;
    }

    List<Order> orderQueue = new List<Order>();

    foreach (Customer c in customers)
    {
        if (c == null) continue;

        foreach (Order o in c.GetOrders())
        {
            if (o.Restaurant.RestaurantId == restId)
            {
                orderQueue.Add(o);
            }
        }
    }

    if (orderQueue.Count == 0)
    {
        Console.WriteLine("No orders to process for this restaurant.");
        return;
    }

    foreach (Order o in orderQueue)
    {
        Console.WriteLine("\n---------------------------");
        Console.WriteLine($"Order {o.OrderId}:");
        Console.WriteLine($"Customer: {o.Customer.CustomerName}");
        Console.WriteLine("Ordered Items:");
        foreach (OrderedFoodItem item in o.GetOrderedItems())
        {
            Console.WriteLine($"{item.ItemName} - {item.QtyOrdered}");
        }
        Console.WriteLine($"Delivery date/time: {o.DeliveryDateTime:dd/MM/yyyy HH:mm}");
        Console.WriteLine($"Total Amount: ${o.OrderTotal:F2}");
        Console.WriteLine($"Order Status: {o.OrderStatus}");

        // Prompt for action
        while (true)
        {
            Console.Write("[C]onfirm / [R]eject / [S]kip / [D]eliver: ");
            string input = Console.ReadLine().Trim().ToUpper();

            if (input == "C")
            {
                if (o.OrderStatus == "Pending")
                {
                    o.OrderStatus = "Preparing";
                    Console.WriteLine($" Order {o.OrderId} confirmed. Status: Preparing");
                }
                else
                {
                    Console.WriteLine($" Cannot confirm. Current status: {o.OrderStatus}");
                }
                break;
            }
            else if (input == "R")
            {
                if (o.OrderStatus == "Pending")
                {
                    o.OrderStatus = "Rejected";
                    Console.WriteLine($" Order {o.OrderId} rejected. Refund initiated.");
                }
                else
                {
                    Console.WriteLine($" Cannot reject. Current status: {o.OrderStatus}");
                }
                break;
            }
            else if (input == "S")
            {
                Console.WriteLine($" Skipping Order {o.OrderId}");
                break;
            }
            else if (input == "D")
            {
                if (o.OrderStatus == "Preparing")
                {
                    o.OrderStatus = "Delivered";
                    Console.WriteLine($" Order {o.OrderId} delivered.");
                }
                else
                {
                    Console.WriteLine($" Cannot deliver. Current status: {o.OrderStatus}");
                }
                break;
            }
            else
            {
                Console.WriteLine(" Invalid input. Please enter C, R, S, or D.");
            }
        }
    }

    Console.WriteLine("\n All orders processed.");
}

// Feature 7: Modify order - Droydon Goh

void ModifyOrder()
{
    Console.WriteLine("\n===== Modify Order =====");

    Customer cust = null;

    while (cust == null)
    {
        Console.Write("Enter Customer Email: ");
        string email = Console.ReadLine().Trim();

        for (int i = 0; i < customerCount; i++)
        {
            if (customers[i] != null &&
                customers[i].EmailAddress == email)
            {
                cust = customers[i];
                break;
            }
        }

        if (cust == null)
            Console.WriteLine("Customer not found. Try again.");
    }

    List<Order> pending = new List<Order>();

    Console.WriteLine("\nPending Orders:");

    foreach (Order o in cust.GetOrders())
    {
        if (o.OrderStatus == "Pending")
        {
            Console.WriteLine(o.OrderId);
            pending.Add(o);
        }
    }

    if (pending.Count == 0)
    {
        Console.WriteLine("No pending orders to modify.");
        return;
    }

    Order selected = null;

    while (selected == null)
    {
        Console.Write("Enter Order ID: ");
        int id;

        if (!int.TryParse(Console.ReadLine(), out id))
        {
            Console.WriteLine("Invalid number.");
            continue;
        }

        foreach (Order o in pending)
        {
            if (o.OrderId == id)
            {
                selected = o;
                break;
            }
        }

        if (selected == null)
            Console.WriteLine("Order not found.");
    }
    
    Console.WriteLine("\nOrder Items:");

    int idx = 1;
    foreach (OrderedFoodItem item in selected.GetOrderedItems())
    {
        Console.WriteLine($"{idx}. {item.ItemName} - {item.QtyOrdered}");
        idx++;
    }

    Console.WriteLine("\nAddress:");
    Console.WriteLine(selected.DeliveryAddress);

    Console.WriteLine("\nDelivery Date/Time:");
    Console.WriteLine(selected.DeliveryDateTime.ToString("dd/MM/yyyy HH:mm"));

    Console.WriteLine("\nModify:");
    Console.WriteLine("[1] Items");
    Console.WriteLine("[2] Address");
    Console.WriteLine("[3] Delivery Time");

    int choice;

    while (true)
    {
        Console.Write("Enter choice: ");

        if (int.TryParse(Console.ReadLine(), out choice) &&
            choice >= 1 && choice <= 3)
            break;

        Console.WriteLine("Invalid choice.");
    }


    double oldTotal = selected.OrderTotal;

    if (choice == 1)
    {
        selected.GetOrderedItems().Clear();

        Console.WriteLine("\nRe-enter items:");

        Menu menu = restaurantMenus[restaurants.IndexOf(selected.Restaurant)];
        List<FoodItem> foods = menu.GetFoodItems();

        for (int i = 0; i < foods.Count; i++)
        {
            Console.WriteLine($"{i + 1}. {foods[i].ItemName} - ${foods[i].ItemPrice:F2}");
        }

        while (true)
        {
            Console.Write("\nEnter item number (0 to finish): ");
            int num = int.Parse(Console.ReadLine());

            if (num == 0)
                break;

            if (num < 1 || num > foods.Count)
            {
                Console.WriteLine("Invalid item.");
                continue;
            }

            Console.Write("Enter quantity: ");
            int qty = int.Parse(Console.ReadLine());

            if (qty <= 0)
            {
                Console.WriteLine("Invalid quantity.");
                continue;
            }

            FoodItem f = foods[num - 1];

            OrderedFoodItem of =
                new OrderedFoodItem(
                    f.ItemName,
                    f.ItemDesc,
                    f.ItemPrice,
                    "",
                    qty
                );

            selected.AddOrderedFoodItem(of);
        }
    }

    else if (choice == 2)
    {
        Console.Write("Enter new Address: ");
        selected.DeliveryAddress = Console.ReadLine();

        Console.WriteLine("Address updated.");
    }

    else if (choice == 3)
    {
        while (true)
        {
            try
            {
                Console.Write("Enter new Delivery Time (HH:mm): ");
                string t = Console.ReadLine();

                DateTime newDT = DateTime.Parse(
                    selected.DeliveryDateTime.ToString("dd/MM/yyyy") + " " + t
                );

                selected.DeliveryDateTime = newDT;

                Console.WriteLine($"New Delivery Time: {t}");
                break;
            }
            catch
            {
                Console.WriteLine("Invalid time format.");
            }
        }
    }

    double newTotal = selected.CalculateOrderTotal();
    selected.OrderTotal = newTotal;

    if (newTotal > oldTotal)
    {
        Console.WriteLine($"\nOld Total: ${oldTotal:F2}");
        Console.WriteLine($"New Total: ${newTotal:F2}");

        Console.Write("Additional payment required. Pay now? (Y/N): ");
        string pay = Console.ReadLine().ToUpper();

        if (pay != "Y")
        {
            Console.WriteLine("Update cancelled.");
            return;
        }

        Console.WriteLine("Payment received.");
    }

    Console.WriteLine($"\nOrder {selected.OrderId} updated successfully!");
}


// Feature 8: Delete an Existing Order - Fan Ming
void DeleteOrder()
{
    Console.WriteLine("\n===== Delete Order =====");

    Customer cust = null;

    while (cust == null)
    {
        Console.Write("Enter Customer Email: ");
        string email = Console.ReadLine().Trim();

        for (int i = 0; i < customerCount; i++)
        {
            if (customers[i] != null && customers[i].EmailAddress == email)
            {
                cust = customers[i];
                break;
            }
        }

        if (cust == null)
            Console.WriteLine(" Customer not found. Try again.");
    }

    List<Order> pendingOrders = new List<Order>();

    Console.WriteLine("\nPending Orders:");
    foreach (Order o in cust.GetOrders())
    {
        if (o.OrderStatus == "Pending")
        {
            Console.WriteLine(o.OrderId);
            pendingOrders.Add(o);
        }
    }

    if (pendingOrders.Count == 0)
    {
        Console.WriteLine("No pending orders to delete.");
        return;
    }

    Order selectedOrder = null;

    while (selectedOrder == null)
    {
        Console.Write("Enter Order ID to delete: ");
        int id;

        if (!int.TryParse(Console.ReadLine(), out id))
        {
            Console.WriteLine(" Invalid number. Try again.");
            continue;
        }

        foreach (Order o in pendingOrders)
        {
            if (o.OrderId == id)
            {
                selectedOrder = o;
                break;
            }
        }

        if (selectedOrder == null)
            Console.WriteLine(" Order not found in pending orders. Try again.");
    }

    Console.WriteLine($"\nCustomer: {selectedOrder.Customer.CustomerName}");
    Console.WriteLine("Ordered Items:");
    foreach (OrderedFoodItem item in selectedOrder.GetOrderedItems())
    {
        Console.WriteLine($"{item.ItemName} - {item.QtyOrdered}");
    }
    Console.WriteLine($"Delivery date/time: {selectedOrder.DeliveryDateTime:dd/MM/yyyy HH:mm}");
    Console.WriteLine($"Total Amount: ${selectedOrder.OrderTotal:F2}");
    Console.WriteLine($"Order Status: {selectedOrder.OrderStatus}");

    while (true)
    {
        Console.Write("Confirm deletion? [Y/N]: ");
        string confirm = Console.ReadLine().Trim().ToUpper();

        if (confirm == "Y")
        {
            selectedOrder.OrderStatus = "Cancelled";
            Console.WriteLine($" Order {selectedOrder.OrderId} cancelled. Refund of ${selectedOrder.OrderTotal:F2} processed.");
            break;
        }
        else if (confirm == "N")
        {
            Console.WriteLine(" Deletion cancelled.");
            break;
        }
        else
        {
            Console.WriteLine(" Invalid input. Enter Y or N.");
        }
    }
}

// Advanced Features 1: Bulk Process Pending Orders - Droydon Goh
void BulkProcessPendingOrders()
{
    Console.WriteLine("\n===== Bulk Processing Pending Orders (Today) =====");

    DateTime today = DateTime.Today;

    List<Order> pendingToday = new List<Order>();

    foreach (Customer c in customers)
    {
        if (c == null) continue;

        foreach (Order o in c.GetOrders())
        {
            if (o.OrderStatus == "Pending" &&
                o.DeliveryDateTime.Date == today)
            {
                pendingToday.Add(o);
            }
        }
    }

    Console.WriteLine($"Total Pending Orders Today: {pendingToday.Count}");

    if (pendingToday.Count == 0)
    {
        Console.WriteLine("No pending orders for today.");
        return;
    }

    int processed = 0;
    int preparing = 0;
    int rejected = 0;

    DateTime now = DateTime.Now;

    foreach (Order o in pendingToday)
    {
        TimeSpan diff = o.DeliveryDateTime - now;

        if (diff.TotalMinutes < 60)
        {
            o.OrderStatus = "Rejected";
            rejected++;

            Console.WriteLine(
                $"Order {o.OrderId} -> Rejected (Too Late)"
            );
        }

        else
        {
            o.OrderStatus = "Preparing";
            preparing++;

            Console.WriteLine(
                $"Order {o.OrderId} -> Preparing"
            );
        }

        processed++;
    }

    Console.WriteLine("\n===== Summary =====");
    Console.WriteLine($"Orders Processed : {processed}");
    Console.WriteLine($"Preparing        : {preparing}");
    Console.WriteLine($"Rejected         : {rejected}");

    int totalOrders = 0;

    foreach (Customer c in customers)
    {
        if (c == null) continue;
        totalOrders += c.GetOrders().Count;
    }

    double percent = 0;

    if (totalOrders > 0)
    {
        percent = (double)processed / totalOrders * 100;
    }

    Console.WriteLine(
        $"Auto-Processed % : {percent:F2}%"
    );

    Console.WriteLine("==============================");
}



// Advanced Feature B: Display Total Order Amount per Restaurant - Fan Ming
void DisplayTotalOrderAmounts()
{
    Console.WriteLine("\n===== Total Order Amounts per Restaurant =====\n");

    double grandTotalSales = 0;
    double grandTotalRefunds = 0;
    double deliveryFee = 5.00; 

    foreach (Restaurant r in restaurants)
    {
        double totalSales = 0;
        double totalRefunds = 0;

        foreach (Customer c in customers)
        {
            if (c == null) continue;

            foreach (Order o in c.GetOrders())
            {
                if (o.Restaurant.RestaurantId != r.RestaurantId)
                    continue;

                if (o.OrderStatus == "Delivered")
                {
                    totalSales += o.OrderTotal - deliveryFee;
                }
                if (o.OrderStatus == "Rejected" || o.OrderStatus == "Cancelled")
                {
                    totalRefunds += o.OrderTotal;
                }
            }
        }

        grandTotalSales += totalSales;
        grandTotalRefunds += totalRefunds;

        Console.WriteLine($"Restaurant: {r.RestaurantName} ({r.RestaurantId})");
        Console.WriteLine($"  Total Sales (after delivery fee): ${totalSales:F2}");
        Console.WriteLine($"  Total Refunds: ${totalRefunds:F2}");
        Console.WriteLine();
    }

    double finalEarnings = grandTotalSales - grandTotalRefunds;

    Console.WriteLine("===== Gruberoo Overall Summary =====");
    Console.WriteLine($"Total Sales    : ${grandTotalSales:F2}");
    Console.WriteLine($"Total Refunds  : ${grandTotalRefunds:F2}");
    Console.WriteLine($"Final Earnings : ${finalEarnings:F2}");
    Console.WriteLine("==============================\n");
}