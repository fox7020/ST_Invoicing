using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace ST_Invoicing.Models
{
    public class LoginVM : IValidatableObject
    {
        [DisplayFormat(ConvertEmptyStringToNull = false)]
        public string ReturnUrl { get; set; }
      
        [DisplayFormat(ConvertEmptyStringToNull = false)]
        [Required]
        public string Account { get; set; }
       
        [DisplayFormat(ConvertEmptyStringToNull = false)]
        [Required]
        public string Password { get; set; }

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
          
            ST_EmpDAO mST_EmpDAO = new ST_EmpDAO();

            if (mST_EmpDAO.isCurrentUser(Account, Password) == false) {
                yield return new ValidationResult("帳號或密碼錯誤", new string[] { "Account" });
            }    
        }
    }
}