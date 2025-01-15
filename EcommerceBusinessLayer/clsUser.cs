using EcommerceDataAccessLayer;

namespace EcommerceBusinessLayer
{
    public class clsUser
    {
        public enum enMode { AddNew = 0, Update = 1 };
        public enMode Mode = enMode.AddNew;

        public UserDTO userDTO
        {
            get { return (new UserDTO(this.ID, this.UserName,this.PasswordHash,this.FullName,
                this.Email,this.Phone,this.Role)); }
        }

     

        public int ID { get; set; }
        public string UserName { get; set; }
        public string PasswordHash { get; set; }
        public string FullName { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }
        public string Role { get; set; }

        public clsUser(UserDTO UsDTO, enMode cMode = enMode.AddNew)

        {
            this.ID = UsDTO.UserID;
            this.UserName = UsDTO.UserName;
            this.FullName = UsDTO.FullName;
            this.Phone = UsDTO.Phone;
            this.Email = UsDTO.Email;
            this.Role = UsDTO.Role;
            this.PasswordHash = UsDTO.PasswordHash;
          

         

            Mode = cMode;
        }






        private bool _AddNewStudent()
        {
            //call DataAccess Layer 

            this.ID = clsUserDataAccess.AddNewUser(userDTO);

            return (this.ID != -1);
        }

        private bool _UpdateStudent()
        {
            return clsUserDataAccess.UpdateUser(userDTO);
        }


        public static List<UserDTO> GetAllUsers()
        {
            return EcommerceDataAccessLayer.clsUserDataAccess.GetAllUsernameAndUseridOfUsers();
        }

        public static List<UserDTO> GetAllAdminsUsernameAndUseridUsers()
        {
            return EcommerceDataAccessLayer.clsUserDataAccess.GetAllAdminsUsers();
        }



        public static clsUser FindUser(int ID)
        {

            UserDTO userDTO = clsUserDataAccess.GetInfoUserById(ID);

            if (userDTO != null)
            {

                return new clsUser(userDTO, enMode.Update);
            }
            else
                return null;
        }


        public static bool DeleteUser(int UserID)
        {

            return clsUserDataAccess.DeleteUser(UserID);
        }

        public bool Save()
        {
            switch (Mode)
            {
                case enMode.AddNew:
                    if (_AddNewStudent())
                    {

                        Mode = enMode.Update;
                        return true;
                    }
                    else
                    {
                        return false;
                    }

                case enMode.Update:

                    return _UpdateStudent();
                    
            }

            return false;
        }

       

    }
    }

