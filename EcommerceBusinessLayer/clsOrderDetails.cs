using EcommerceDataAccessLayer;

namespace EcommerceBusinessLayer
{
    public class clsOrderDetails
    {
        public enum enMode { AddNew = 0, Update = 1 };
        enMode Mode = enMode.AddNew;


        public int OrderDetailID { get; set; }
        public int OrderID { get; set; }
        public int ProductID { get; set; }
        public int Quantity { get; set; }
        public decimal Price { get; set; }
        public decimal TotalAmount { get; set; }

        public OrderDetailsDTO orderDetailsDTO
        {
            get
            {
                return (new OrderDetailsDTO(this.OrderDetailID, this.OrderID, this.ProductID, this.Quantity,this.Price,this.TotalAmount));
            }
        }

        public clsOrderDetails(OrderDetailsDTO orderDetailsDTO, enMode cMode = enMode.AddNew)
        {
            this.OrderDetailID = orderDetailsDTO.OrderDetailID;
            this.OrderID = orderDetailsDTO.OrderID;
            this.ProductID = orderDetailsDTO.ProductID;
            this.Quantity = orderDetailsDTO.Quantity;
            this.Price = orderDetailsDTO.Price;
            this.TotalAmount = orderDetailsDTO.TotalAmount;

            Mode = cMode;

        }

        private bool _AddNewOrderDetails()
        {
            //call DataAccess Layer 
            this.OrderDetailID = clsOrderDetailsDataAccess.AddNewOrder(orderDetailsDTO);
            return (this.OrderDetailID != -1);
            
        }

        //private bool _UpdateOrderDetails()
        //{
        //    return true;

        //    //return clsProductDataAccess.UpdateProduct(productDTO);
        //}


        public static clsOrderDetails FindOrderDetials(int ID)
        {

            OrderDetailsDTO orderDetailsDTO = clsOrderDetailsDataAccess.GetInfoOrderDetailsById(ID);

            if (orderDetailsDTO != null)
            //we return new object of that student with the right data
            {

                return new clsOrderDetails(orderDetailsDTO, enMode.Update);
            }
            else
                return null;
        }

        public static List<OrderDetailsDTO> GetAllOrdersDetials()
        {
            return clsOrderDetailsDataAccess.GetAllOrdersDetials();
        }



        public bool Save()
        {
            switch (Mode)
            {
                case enMode.AddNew:
                    if (_AddNewOrderDetails())
                    {

                        Mode = enMode.Update;
                        return true;
                    }
                    else
                    {
                        return false;
                    }

                //case enMode.Update:

                //    return _UpdateOrderDetails();

            }

            return false;
        }

    }
}
