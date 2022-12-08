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
    /// 创建时间    ：  2022/12/8 10:26:53 
    /// Description   ：  
    ///********************************************/
    /// </summary>
    public class MessageContent
    {
        [JsonPropertyName("content_type")]
        public string ContentType { get; set; }
        [JsonPropertyName("parts")]
        public List<string> Parts { get; set; }
    }
}
