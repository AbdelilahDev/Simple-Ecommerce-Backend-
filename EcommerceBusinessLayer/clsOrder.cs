using EcommerceDataAccessLayer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EcommerceBusinessLayer
{
    public class clsOrder
    {
        public enum enMode { AddNew = 0, Update = 1 };
        enMode Mode = enMode.AddNew;

        public int OrderID { get; set; }
        public int CostomerID { get; set; }
        public string ShippingAddress { get; set; }
        public string OrderStutas { get; set; }

        public OrderDTO orderDTO
        {
            get
            {
                return (new OrderDTO(this.OrderID, this.CostomerID, this.ShippingAddress, this.OrderStutas));
            }
        }

        public clsOrder(OrderDTO orderDTO, enMode cMode = enMode.AddNew)
        {
            this.OrderID = orderDTO.OrderID;
            this.CostomerID = orderDTO.CustomerID;
      
            this.ShippingAddress = orderDTO.ShippingAddress;
            this.OrderStutas = orderDTO.OrderStutas;
            Mode = cMode;

        }

        private bool _AddNewOrder()
        {
            //call DataAccess Layer 

            this.OrderID = clsOrdersDataAccess.AddNewOrder(orderDTO);

            return (this.OrderID != -1);
        }

        //private bool _UpdateOrder()
        //{
        //    return true;

        //    //return clsProductDataAccess.UpdateProduct(productDTO);
        //}


        public static bool IsOrderExist(int OrderID)
        {

            return clsOrdersDataAccess.IsOrderExist(OrderID);
        }


        public static clsOrder FindOrder(int ID)
        {

            OrderDTO orderDTO = clsOrdersDataAccess.GetInfoOrderById(ID);

            if (orderDTO != null)
            //we return new object of that student with the right data
            {

                return new clsOrder(orderDTO, enMode.Update);
            }
            else
                return null;
        }

        public static List<OrderDTO> GetAllOrders()
        {
            return clsOrdersDataAccess.GetAllOrders();
        }



        public bool Save()
        {
            switch (Mode)
            {
                case enMode.AddNew:
                    if (_AddNewOrder())
                    {

                        Mode = enMode.Update;
                        return true;
                    }
                    else
                    {
                        return false;
                    }

                //case enMode.Update:

                //    return _UpdateOrder();

            }

            return false;
        }




    }
}
