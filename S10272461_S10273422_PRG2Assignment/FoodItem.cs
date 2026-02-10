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

namespace S10272461_S10273422_PRG2Assignment
{
	internal class FoodItem
    {
        private string itemName;

        public string ItemName
        {
            get { return itemName; }
            set { itemName = value; }
        }

        private string itemDesc;

        public string ItemDesc
        {
            get { return itemDesc; }
            set { itemDesc = value; }
        }

        private double itemPrice;

        public double ItemPrice
        {
            get { return itemPrice; }
            set { itemPrice = value; }
        }

        private string customize;

        public string Customzie
        {
            get { return customize; }
            set { customize = value; }
        }

        public FoodItem(string name, string desc, double price, string customize)
        {
            this.itemName = name;
            this.itemDesc = desc;
            this.itemPrice = price;
            this.customize = customize;
        }

        public override string ToString()
        {
            return $"{itemName} - {itemDesc} - ${itemPrice} - Customization: {customize}";
        }
    }
}
