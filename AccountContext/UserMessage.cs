using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AccountContext
{
    public class UserMessage
    {
        [Key]
        public long UserMessageID { get; set; }
        [Required, MaxLength(500)]
        public string MessageBody { get; set; }
        public long MessageThreadID { get; set; }
        public MessageThread MessageThread { get; set; }
        public long UserID { get; set; }
        public User User { get; set; }
    }
}
