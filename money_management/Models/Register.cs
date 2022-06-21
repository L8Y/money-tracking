namespace money_management.Models
{
    public class Register
    {
        internal int count;

        public string Name { get; set; }    
            public string Email { get; set; }   
        public string Password { get; set; }  
        [System.ComponentModel.DataAnnotations.Key]
        public int UserId { get; set; } 
    }
}
