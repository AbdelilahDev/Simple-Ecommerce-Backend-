using EcommerceBusinessLayer;
using EcommerceDataAccessLayer;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace EcommerceProject.Controllers
{
    [Route("api/Order")]
    [ApiController]
    public class OrderController : ControllerBase
    {


        [HttpPost("AddNewOrder", Name = "AddNewOrder")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public ActionResult<OrderDTO> AddNewOrder(OrderDTO orderDTO)
        {
            
           // Check If Customer is exist

            if (!clsCustomer.IsCustomerexist(orderDTO.CustomerID))
            {
                return BadRequest("this Customer not Found.");
            }

          //  we validate the data here
            if (orderDTO == null || string.IsNullOrEmpty(orderDTO.ShippingAddress) )
            {
                return BadRequest("Invalid student data.");
            }

            EcommerceBusinessLayer.clsOrder newOrder = new EcommerceBusinessLayer.clsOrder(new OrderDTO(orderDTO.OrderID, orderDTO.CustomerID, orderDTO.ShippingAddress, orderDTO.OrderStutas));

            newOrder.Save();

            orderDTO.OrderID = newOrder.OrderID;

            //we dont return Ok here,we return createdAtRoute: this will be status code 201 created.
            return CreatedAtRoute("GetOrderById", new { id = orderDTO.OrderID }, orderDTO);

        }




        [HttpGet("{id}", Name = "GetOrderById")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public ActionResult<OrderDTO> GetOrderById(int id)
        {
            if (id < 1)
            {
                return BadRequest($"Not accepted ID {id}");
            }


            EcommerceBusinessLayer.clsOrder order = EcommerceBusinessLayer.clsOrder.FindOrder(id);

            if (order == null)
            {
                return NotFound($"Student with ID {id} not found.");
            }

            //here we get only the DTO object to send it back.
            OrderDTO orderDTO = order.orderDTO;

            //we return the DTO not the student object.
            return Ok(orderDTO);

        }




        [HttpGet("AllOrders", Name = "GetAllOrders")] // Marks this method to respond to HTTP GET requests.
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]

        //here we used StudentDTO
        public ActionResult<IEnumerable<OrderDTO>> GetAllOrders() // Define a method to get all students.
        {
            
            List<OrderDTO> UsersList = EcommerceBusinessLayer.clsOrder.GetAllOrders();
            if (UsersList.Count == 0)
            {
                return NotFound("No User Found!");
            }
            return Ok(UsersList); // Returns the list of students.

        }







    }
}
