//==========================================================
// Student Number : S10273422H
// Student Name : FanMing
// Partner Name : Droydon Goh
//==========================================================

using S10272461_S10273422_PRG2Assignment;

Restaurant[] restaurants = new Restaurant[50];
int restaurantCount = 0;

Customer[] customers = new Customer[100];
int customerCount = 0;

Menu[] menus = new Menu[50];


// Feature 2 - Droydon Goh
LoadAllData();
void LoadAllData()
{

    string[] restLines = File.ReadAllLines("restaurants.csv");

    for (int i = 1; i < restLines.Length; i++)
    {
        if (string.IsNullOrWhiteSpace(restLines[i])) continue;

        string[] p = restLines[i].Split(',');

        Restaurant r = new Restaurant(p[0], p[1], p[2]);

        restaurants[restaurantCount] = r;
        restaurantCount++;
    }


    for (int i = 0; i < restaurantCount; i++)
    {
        Menu m = new Menu("M001", "Main Menu");
        restaurants[i].AddMenu(m);

        menus[i] = m;
    }

    string[] foodLines = File.ReadAllLines("fooditems.csv");

    for (int i = 1; i < foodLines.Length; i++)
    {
        if (string.IsNullOrWhiteSpace(foodLines[i])) continue;

        string[] p = foodLines[i].Split(',');

        string restId = p[0];

        for (int j = 0; j < restaurantCount; j++)
        {
            if (restaurants[j].RestaurantId == restId)
            {
                Menu m = menus[j];

                FoodItem f = new FoodItem(
                    p[1],
                    p[2],
                    double.Parse(p[3]),
                    ""
                );

                m.AddFoodItem(f);
                break;
            }
        }
    }



    string[] custLines = File.ReadAllLines("customers.csv");

    for (int i = 1; i < custLines.Length; i++)
    {
        if (string.IsNullOrWhiteSpace(custLines[i])) continue;

        string[] p = custLines[i].Split(',');

        Customer c = new Customer(p[1], p[0]);

        customers[customerCount] = c;
        customerCount++;
    }


    string[] orderLines = File.ReadAllLines("orders.csv");

    for (int i = 1; i < orderLines.Length; i++)
    {
        if (string.IsNullOrWhiteSpace(orderLines[i])) continue;

        string line = orderLines[i];

        int q = line.IndexOf('"');
        string items = "";

        if (q != -1)
        {
            items = line.Substring(q + 1);
            items = items.Substring(0, items.Length - 1);
            line = line.Substring(0, q - 1);
        }

        string[] p = line.Split(',');

        int id = int.Parse(p[0]);
        string email = p[1];
        string restId = p[2];

        DateTime created = DateTime.Parse(p[6]);
        string status = p[8];

        Customer cust = null;
        Restaurant rest = null;



        for (int j = 0; j < customerCount; j++)
        {
            if (customers[j].EmailAddress == email)
                cust = customers[j];
        }

        for (int j = 0; j < restaurantCount; j++)
        {
            if (restaurants[j].RestaurantId == restId)
                rest = restaurants[j];
        }

        if (cust == null || rest == null) continue;

        DateTime deliveryDT = DateTime.Parse(p[3] + " " + p[4]);

        Order o = new Order(id,created,status,p[5],"CC");

        o.DeliveryDateTime = deliveryDT;




        if (items != "")
        {
            string[] list = items.Split('|');

            double total = double.Parse(p[7]);
            double perItem = total / list.Length;

            foreach (string it in list)
            {
                string[] x = it.Split(',');

                int qty = int.Parse(x[1]);

                OrderedFoodItem of = new OrderedFoodItem("Unknown","",perItem,"",qty);


                o.AddOrderedFoodItem(of);
            }
        }

        o.CalculateOrderTotal();

        cust.AddOrder(o);
    }

    Console.WriteLine("Feature 2: All data loaded.");
}
