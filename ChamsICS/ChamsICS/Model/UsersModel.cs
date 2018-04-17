using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace ChamsICSWebService.Model
{
    public class UserLoginRes : Response
    {
        public int UserId { get; set; }
        public string Mobile { get; set; }
        public string Email { get; set; }
        public int RoleId { get; set; }
        public string RoleCode { get; set; }
        public int PasswordStatus { get; set; }
        public int AccountStatus { get; set; }
        public  bool CanCreateWebUsers { get; set; }
        public User User { get; set; }
        public UserDashBoardData UserDashBoardData { get; set;}
         
    }

    public class UserDetailModel
    {
        public int userID { get; set; }
        public string Firstname { get; set; }
        public string Middlename { get; set; }
        public string Lastname { get; set; }
        public string Name { get; set; }
        public int? Sex { get; set; }
        public string Address { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }
        public string Website { get; set; }
        public int? TerminalId { get; set; }
        public string TerinalCode { get; set;}
        public int? Zoneid { get; set; }
        public string ZoneCode { get; set; }
        public string RegistrationNumber { get; set; }
    }

    public class UserDashBoardData
    {
        public int ClientId { get; set; }
        public string RoleName { get; set; }
        public string ClientName { get; set; }
        public string ClientLogoUrl { get; set; }
        public int UserTypeParentId { get; set; }

    }

    public class UserLoginReq
    {
        public string Email { get; set; }
        public string UserPassword { get; set; }
    }

    public class User
    {
        public int UserId { get; set; }
        public int UserTypeParentId { get; set; }
        public string Mobile { get; set; }
        public string Email { get; set; }
        public int? RoleId { get; set; }
        public string RoleCode { get; set; }
        public string Password { get; set; }
        public int? PasswordStatus { get; set; }
        public int? Status { get; set; }
        public int? ClientId { get; set; }
        public UserDetailModel userDetail { get; set; }
        public AuditTrailData AuditTrailData { get; set; }
    }

    public class FindUserRes : Response
    {
        public User User { get; set; }
        public int? ZoneId { get; set; }
        public string ZoneName { get; set; }
    }

    public class GetAllUserReq
    {
       public  int roleId { get; set; } 
       public  int UserTypeParentId { get; set; } 
    }

    public class GetAllUserRes:Response
    {
        public IEnumerable<User> Users { get; set; }
    }

    public class ChangeUserPasswordReq
    {
        public string Email { get; set; }
        public string OldPassword { get; set; }
        public string NewPassword { get; set; }

        public AuditTrailData AuditTrailData { get; set; }
    }

    public class ResetUserPasswordReq
    {
        public string Email { get; set; }
        public int Id { get; set; }

        public AuditTrailData AuditTrailData { get; set; }
    }

    public class RoleRes: Response
    {
        public IEnumerable<Role> Role { get; set; }
    }

    public class FindRoleRes:Response
    {
        public int RoleId { get; set; }
        public string Description { get; set; }
        public string RoleCode { get; set; }
    }

    public class Role
    {
        public int RoleId { get; set; }
        public string Description { get; set; }
        public string RoleCode { get; set; }
    }

    public class CreateUserRes: Response
    {
        public int userId { get; set; }
    }
}
