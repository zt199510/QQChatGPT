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
    /// 创建人        ：  zt
    /// 创建时间    ：  2022/12/8 10:27:04 
    /// Description   ：  
    ///********************************************/
    /// </summary>
    public class ConversationRequest
    {
        [JsonPropertyName("action")]
        public string Action { get; set; }
        [JsonPropertyName("messages")]
        public List<Message> Message { get; set; }
        [JsonPropertyName("parent_message_id")]
        public string ParentMessageId { get; set; }
        [JsonPropertyName("model")]
        public string Model { get; set; }
        [JsonPropertyName("conversation_id")]
        public string? ConversationId { get; set; }
    }
}
