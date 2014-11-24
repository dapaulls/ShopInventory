using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ShopInventory
{
    class Product
    {
        private decimal fastSellingFactor = 1.25M;
        private decimal slowSellingFactor = 1.15M;

        public decimal FastSellingFactor
        {
            get { return fastSellingFactor; }
            set { fastSellingFactor = value; }
        }

        public decimal SlowSellingFactor
        {
            get { return slowSellingFactor; }
            set { slowSellingFactor = value; }
        }
        
        public string ProductName { get; set; }
        public string ProductBrand { get; set; }
        public string ProductDescription { get; set; }
        public decimal ProductCostPrice { get; set; }
        public char ProductFastSelling { get; set; }
        public int ProductQuantity { get; set; }        
    }
}
