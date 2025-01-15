using EcommerceDataAccessLayer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static EcommerceBusinessLayer.clsUser;

namespace EcommerceBusinessLayer
{
    public class clsProduct
    {
        public enum enMode { AddNew = 0, Update = 1 };
        public enMode Mode = enMode.AddNew;


        public ProductDTO productDTO
        {
            get
            {
                return (new ProductDTO(this.ProductID, this.Name, this.Description, this.Price,this.StockQauntity));
            }
        }

     
        public int ProductID { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }

        public decimal Price { get; set; }

        public int StockQauntity { get; set; }



        public clsProduct(ProductDTO productDTO,enMode cMode=enMode.AddNew)
        {
            this.ProductID = productDTO.ProductID;
            this.Name = productDTO.Name;
            this.Description = productDTO.Description;
            this.Price = productDTO.Price;
            this.StockQauntity =productDTO.StockQauntity;
            Mode = cMode;
            
        }



        private bool _AddNewProduct()
        {
            //call DataAccess Layer 

            this.ProductID = clsProductDataAccess.AddNewProduct(productDTO);

            return (this.ProductID != -1);
        }

        private bool _UpdateProduct()
        {

            return clsProductDataAccess.UpdateProduct(productDTO);
        }



        public static clsProduct FindProduct(int ID)
        {

            ProductDTO productDTO = clsProductDataAccess.GetInfoProductById(ID);

            if (productDTO != null)
            {

                return new clsProduct(productDTO, enMode.Update);
            }
            else
                return null;
        }



        public bool Save()
        {
            switch (Mode)
            {
                case enMode.AddNew:
                    if (_AddNewProduct())
                    {

                        Mode = enMode.Update;
                        return true;
                    }
                    else
                    {
                        return false;
                    }

                case enMode.Update:

                    return _UpdateProduct();

            }

            return false;
        }


        public static bool DeleteProduct(int ProductID)
        {

            return clsProductDataAccess.DeleteProduct(ProductID);
        }

        public static List<ProductDTO> GetAllProducts()
        {
            return clsProductDataAccess.GetAllProducts();
        }
    }
}
