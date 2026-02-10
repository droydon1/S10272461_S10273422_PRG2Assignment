using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace S10272461_S10273422_PRG2Assignment
{
    public class FoodItem
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

        public string Customize
        {
            get { return customize; }
            set { customize = value; }
        }

        public FoodItem(string itemName, string itemDesc, double itemPrice, string customize)
        {
            this.itemName = itemName;
            this.itemDesc = itemDesc;
            this.itemPrice = itemPrice;
            this.customize = customize;

        }

        public override string ToString()
        {
            return itemName + ": " + itemDesc + " - $" + itemPrice.ToString("0.00");
        }

    }
}
