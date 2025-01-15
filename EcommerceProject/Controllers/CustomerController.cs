using EcommerceDataAccessLayer;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace EcommerceProject.Controllers
{
    [Route("api/Customer")]
    [ApiController]
    public class CustomerController : ControllerBase
    {

        [HttpPost("AddNewCustomer", Name = "AddNewCustomer")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public ActionResult<CustomerDTO> AddNewCustomer(CustomerDTO customerDTO)
        {
            //we validate the data here
            if (customerDTO == null || string.IsNullOrEmpty(customerDTO.FullName) )
            {
                return BadRequest("Invalid student data.");
            }
            EcommerceBusinessLayer.clsCustomer newCustomer = new EcommerceBusinessLayer.clsCustomer(new CustomerDTO(customerDTO.CustomerID, customerDTO.FullName, customerDTO.Email, customerDTO.Phone, customerDTO.City));

            newCustomer.Save();

            customerDTO.CustomerID = newCustomer.CustomerID;

            //we dont return Ok here,we return createdAtRoute: this will be status code 201 created.
            return CreatedAtRoute("GetCustomerById", new { id = customerDTO.CustomerID }, customerDTO);

        }


        [HttpGet("{id}", Name = "GetCustomerById")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public ActionResult<CustomerDTO> GetCustomerById(int id)
        {

            if (id < 1)
            {
                return BadRequest($"Not accepted ID {id}");
            }

            EcommerceBusinessLayer.clsCustomer customer = EcommerceBusinessLayer.clsCustomer.FindCustomer(id);

            if (customer == null)
            {
                return NotFound($"Student with ID {id} not found.");
            }

            //here we get only the DTO object to send it back.
            CustomerDTO CsDTO = customer.customerDTO;

            //we return the DTO not the student object.
            return Ok(CsDTO);

        }




        //here we use http put method for update
        [HttpPut("{id}", Name = "UpdateCustomer")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public ActionResult<CustomerDTO> UpdateCustomer(int id, CustomerDTO updatedCustomer)
        {
            if (id < 1 || updatedCustomer == null || string.IsNullOrEmpty(updatedCustomer.FullName) || string.IsNullOrEmpty(updatedCustomer.Email) || string.IsNullOrEmpty(updatedCustomer.Phone))
            {
                return BadRequest("Invalid student data.");
            }

            EcommerceBusinessLayer.clsCustomer customer = EcommerceBusinessLayer.clsCustomer.FindCustomer(id);


            if (customer == null)
            {
                return NotFound($"Student with ID {id} not found.");
            }


            customer.FullName = updatedCustomer.FullName;
            customer.Email = updatedCustomer.Email;
            customer.Phone = updatedCustomer.Phone;
            customer.City = updatedCustomer.City;
         



            customer.Save();

            //we return the DTO not the full student object.
            return Ok(customer.customerDTO);

        }







        //here we use HttpDelete method
        [HttpDelete("{id}", Name = "DeleteCustomer")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public ActionResult DeleteCustomer(int id)
        {
            if (id < 1)
            {
                return BadRequest($"Not accepted ID {id}");
            }


            if (EcommerceBusinessLayer.clsCustomer.DeleteCustomer(id))
                return Ok($"User with ID {id} has been deleted.");
           
            else
                return NotFound($"User with ID {id} not found. no rows deleted!");
        }

    }
}
