using EcommerceDataAccessLayer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EcommerceBusinessLayer
{
    public class clsCustomer
    {
        public enum enMode { AddNew=0,Update=1};
        public enMode Mode = enMode.AddNew; 

        public CustomerDTO customerDTO
        {
            get
            {
                return (new CustomerDTO(this.CustomerID, this.FullName, this.Email, this.Phone, this.City));
            }
        }


        public clsCustomer(CustomerDTO customerDTO,enMode cMode=enMode.AddNew)
        {
            this.CustomerID = customerDTO.CustomerID;
            this.FullName = customerDTO.FullName;
            this.Email = customerDTO.Email;
            this.Phone = customerDTO.Phone;
            this.City = customerDTO.City;

            Mode = cMode;

            
        }
        public int CustomerID { get; set; }
        public string FullName { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }
        public string City { get; set; }


        private bool _AddNewCustomer()
        {
            //call DataAccess Layer 

            this.CustomerID = clsCustomersDataAccess.AddNewCostumer(customerDTO);

            return (this.CustomerID != -1);
        }


        private bool _UpdateCustomer()
        {

            return clsCustomersDataAccess.UpdateCustomer(customerDTO);
        }


        public static clsCustomer FindCustomer(int ID)
        {

            CustomerDTO customerDTO = clsCustomersDataAccess.GetInfoCustomerById(ID);

            if (customerDTO != null)
            {

                return new clsCustomer(customerDTO, enMode.Update);
            }
            else
                return null;
        }

        public static bool IsCustomerexist(int ID)
        {

            return clsCustomersDataAccess.IsCustomerExist(ID);
        }



        public bool Save()
        {
            switch (Mode)
            {
                case enMode.AddNew:
                    if (_AddNewCustomer())
                    {

                        Mode = enMode.Update;
                        return true;
                    }
                    else
                    {
                        return false;
                    }

                case enMode.Update:

                    return _UpdateCustomer();

            }

            return false;
        }


        public static bool DeleteCustomer(int CustomerID)
        {

            return clsCustomersDataAccess.DeleteCustomer(CustomerID);
        }





    }
}
