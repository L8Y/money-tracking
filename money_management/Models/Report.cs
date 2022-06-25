using System.ComponentModel.DataAnnotations.Schema;

namespace money_management.Models
{
    public class Report
    {
        [System.ComponentModel.DataAnnotations.Key]
        public int Report_No {get; set;}
        public DateTime Date  { get; set; }
        public string Reason { get; set; }  
        public int Amount { get; set; } 
        public int Balance { get; set; }    
        public byte Transaction_type { get; set; }
        [ForeignKey("UserId")]
        public virtual Register? UserId { get; set; }
        [ForeignKey("User_BankId")]
        public virtual Bank_details? User_BankId { get; set; }



    }

}
