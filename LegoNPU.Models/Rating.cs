using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LegoNPU.Models
{
    public class Rating
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid Id { get; set; }
        public int Score { get; set; }
        public Guid ImageId { get; set; }
        public Image Image { get; set; }
        public Guid UserId { get; set; }
        public User User { get; set; }
    }
}
