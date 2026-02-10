using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

//==========================================================
// Student Number : S10273422H
// Student Name : FanMing
// Partner Name : Droydon Goh
//==========================================================

namespace S10272461_S10273422_PRG2Assignment
{
    internal class SpecialOffer
    {
        private string offerCode;

        public string OfferCode
        {
            get { return offerCode; }
            set { offerCode = value; }
        }
        private string offerDesc;

        public string OfferDesc
        {
            get { return offerDesc; }
            set { offerDesc = value; }
        }
        private double discount;

        public double Discount
        {
            get { return discount; }
            set { discount = value; }
        }


        public SpecialOffer(string offerCode, string offerDesc, double discount)
        {
            this.offerCode = offerCode;
            this.offerDesc = offerDesc;
            this.discount = discount;
        }

     
        public override string ToString()
        {
            return $"Offer Code: {offerCode} - Description: {offerDesc} - Discount: {discount}%";

        }
    }
}
