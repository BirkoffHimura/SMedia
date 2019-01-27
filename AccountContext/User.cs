using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AccountContext
{
    public class User
    {
        public User()
        {
            UserPictures = new List<UserPicture>();
            UserPosts = new List<UserPost>();
            UserPictures = new List<UserPicture>();
            UserMessages = new List<UserMessage>();
        }
        [Key]
        public long ID { get; set; }
        [MaxLength(50),Required]
        public string Name { get; set; }
        [EmailAddress, MaxLength(256), DisplayName("User Name"), Required, Index(IsUnique = true)]        
        public string UserName { get; set; }
        [MaxLength(25), Required]
        public string Password { get; set; }
        [MaxLength(50), DisplayName("Address Line 1")]
        public string AddressLine1 { get; set; }
        [MaxLength(50), DisplayName("Address Line 2")]
        public string AddressLine2 { get; set; }
        [MaxLength(55)]
        public string Country { get; set; }
        public string City { get; set; }
        [MaxLength(2)]
        public string State { get; set; }
        [MaxLength(5)]
        public string ZipCode { get; set; }
        [DisplayName("Birth Date")]
        public DateTime BirthDate { get; set; }
        public DateTime SignupDate { get; set; }
        [DisplayName("Small Bio")]
        public string SmallBio { get; set; }
        public List<UserPicture> UserPictures { get; set; }
        public List<UserPost> UserPosts { get; set; } 
        public List<UserMessage> UserMessages { get; set; }
    }
}
