using ChatGPTAI.ChatGPT;
using ChatGPTAI.ChatGPT.Data;
using Manganese.Text;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Mirai.Net.Data.Messages;
using Mirai.Net.Data.Messages.Concretes;
using Mirai.Net.Data.Messages.Receivers;
using Mirai.Net.Modules;
using Mirai.Net.Utils.Scaffolds;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.WebSockets;
using System.Text;
using System.Text.Json;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace ChatGPTAI.MirAIModules
{
    /// <summary>
    ///********************************************
    /// 创建人        ：  ZT
    /// 创建时间    ：  2022/12/8 10:50:32 
    /// Description   ：  
    ///********************************************/
    /// </summary>
    public class BateModules : IModule
    {
        private  ChatGPTClient _gpt;
        private  ILogger<MiraiManager> _logger;

        public BateModules(ChatGPTClient gpt, ILogger<MiraiManager> logger)
        {
            _gpt = gpt;
            _logger = logger;
        }


        public BateModules()
        {
         
        }
        //public void SetGPT(ChatGPTClient gpt, ILogger<MiraiManager> logger) {

        //    _gpt = gpt;
        //    _logger = logger;
        //} 


        public bool? IsEnable { get; set; }

        public async void Execute(MessageReceiverBase @base)
        {
            long userid = 0;
            string Msg = "";
            if (@base is FriendMessageReceiver Frien)
            {
                userid = long.Parse(Frien.FriendId);
                Msg = Frien.MessageChain.GetPlainMessage();
                if (!_gpt.GetUserCompleted(userid))
                {
                    await Frien.SendMessageAsync("您的上条请求尚未完成，请稍后");
                }
                _logger.LogInformation("收到ChatGPT请求：{message}", Msg);
                if (string.IsNullOrEmpty(Msg)) return;
                var match = Regex.Match(Msg, "^\\/chat\\s+(.+)");
                if (!match.Success)
                    Msg = "/chat " + Msg;
                _ = _gpt.RequestConversation(userid, Msg, async (str, exp) =>
                {
                    _logger.LogInformation("回复：{message}, {str}", Msg, str);
                    await Frien.SendMessageAsync(exp == null ? str : exp.Message);
                }, CancellationToken.None);
            }
          
        }




       
    }
}
