using System.ComponentModel.DataAnnotations;
using System.Runtime.InteropServices;
using System.ComponentModel;
namespace SampleAPI.Entities
{
    public class Order
    {
        public int Id { get; set; }
        [Required]
        public DateTime Date { get; set; }
        [Required]
        [MaxLength(100)]
        public string Description { get; set; }
        [MaxLength(100)]
        [Required]
        public string Name { get; set; }
        [DefaultValue(true)]
        public bool IsInvoiced { get; set; }
        [DefaultValue(false)]
        public bool IsDeleted { get; set; }
    }
}