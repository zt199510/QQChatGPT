using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChatGPTAI.ChatGPT.Data
{
    /// <summary>
    ///********************************************
    /// 创建人        ：  zt
    /// 创建时间    ：  2022/12/8 10:27:15 
    /// Description   ：  
    ///********************************************/
    /// </summary>
    public class ConversationStatus
    {
        public string? ConversationId { get; set; }
        public string? ParentMessageId { get; set; }
        public bool IsCompleted { get; set; }
    }
}
