using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LegoNPU.Models
{
    public class Image
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public string BlobUrl { get; set; }
        public DateTime UploadedAt { get; set; }
        public Guid UserId { get; set; }
        public User User { get; set; }
        public ICollection<Rating> Ratings { get; set; }
    }
}
