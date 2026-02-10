using S10272461_S10273422_PRG2Assignment;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public class Menu
{
    public string MenuId { get; set; }
    public string MenuName { get; set; }
    private FoodItem[] foodItems;
    private int itemCount;

    public Menu(string menuId, string menuName)
    {
        MenuId = menuId;
        MenuName = menuName;
        foodItems = new FoodItem[100];
        itemCount = 0;
    }

    public void AddFoodItem(FoodItem foodItem)
    {
        if (itemCount < foodItems.Length)
        {
            foodItems[itemCount] = foodItem;
            itemCount++;
            Console.WriteLine("Food item added successfully!");
        }
        else
        {
            Console.WriteLine("Menu is full! Cannot add more items.");
        }
    }

    public bool RemoveFoodItem(FoodItem foodItem)
    {
        for (int i = 0; i < itemCount; i++)
        {
            if (foodItems[i] == foodItem)
            {
                for (int j = i; j < itemCount - 1; j++)
                {
                    foodItems[j] = foodItems[j + 1];
                }
                foodItems[itemCount - 1] = null;
                itemCount--;
                Console.WriteLine("Food item removed successfully!");
                return true;
            }
        }
        Console.WriteLine("Food item not found!");
        return false;
    }

    public void DisplayFoodItems()
    {
        Console.WriteLine("Menu: " + MenuName);
        if (itemCount == 0)
        {
            Console.WriteLine("No items in this menu.");
            return;
        }
        for (int i = 0; i < itemCount; i++)
        {
            Console.WriteLine((i + 1) + ". " + foodItems[i]);
        }
    }

    public FoodItem[] GetFoodItems()
    {
        FoodItem[] items = new FoodItem[itemCount];
        for (int i = 0; i < itemCount; i++)
        {
            items[i] = foodItems[i];
        }
        return items;
    }

    public int GetItemCount()
    {
        return itemCount;
    }

    public override string ToString()
    {
        return $"Menu {{ MenuId='{MenuId}', MenuName='{MenuName}', ItemCount={itemCount} }}";
    }
}

