using System.ComponentModel.DataAnnotations.Schema;

namespace money_management.Models
{
    public class Bank_details
    { 
        public string Account_Name { get; set; }    
        public string Initial_Balance { get; set; }

        [System.ComponentModel.DataAnnotations.Key]
        public int User_BankId { get; set; }

        [ForeignKey("UserId")]
        public virtual Register? UserId { get; set; }

    }
}
