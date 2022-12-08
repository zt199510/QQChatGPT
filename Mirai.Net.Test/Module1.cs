using System;
using System.Linq;
using Mirai.Net.Data.Messages;
using Mirai.Net.Data.Messages.Concretes;
using Mirai.Net.Data.Messages.Receivers;
using Mirai.Net.Modules;
using Mirai.Net.Sessions;
using Mirai.Net.Utils.Scaffolds;

namespace Mirai.Net.Test
{
    public class Module1 : IModule
    {
        public async void Execute(MessageReceiverBase @base)
        {
            
          var a=  @base.Concretize<FriendMessageReceiver>();

            if(@base is GroupMessageReceiver receiver)
            {
                if (receiver.Sender.Id != "2933170747")
                {
                    return;
                }
                var plain = receiver.MessageChain.GetPlainMessage();
                if (plain == "/off")
                {
                    IsEnable = false;
                    await receiver.SendMessageAsync("Current module will be turned off");
                    return;
                }
            }
          
            
            await a.SendMessageAsync("123");
        }

        public bool? IsEnable { get; set; }
    }
}