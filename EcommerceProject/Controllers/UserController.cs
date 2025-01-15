using EcommerceDataAccessLayer;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace EcommerceProject.Controllers
{
    [Route("api/User")]
    [ApiController]
    public class UserController : ControllerBase
    {

        [HttpGet("AllUsers", Name = "GetAllUsers")] // Marks this method to respond to HTTP GET requests.
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]

        public ActionResult<IEnumerable<UserDTO>> GetAllUsers() 
        {
            List<UserDTO> UsersList = EcommerceBusinessLayer.clsUser.GetAllUsers();
            if (UsersList.Count == 0)
            {
                return NotFound("No User Found!");
            }
            return Ok(UsersList); 

        }



        [HttpGet("AllAdminsUsers", Name = "GetAllAdminUsers")] // Marks this method to respond to HTTP GET requests.
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]

        public ActionResult<IEnumerable<UserDTO>> GetAllAdminUsers() 
        {
          
            List<UserDTO> AdminiUsersList = EcommerceBusinessLayer.clsUser.GetAllAdminsUsernameAndUseridUsers();
            if (AdminiUsersList.Count == 0)
            {
                return NotFound("No User Found!");
            }
            return Ok(AdminiUsersList); 

        }





        [HttpGet("{id}", Name = "GetUserById")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]

        public ActionResult<UserDTO> GetUserById(int id)
        {

            if (id < 1)
            {
                return BadRequest($"Not accepted ID {id}");
            }

            EcommerceBusinessLayer.clsUser user = EcommerceBusinessLayer.clsUser.FindUser(id);

            if (user == null)
            {
                return NotFound($"Student with ID {id} not found.");
            }

            //here we get only the DTO object to send it back.
            UserDTO UsDTO =user.userDTO;

            //we return the DTO not the  object.
            return Ok(UsDTO);

        }






        [HttpPost("AddNewUser",Name = "AddUser")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public ActionResult<UserDTO> AddNewUser(UserDTO newUserDTO)
        {
            //we validate the data here
            if (newUserDTO == null || string.IsNullOrEmpty(newUserDTO.UserName) || string.IsNullOrEmpty(newUserDTO.FullName) || string.IsNullOrEmpty(newUserDTO.PasswordHash))
            {
                return BadRequest("Invalid student data.");
            }



            EcommerceBusinessLayer.clsUser NewUser = new EcommerceBusinessLayer.clsUser(new UserDTO(newUserDTO.UserID, newUserDTO.UserName,newUserDTO.PasswordHash,
                newUserDTO.FullName,newUserDTO.Email,newUserDTO.Phone,newUserDTO.Role));



            NewUser.Save();

            newUserDTO.UserID = NewUser.ID;

            //we return the DTO only not the full student object
            //we dont return Ok here,we return createdAtRoute: this will be status code 201 created.
            return CreatedAtRoute("GetUserById", new { id = newUserDTO.UserID }, newUserDTO);

        }





        //here we use http put method for update
        [HttpPut("{id}", Name = "UpdateUser")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public ActionResult<UserDTO> UpdateUser(int id, UserDTO updatedUser)
        {
            if (id < 1 || updatedUser == null || string.IsNullOrEmpty(updatedUser.UserName) || string.IsNullOrEmpty(updatedUser.FullName) || string.IsNullOrEmpty(updatedUser.PasswordHash))
            {
                return BadRequest("Invalid student data.");
            }

            
          EcommerceBusinessLayer.clsUser User = EcommerceBusinessLayer.clsUser.FindUser(id);


            if (User == null)
            {
                return NotFound($"Student with ID {id} not found.");
            }


            User.UserName = updatedUser.UserName;
            User.PasswordHash = updatedUser.PasswordHash;
            User.FullName = updatedUser.FullName;
            User.Email = updatedUser.Email;
            User.Phone = updatedUser.Phone;
            User.Role = updatedUser.Role;



            User.Save();

            //we return the DTO not the full student object.
            return Ok(User.userDTO);

        }







        //here we use HttpDelete method
        [HttpDelete("{id}", Name = "DeleteUser")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public ActionResult DeleteUser(int id)
        {
            if (id < 1)
            {
                return BadRequest($"Not accepted ID {id}");
            }

            if (EcommerceBusinessLayer.clsUser.DeleteUser(id))

                return Ok($"User with ID {id} has been deleted.");
            else
                return NotFound($"User with ID {id} not found. no rows deleted!");
        }



















    }
}

