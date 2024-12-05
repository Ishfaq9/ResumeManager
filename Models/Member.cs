using System.ComponentModel.DataAnnotations;

namespace ResumeManager.Models
{
    public class Member
    {
        public int Id { get; set; }
        [StringLength(21), Required]
        public string PhoneNumber { get; set; }
        [StringLength(200)]
        public string Name { get; set; }
        public DateTime InsertedDate { get; set; } = DateTime.Now;
        public int GroupId { get; set; }
        public virtual Group group { get; set; }

    }
}
