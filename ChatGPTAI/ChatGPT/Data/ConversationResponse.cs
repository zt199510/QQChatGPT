using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace ChatGPTAI.ChatGPT.Data
{
    /// <summary>
    ///********************************************
    /// 创建人        ：  ZT
    /// 创建时间    ：  2022/12/8 10:27:36 
    /// Description   ：  
    ///********************************************/
    /// </summary>
    public class ConversationResponse
    {
        [JsonPropertyName("conversation_id")]
        public string? ConversationId { get; set; }
        [JsonPropertyName("error")]
        public string? Error { get; set; }
        [JsonPropertyName("message")]
        public Message Message { get; set; }
    }
}
