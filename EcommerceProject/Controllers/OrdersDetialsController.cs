using EcommerceBusinessLayer;
using EcommerceDataAccessLayer;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace EcommerceProject.Controllers
{
    [Route("api/OrdersDetials")]
    [ApiController]
    public class OrdersDetialsController : ControllerBase
    {


        [HttpPost("AddNewOrderDetials", Name = "AddNewOrderDetials")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public ActionResult<OrderDetailsDTO> AddNewOrderDetials(OrderDetailsDTO orderDetialsDTO)
        {


            // Check If Customer is exist

            if (!clsOrder.IsOrderExist(orderDetialsDTO.OrderID))
            {
                return BadRequest("this Order not Found.");
            }

            //  we validate the data here
            if (orderDetialsDTO == null)
            {
                return BadRequest("Invalid student data.");
            }
            EcommerceBusinessLayer.clsOrderDetails orderDetials = new EcommerceBusinessLayer.clsOrderDetails(new OrderDetailsDTO(orderDetialsDTO.OrderDetailID, orderDetialsDTO.OrderID, orderDetialsDTO.ProductID, orderDetialsDTO.Quantity, orderDetialsDTO.Price, orderDetialsDTO.Quantity * orderDetialsDTO.Price));

            orderDetials.Save();

            orderDetialsDTO.OrderDetailID = orderDetials.OrderDetailID;
            orderDetialsDTO.TotalAmount = orderDetials.TotalAmount;

           
            //we dont return Ok here,we return createdAtRoute: this will be status code 201 created.
            return CreatedAtRoute("GetOrdertialsById", new { id = orderDetialsDTO.OrderID }, orderDetialsDTO);

        }




        [HttpGet("{id}", Name = "GetOrdertialsById")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]

        public ActionResult<OrderDetailsDTO> GetOrdertialsById(int id)
        {

            if (id < 1)
            {
                return BadRequest($"Not accepted ID {id}");
            }


            EcommerceBusinessLayer.clsOrderDetails orderDtials = EcommerceBusinessLayer.clsOrderDetails.FindOrderDetials(id);

            if (orderDtials == null)
            {
                return NotFound($"Student with ID {id} not found.");
            }

            //here we get only the DTO object to send it back.
            OrderDetailsDTO orderDetialsDTO = orderDtials.orderDetailsDTO;

            //we return the DTO not the student object.
            return Ok(orderDetialsDTO);

        }




        [HttpGet("AllOrdersDetials", Name = "GetAllOrdersDetials")] // Marks this method to respond to HTTP GET requests.
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]

        public ActionResult<IEnumerable<OrderDTO>> GetAllOrdersDetials() 
        {

            List<OrderDetailsDTO> OrdersDetialsList = EcommerceBusinessLayer.clsOrderDetails.GetAllOrdersDetials();
            if (OrdersDetialsList.Count == 0)
            {
                return NotFound("No User Found!");
            }
            return Ok(OrdersDetialsList); 

        }


    }
}
