using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AccountContext
{
    public class UserPicture
    {
        [Key]
        public long PictureID { get; set; }
        [Required, MaxLength(60), DataType(DataType.ImageUrl)]
        public string FileName { get; set; }
        [MaxLength(60), DataType(DataType.ImageUrl)]
        public string OriginalFileName { get; set; }
        [Required]
        public bool ProfilePicture { get; set; }
        [Required]
        public DateTime UploadDate { get; set; }
        public virtual User User { get; set; }
    }
}
