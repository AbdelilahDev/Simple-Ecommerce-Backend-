using EcommerceBusinessLayer;
using EcommerceDataAccessLayer;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;

namespace EcommerceProject.Controllers
{
    [Route("api/Product")]
    [ApiController]
    public class ProductController : ControllerBase
    {

        [HttpPost("AddNewProduct", Name = "AddNewProduct")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public ActionResult<ProductDTO> AddNewProduct(ProductDTO productDTO)
        {
            //we validate the data here
            if (productDTO == null || string.IsNullOrEmpty(productDTO.Name) || productDTO.Price<0 )
            {
                return BadRequest("Invalid student data.");
            }

            EcommerceBusinessLayer.clsProduct newProduct = new EcommerceBusinessLayer.clsProduct(new ProductDTO(productDTO.ProductID,productDTO.Name,productDTO.Description,productDTO.Price,productDTO.StockQauntity));

            newProduct.Save();

            productDTO.ProductID = newProduct.ProductID;

            //we dont return Ok here,we return createdAtRoute: this will be status code 201 created.
            return CreatedAtRoute("GetProductById", new { id = productDTO.ProductID}, productDTO);

        }




        [HttpGet("{id}", Name = "GetProductById")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]

        public ActionResult<ProductDTO> GetProductById(int id)
        {

            if (id < 1)
            {
                return BadRequest($"Not accepted ID {id}");
            }

            EcommerceBusinessLayer.clsProduct product = EcommerceBusinessLayer.clsProduct.FindProduct(id);

            if (product == null)
            {
                return NotFound($"Student with ID {id} not found.");
            }

            //here we get only the DTO object to send it back.
            ProductDTO PrDTO = product.productDTO;

            //we return the DTO not the student object.
            return Ok(PrDTO);

        }






        //here we use http put method for update
        [HttpPut("{id}", Name = "UpdateProduct")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public ActionResult<ProductDTO> UpdateProduct(int id, ProductDTO updatedProduct)
        {
            if (id < 1 || updatedProduct == null || string.IsNullOrEmpty(updatedProduct.Name) || updatedProduct.Price<0 || updatedProduct.StockQauntity<0)
            {
                return BadRequest("Invalid student data.");
            }


            EcommerceBusinessLayer.clsProduct product = EcommerceBusinessLayer.clsProduct.FindProduct(id);


            if (product == null)
            {
                return NotFound($"Student with ID {id} not found.");
            }


            product.Name= updatedProduct.Name;
            product.Description = updatedProduct.Description;
            product.Price = updatedProduct.Price;
            product.StockQauntity = updatedProduct.StockQauntity;
       
            product.Save();

            //we return the DTO not the full student object.
            return Ok(product.productDTO);

        }




        //here we use HttpDelete method
        [HttpDelete("{id}", Name = "DeleteProduct")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public ActionResult DeleteProduct(int id)
        {
            if (id < 1)
            {
                return BadRequest($"Not accepted ID {id}");
            }

            if (EcommerceBusinessLayer.clsProduct.DeleteProduct(id))

                return Ok($"User with ID {id} has been deleted.");
            else
                return NotFound($"User with ID {id} not found. no rows deleted!");
        }



        [HttpGet("GetAllProducts", Name = "AllProducts")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]

        public ActionResult<IEnumerable<ProductDTO>> AllProducts()
        {
            List<ProductDTO> AllProducts = clsProduct.GetAllProducts();

            if(AllProducts.Count==0)
            {
                return NotFound("Not Product Found");

            }
            return Ok(AllProducts);


        }





    }
}
