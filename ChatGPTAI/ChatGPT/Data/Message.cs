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
    /// 创建时间    ：  2022/12/8 10:26:24 
    /// Description   ：  
    ///********************************************/
    /// </summary>
    public class Message
    {
        [JsonPropertyName("id")]
        public string Id { get; set; }
        [JsonPropertyName("role")]
        public string Role { get; set; }
        [JsonPropertyName("content")]
        public MessageContent Content { get; set; }
    }
}
