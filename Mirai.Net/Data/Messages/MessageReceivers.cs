﻿using System.ComponentModel;
using System.Runtime.Serialization;

namespace Mirai.Net.Data.Messages;

/// <summary>
/// 消息接收器的类型
/// </summary>
public enum MessageReceivers
{
    /// <summary>
    /// 好友消息
    /// </summary>
    [Description("FriendMessage")]
    [EnumMember(Value = "FriendMessage")]
    Friend,

    /// <summary>
    /// 群消息
    /// </summary>
    [Description("GroupMessage")]
    [EnumMember(Value = "GroupMessage")]
    Group,

    /// <summary>
    /// 临时消息
    /// </summary>
    [Description("TempMessage")]
    [EnumMember(Value = "TempMessage")]
    Temp,

    /// <summary>
    /// 陌生人消息
    /// </summary>
    [Description("StrangerMessage")]
    [EnumMember(Value = "StrangerMessage")]
    Stranger,

    /// <summary>
    /// 其它客户端消息
    /// </summary>
    [Description("OtherClientMessage")]
    [EnumMember(Value = "OtherClientMessage")]
    OtherClient,

    /// <summary>
    /// 好友消息的同步消息
    /// </summary>
    [Description("FriendSyncMessage")]
    [EnumMember(Value = "FriendSyncMessage")]
    FriendSync,

    /// <summary>
    /// 群消息的同步消息
    /// </summary>
    [Description("GroupSyncMessage")]
    [EnumMember(Value = "GroupSyncMessage")]
    GroupSync,

    /// <summary>
    /// 临时消息的同步消息
    /// </summary>
    [Description("TempSyncMessage")]
    [EnumMember(Value = "TempSyncMessage")]
    TempSync,

    /// <summary>
    /// 未知类型的接收器
    /// </summary>
    Unknown
}