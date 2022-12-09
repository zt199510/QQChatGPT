using ChatGPTAI.ChatGPT;
using ChatGPTAI.MirAIModules;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Mirai.Net.Data.Messages.Receivers;
using Mirai.Net.Data.Messages;
using Mirai.Net.Data.Sessions;
using Mirai.Net.Sessions;
using Mirai.Net.Utils.Scaffolds;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Websocket.Client.Logging;
using Mirai.Net.Data.Events.Concretes.Message;
using Mirai.Net.Data.Messages.Concretes;
using Mirai.Net.Sessions.Http.Managers;

namespace ChatGPTAI
{
    /// <summary>
    ///********************************************
    /// 创建人        ：  zt
    /// 创建时间    ：  2022/12/8 10:35:07 
    /// Description   ：  
    ///********************************************/
    /// </summary>
    public class MiraiManager : BackgroundService
    {
        private IConfiguration _config;
        private ILogger<MiraiManager> _logger;
        private ChatGPTClient _gpt;

        ConfigData data = new ConfigData();
        public MiraiManager(ChatGPTClient gpt, IConfiguration config, ILogger<MiraiManager> logger)
        {
            _config = config;
            _logger = logger;
            _gpt = gpt;

            data.OAIUsername = config.GetValue<string>(nameof(data.OAIUsername));
            data.OAIPassword = config.GetValue<string>(nameof(data.OAIPassword));
            data.AccessToken = config.GetValue<string>(nameof(data.AccessToken));
            data.MirAIIp = config.GetValue<string>(nameof(data.MirAIIp));
            data.MirAIPort = config.GetValue<int>(nameof(data.MirAIPort));
            data.VerifyKey = config.GetValue<string>(nameof(data.VerifyKey));
            data.QQ = config.GetValue<float>(nameof(data.QQ));
        }
        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (true)
            {
                if (string.IsNullOrEmpty(data.OAIUsername) || string.IsNullOrEmpty(data.OAIPassword))
                {
                    _logger.LogWarning("请在配置文件中填写要登录的OpenAI ChatGPT账号和密码");
                    return;
                }
                var loginResult = await _gpt.AuthLogin(data.OAIUsername, data.OAIPassword, (str, e) =>
                {
                    if (!string.IsNullOrEmpty(str))
                        _logger.LogWarning(str);
                    if (e != null)
                        _logger.LogWarning(e.Message);
                }, stoppingToken);

                if (!loginResult)
                {
                    _logger.LogWarning("登录失败，请重新登录");
                    continue;

                }

                var exit = new ManualResetEvent(false);

                var bot = new MiraiBot
                {
                    Address = new ConnectConfig
                    {
                        HttpAddress = new ConnectConfig.AdapterConfig(data.MirAIIp, data.MirAIPort.ToString()),
                        WebsocketAddress = new ConnectConfig.AdapterConfig(data.MirAIIp, data.MirAIPort.ToString())
                    },
                    VerifyKey = "48d7f9cdd",
                    QQ = "2335403232"
                };

                await bot.LaunchAsync();

                bot.MessageReceived.Subscribe(Execute);

                Console.WriteLine("启动Chat机器人");
                exit.WaitOne();
            }

        }

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
                    return;
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
            if (@base is GroupMessageReceiver Group)
            {

                if (Group.MessageChain.OfType<AtMessage>().Any(x => x.Target == "2335403232"))
                {
                    userid = long.Parse(Group.Sender.Id);
                    Msg = Group.MessageChain.GetPlainMessage();
                    if (!_gpt.GetUserCompleted(userid))
                    {
                        ///  await Group.SendMessageAsync("您的上条请求尚未完成，请稍后");
                        return;
                    }
                    _logger.LogInformation("收到ChatGPT请求：{message}", Msg);
                    if (string.IsNullOrEmpty(Msg)) return;
                    var match = Regex.Match(Msg, "^\\/chat\\s+(.+)");
                    if (!match.Success)
                        Msg = "/chat " + Msg;
                    _ = _gpt.RequestConversation(userid, Msg, async (str, exp) =>
                    {
                        _logger.LogInformation("回复：{message}, {str}", Msg, str);

                        MessageChainBuilder messages = new MessageChainBuilder();
                        messages.At(Group.Sender.Id);
                        if (exp != null)
                        {
                            var loginResult = await _gpt.AuthLogin(data.OAIUsername, data.OAIPassword, (str, e) =>
                            {
                                if (!string.IsNullOrEmpty(str))
                                    _logger.LogWarning(str);
                                if (e != null)
                                    _logger.LogWarning(e.Message);
                            },new CancellationToken());

                        }
                        messages.Plain(exp == null ? str : exp.Message);
                        try
                        {

                            await Group.SendMessageAsync(messages.Build());
                        }
                        catch (Exception)
                        {


                        }

                        //  await Group.SendMessageAsync(exp == null ? str : exp.Message);
                    }, CancellationToken.None);
                }



                //userid = long.Parse(Group.MessageChain);
                //Msg = Frien.MessageChain.GetPlainMessage();
            }

        }
    }
}
