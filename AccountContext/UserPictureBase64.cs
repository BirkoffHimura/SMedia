using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AccountContext
{
    public class UserPictureBase64
    {        
        public long PictureID { get; set; }
        public string FileName { get; set; }
        public string OriginalFileName { get; set; }
        public bool ProfilePicture { get; set; }
        public DateTime UploadDate { get; set; }
        public string Base64ImageString { get; set; }
        public virtual User User { get; set; }         
    }
}
