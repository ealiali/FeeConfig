using System.ComponentModel.DataAnnotations;

namespace VMTest
{
    public class FeeConfiguration
    {
        [Key]
        public int Id { get; set; }
        [Required] 
        
        public string Name { get; set; }
        [Required]
        public decimal FlatValue { get; set; }

    }
}