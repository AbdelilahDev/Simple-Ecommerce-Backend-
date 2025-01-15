using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EcommerceDataAccessLayer
{
    public class clsTest
    {
        public clsTest(int OrderDetailID, int OrderID, int ProductID, int Quantity, decimal Price, decimal TotalAmount)
        {
            this.OrderDetailID = OrderDetailID;
            this.OrderID = OrderID;
            this.ProductID = ProductID;
            this.Quantity = Quantity;
            this.Price = Price;
            this.TotalAmount = TotalAmount;

            //   , int ProductID, int Quantity, decimal Price, decimal TotalPrice

        }


        public int OrderDetailID { get; set; }
        public int OrderID { get; set; }
        public int ProductID { get; set; }
        public int Quantity { get; set; }
        public decimal Price { get; set; }
        public decimal TotalAmount { get; set; }
    }
}
