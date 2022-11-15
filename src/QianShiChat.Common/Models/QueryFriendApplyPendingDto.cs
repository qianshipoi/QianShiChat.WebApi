using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QianShiChat.Models
{
    public class QueryFriendApplyPendingDto
    {
        public int Size { get; set; }

        public long BeforeLastTime { get; set; }
    }
}
