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
