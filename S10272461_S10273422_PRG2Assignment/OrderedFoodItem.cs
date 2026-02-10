using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace S10272461_S10273422_PRG2Assignment
{
    internal class OrderedFoodItem : FoodItem
    {
        private int qtyOrdered;

        public int QtyOrdered
        {
            get { return qtyOrdered; }
            set { qtyOrdered = value; }
        }

        private double subTotal;

        public double SubTotal
        {
            get { return subTotal; }
            set { subTotal = value; }
        }

        public OrderedFoodItem(string name, string desc, double price, string customize, int qtyOrdered) : base(name, desc, price, customize)
        {
            this.qtyOrdered = qtyOrdered;
            this.subTotal = price * qtyOrdered;
        }

        public double CalculateSubTotal()
        {
            SubTotal = ItemPrice * QtyOrdered;
            return SubTotal;
        }

        public override string ToString()
        {
            return $"{base.ToString()} - Quantity Ordered: {qtyOrdered} - Subtotal: ${subTotal}";
        }

    }
}
