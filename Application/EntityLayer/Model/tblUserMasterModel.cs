using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EntityLayer
{
    [MetadataType(typeof(tblUserMasterModel))]
    public partial class tblUserMaster {
       
    }

    public class tblUserMasterModel
    {
        [Display(Name = "Email id")]
        [Required(ErrorMessage = "This field is required.")]
        [RegularExpression("^[a-z0-9_\\+-]+(\\.[a-z0-9_\\+-]+)*@[a-z0-9-]+(\\.[a-z0-9]+)*\\.([a-z]{2,4})$", ErrorMessage = "Invalid email id entered.")]
        public string EmailId { get; set; }

        [Display(Name = "Mobile no")]
        [Required(ErrorMessage = "This field is required.")]
        public string MobileNo { get; set; }

        [Display(Name = "Firstname")]
        [Required(ErrorMessage = "This field is required.")]
        [RegularExpression("^([a-zA-Z']+)$", ErrorMessage = "Please enter alphabets only.")]
        public string FirstName { get; set; }

        [Display(Name = "Lastname")]
        [Required(ErrorMessage = "This field is required.")]
        [RegularExpression("^([a-zA-Z']+)$", ErrorMessage = "Please enter alphabets only.")]
        public string LastName { get; set; }

        [Required]
        [StringLength(100, ErrorMessage = "The {0} must be at least {2} characters long.", MinimumLength = 6)]
        [DataType(DataType.Password)]
        [Display(Name = "Password")]
        public string Password { get; set; }
    }

    public class tblUserFilterModel
    {
        [Display(Name = "Firstname")]
        public string FirstName { get; set; }

        [Display(Name = "Lastname")]
        public string LastName { get; set; }
    }

    public class UserPageModel
    {
        public tblUserFilterModel Filter { get; set; }
        public List<tblUserMaster> List { get; set; }
    }
}
