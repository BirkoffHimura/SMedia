using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AccountContext
{
    public class UserPost
    {
        public UserPost()
        {
            UserPostComments = new List<UserPostComment>();
        }
        [Key]
        public long ID { get; set; }
        [MaxLength(60)]
        public string Subject { get; set; }
        [MaxLength(256), Required]
        public string Post_Body { get; set; }
        [DataType(DataType.ImageUrl)]
        public string Img_Extern { get; set; }
        [Required]
        public DateTime PostDate { get; set; }
        
        public UserPicture UserPicture { get; set; }
        [Required]
        public User User { get; set; }
        public List<UserPostComment> UserPostComments { get; set; }
    }
}
