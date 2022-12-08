﻿using Manganese.Text;
using Mirai.Net.Data.Events.Concretes.Request;
using Mirai.Net.Data.Messages;
using Mirai.Net.Data.Messages.Concretes;
using Mirai.Net.Data.Messages.Receivers;
using Mirai.Net.Data.Sessions;
using Mirai.Net.Data.Shared;
using Mirai.Net.Modules;
using Mirai.Net.Sessions;
using Mirai.Net.Sessions.Http.Managers;
using Mirai.Net.Utils.Internal;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reactive.Linq;
using System.Threading.Tasks;

namespace Mirai.Net.Utils.Scaffolds;

/// <summary>
/// mirai相关拓展方法
/// </summary>
public static class MiraiScaffold
{
    #region MiraiBot extensions

    /// <summary>
    ///     拓展方法，获取mirai-api-http插件的版本，此方法不需要经过任何认证
    /// </summary>
    /// <returns></returns>
    public static async Task<string> GetPluginVersionAsync(this MiraiBot bot)
    {
        try
        {
            var response = await HttpEndpoints.About.GetAsync();

            response.EnsureSuccess(response);

            return response.Fetch("data.version");
        }
        catch (Exception e)
        {
            throw new Exception($"获取失败: {e.Message}\n{bot}");
        }
    }

    /// <summary>
    /// 判断某QQ号是否为bot账号的好友
    /// </summary>
    /// <param name="bot"></param>
    /// <param name="qq"></param>
    /// <returns></returns>
    public static bool IsFriend(this MiraiBot bot, string qq)
    {
        return bot.Friends.Value.Any(x => x.Id == qq);
    }

    /// <summary>
    /// 判断某群成员是否是bot账号的好友
    /// </summary>
    /// <param name="bot"></param>
    /// <param name="member"></param>
    /// <returns></returns>
    public static bool IsFriend(this MiraiBot bot, Member member)
    {
        return bot.Friends.Value.Any(x => x.Id == member.Id);
    }

    /// <summary>
    /// 判断某人是否是bot账号的好友
    /// </summary>
    /// <param name="bot"></param>
    /// <param name="friend"></param>
    /// <returns></returns>
    public static bool IsFriend(this MiraiBot bot, Friend friend)
    {
        return bot.Friends.Value.Any(x => x.Id == friend.Id);
    }

    /// <summary>
    /// An encapsulation of 'OfType.Subscribe' for GroupMessageReceiver
    /// </summary>
    /// <param name="observable"></param>
    /// <param name="action"></param>
    /// <returns></returns>
    public static IDisposable SubscribeGroupMessage(this IObservable<MessageReceiverBase> observable, Action<GroupMessageReceiver> action)
    {
        return observable.OfType<GroupMessageReceiver>().Subscribe(action);
    }

    /// <summary>
    /// An encapsulation of 'OfType.Subscribe' for GroupMessageReceiver
    /// </summary>
    /// <param name="observable"></param>
    /// <param name="action"></param>
    /// <returns></returns>
    public static IDisposable SubscribeGroupMessageAsync(this IObservable<MessageReceiverBase> observable, Func<GroupMessageReceiver, Task> action)
    {
        return observable.OfType<GroupMessageReceiver>().Subscribe(r =>
        {
            action(r);
        });
    }

    /// <summary>
    /// An encapsulation of 'OfType.Subscribe' for FriendMessageReceiver
    /// </summary>
    /// <param name="observable"></param>
    /// <param name="action"></param>
    /// <returns></returns>
    public static IDisposable SubscribeFriendMessage(this IObservable<MessageReceiverBase> observable, Action<FriendMessageReceiver> action)
    {
        return observable.OfType<FriendMessageReceiver>().Subscribe(action);
    }

    /// <summary>
    /// An encapsulation of 'OfType.Subscribe' for GroupMessageReceiver
    /// </summary>
    /// <param name="observable"></param>
    /// <param name="action"></param>
    /// <returns></returns>
    public static IDisposable SubscribeFriendMessageAsync(this IObservable<MessageReceiverBase> observable, Func<FriendMessageReceiver, Task> action)
    {
        return observable.OfType<FriendMessageReceiver>().Subscribe(r =>
        {
            action(r);
        });
    }

    #endregion

    #region Modularization extensions

    /// <summary>
    /// 把MessageReceiverBase转换为具体的MessageReceiver
    /// </summary>
    /// <param name="base"></param>
    /// <typeparam name="T"></typeparam>
    /// <returns></returns>
    public static T Concretize<T>(this MessageReceiverBase @base) where T : MessageReceiverBase
    {
        return (T)@base;
    }

    /// <summary>
    /// 使用模块
    /// </summary>
    /// <param name="source"></param>
    /// <param name="modules"></param>
    /// <returns></returns>
    public static IObservable<MessageReceiverBase> WithModules(this IObservable<MessageReceiverBase> source, List<IModule> modules)
    {
        source.Subscribe(modules.Raise);
        return source;
    }

    #endregion

    #region Message extension

    /// <summary>
    ///     发送群消息
    /// </summary>
    /// <param name="receiver"></param>
    /// <param name="chain"></param>
    /// <returns></returns>
    public static async Task<string> SendMessageAsync(this GroupMessageReceiver receiver,
        MessageChain chain)
    {
        return await MessageManager
            .SendGroupMessageAsync(receiver.Sender.Group.Id, chain);
    }

    /// <summary>
    ///     发送好友消息
    /// </summary>
    /// <param name="receiver"></param>
    /// <param name="chain"></param>
    /// <returns></returns>
    public static async Task<string> SendMessageAsync(this FriendMessageReceiver receiver,
        MessageChain chain)
    {
        return await MessageManager
            .SendFriendMessageAsync(receiver.Sender.Id, chain);
    }

    /// <summary>
    ///     发送临时消息
    /// </summary>
    /// <param name="receiver"></param>
    /// <param name="chain"></param>
    /// <returns></returns>
    public static async Task<string> SendMessageAsync(this TempMessageReceiver receiver, MessageChain chain)
    {
        return await MessageManager
            .SendTempMessageAsync(receiver.Sender.Id, receiver.Sender.Group.Id, chain);
    }

    /// <summary>
    ///     撤回收到的消息
    /// </summary>
    /// <param name="receiver"></param>
    public static async Task RecallAsync(this GroupMessageReceiver receiver)
    {
        var id = receiver.MessageChain.OfType<SourceMessage>().First().MessageId;
        await MessageManager
            .RecallAsync(id, receiver.GroupId);
    }

    /// <summary>
    ///     撤回收到的消息
    /// </summary>
    /// <param name="receiver"></param>
    public static async Task RecallAsync(this FriendMessageReceiver receiver)
    {
        var id = receiver.MessageChain.OfType<SourceMessage>().First().MessageId;
        await MessageManager
            .RecallAsync(id, receiver.FriendId);
    }

    /// <summary>
    ///     撤回收到的消息
    /// </summary>
    /// <param name="receiver"></param>
    public static async Task RecallAsync(this TempMessageReceiver receiver)
    {
        var id = receiver.MessageChain.OfType<SourceMessage>().First().MessageId;
        await MessageManager
            .RecallAsync(id, receiver.Sender.Id);
    }

    /// <summary>
    /// 回复消息
    /// </summary>
    /// <param name="receiver"></param>
    /// <param name="chain"></param>
    /// <returns></returns>
    public static async Task<string> QuoteMessageAsync(this FriendMessageReceiver receiver,
        MessageChain chain)
    {
        var id = receiver.MessageChain.OfType<SourceMessage>().First().MessageId;

        return await MessageManager
            .QuoteFriendMessageAsync(receiver.Sender.Id, id, chain);
    }

    /// <summary>
    /// 回复消息
    /// </summary>
    /// <param name="receiver"></param>
    /// <param name="chain"></param>
    /// <returns></returns>
    public static async Task<string> QuoteMessageAsync(this GroupMessageReceiver receiver,
        MessageChain chain)
    {
        var id = receiver.MessageChain.OfType<SourceMessage>().First().MessageId;

        return await MessageManager
            .QuoteGroupMessageAsync(receiver.Sender.Group.Id, id, chain);
    }

    /// <summary>
    /// 回复消息
    /// </summary>
    /// <param name="receiver"></param>
    /// <param name="chain"></param>
    /// <returns></returns>
    public static async Task<string> QuoteMessageAsync(this TempMessageReceiver receiver,
        MessageChain chain)
    {
        var id = receiver.MessageChain.OfType<SourceMessage>().First().MessageId;

        return await MessageManager
            .QuoteTempMessageAsync(receiver.Sender.Id, receiver.Sender.Group.Id, id, chain);
    }

    #endregion

    #region Request extensions

    /// <summary>
    ///     处理好友请求
    /// </summary>
    /// <param name="event"></param>
    /// <param name="handler"></param>
    /// <param name="message"></param>
    [Obsolete("请使用直接Approve/RejectAndBlock/RejectAsync 等方法")]
    public static async Task Handle(this NewFriendRequestedEvent @event,
        NewFriendRequestHandlers handler, string message = "")
    {
        await RequestManager.HandleNewFriendRequestedAsync(@event, handler, message);
    }

    /// <summary>
    /// 同意好友请求
    /// </summary>
    /// <param name="event"></param>
    public static async Task ApproveAsync(this NewFriendRequestedEvent @event)
    {
        await RequestManager.HandleNewFriendRequestedAsync(@event, NewFriendRequestHandlers.Approve);
    }

    /// <summary>
    /// 拒绝好友请求
    /// </summary>
    /// <param name="event">事件</param>
    /// <param name="message">回复的消息</param>
    public static async Task RejectAsync(this NewFriendRequestedEvent @event, string message = "")
    {
        await RequestManager.HandleNewFriendRequestedAsync(@event, NewFriendRequestHandlers.Reject, message);
    }

    /// <summary>
    /// 拒绝好友请求且屏蔽对方
    /// </summary>
    /// <param name="event">事件</param>
    /// <param name="message">回复的消息</param>
    public static async Task RejectAndBlockAsync(this NewFriendRequestedEvent @event, string message = "")
    {
        await RequestManager.HandleNewFriendRequestedAsync(@event, NewFriendRequestHandlers.RejectAndBlock, message);
    }

    /// <summary>
    ///     处理新成员加群请求
    /// </summary>
    /// <param name="requestedEvent"></param>
    /// <param name="handler"></param>
    /// <param name="message"></param>
    [Obsolete("请使用直接Approve/RejectAndBlock/RejectAsync 等方法")]
    public static async Task Handle(this NewMemberRequestedEvent requestedEvent,
        NewMemberRequestHandlers handler, string message = "")
    {
        await RequestManager
            .HandleNewMemberRequestedAsync(requestedEvent, handler, message);
    }

    /// <summary>
    /// 同意加群请求
    /// </summary>
    /// <param name="requestedEvent"></param>
    public static async Task ApproveAsync(this NewMemberRequestedEvent requestedEvent)
    {
        await RequestManager.HandleNewMemberRequestedAsync(requestedEvent, NewMemberRequestHandlers.Approve);
    }

    /// <summary>
    /// 拒绝加群请求
    /// </summary>
    /// <param name="requestedEvent">事件源</param>
    /// <param name="message">回复消息</param>
    public static async Task RejectAsync(this NewMemberRequestedEvent requestedEvent, string message = "")
    {
        await RequestManager.HandleNewMemberRequestedAsync(requestedEvent, NewMemberRequestHandlers.Reject, message);
    }

    /// <summary>
    /// 忽略加群请求
    /// </summary>
    /// <param name="requestedEvent"></param>
    public static async Task DismissAsync(this NewMemberRequestedEvent requestedEvent)
    {
        await RequestManager.HandleNewMemberRequestedAsync(requestedEvent, NewMemberRequestHandlers.Dismiss);
    }

    /// <summary>
    /// 拒绝加群请求并屏蔽
    /// </summary>
    /// <param name="requestedEvent"></param>
    /// <param name="message">回复消息</param>
    public static async Task RejectAndBlockAsync(this NewMemberRequestedEvent requestedEvent, string message = "")
    {
        await RequestManager.HandleNewMemberRequestedAsync(requestedEvent, NewMemberRequestHandlers.RejectAndBlock, message);
    }

    /// <summary>
    /// 忽略加群请求并屏蔽
    /// </summary>
    /// <param name="requestedEvent"></param>
    public static async Task DismissAndBlockAsync(this NewMemberRequestedEvent requestedEvent)
    {
        await RequestManager.HandleNewMemberRequestedAsync(requestedEvent, NewMemberRequestHandlers.DismissAndBlock);
    }

    /// <summary>
    ///     处理bot被邀请进群请求
    /// </summary>
    /// <param name="requestedEvent"></param>
    /// <param name="handler"></param>
    /// <param name="message"></param>
    [Obsolete("请使用直接Approve/RejectAndBlock/RejectAsync 等方法")]
    public static async Task Handle(this NewInvitationRequestedEvent requestedEvent,
        NewInvitationRequestHandlers handler, string message)
    {
        await RequestManager
            .HandleNewInvitationRequestedAsync(requestedEvent, handler, message);
    }

    /// <summary>
    /// 同意邀请
    /// </summary>
    /// <param name="requestedEvent"></param>
    public static async Task ApproveAsync(this NewInvitationRequestedEvent requestedEvent)
    {
        await RequestManager.HandleNewInvitationRequestedAsync(requestedEvent, NewInvitationRequestHandlers.Approve,
            "");
    }

    /// <summary>
    /// 拒绝邀请
    /// </summary>
    /// <param name="requestedEvent"></param>
    public static async Task RejectAsync(this NewInvitationRequestedEvent requestedEvent)
    {
        await RequestManager.HandleNewInvitationRequestedAsync(requestedEvent, NewInvitationRequestHandlers.Reject,
            "");
    }

    #endregion
}