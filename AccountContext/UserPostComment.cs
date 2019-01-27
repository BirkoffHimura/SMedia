using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AccountContext
{
    public class UserPostComment
    {
        [Key]
        public long PostCommentID { get; set; }
        [MaxLength(256), Required]
        public string Comment_Body { get; set; }
        [Required]
        public DateTime CommentDate { get; set; }
        [Required]
        public User FromUser { get; set; }
        [Required]
        public UserPost UserPost { get; set; }
    }
}
