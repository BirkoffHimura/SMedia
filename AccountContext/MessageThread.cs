using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AccountContext
{
    public class MessageThread
    {
        [Key]
        public long MessageThreadID { get; set; }
        [MaxLength(41), Required]
        [Index("IX_UserCombination", 2, IsUnique = true)]
        public string UserCombination { get; set; }
        public virtual List<UserMessage> UserMessages { get; set; }
    }
}
