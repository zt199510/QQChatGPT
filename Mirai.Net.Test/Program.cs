using System;
using System.Linq;
using System.Net;
using System.Reactive.Linq;
using System.Threading;
using System.Threading.Tasks;
using Manganese.Array;
using Manganese.Text;
using Mirai.Net.Data.Events.Concretes.Request;
using Mirai.Net.Data.Messages;
using Mirai.Net.Data.Messages.Concretes;
using Mirai.Net.Data.Messages.Receivers;
using Mirai.Net.Data.Sessions;
using Mirai.Net.Sessions;
using Mirai.Net.Sessions.Http.Managers;
using Mirai.Net.Utils.Internal;
using Mirai.Net.Utils.Scaffolds;
using Newtonsoft.Json;

namespace Mirai.Net.Test
{
    internal static class Program
    {
        private static async Task Main()
        {
            var exit = new ManualResetEvent(false);
            
            var bot = new MiraiBot
            {
                Address = new ConnectConfig
                {
                    HttpAddress = new ConnectConfig.AdapterConfig("39.108.218.166", "8080"),
                    WebsocketAddress = new ConnectConfig.AdapterConfig("39.108.218.166", "8080")
                },
                VerifyKey = "48d7f9cdd",
                QQ = "2335403232"
            };
            
            await bot.LaunchAsync();
            
            var modules = new Module1().GetModules();
            bot.MessageReceived.WithModules(modules);
          
            Console.WriteLine("启动Chat机器人");
            exit.WaitOne();
        }
    }
}