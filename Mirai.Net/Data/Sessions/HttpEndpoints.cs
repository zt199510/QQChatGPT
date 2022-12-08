﻿using System.ComponentModel;

namespace Mirai.Net.Data.Sessions;

/// <summary>
///     http请求端点
/// </summary>
internal enum HttpEndpoints
{
    [Description("verify")] Verify,
    [Description("bind")] Bind,
    [Description("release")] Release,
    [Description("about")] About,

    [Description("friendList")] FriendList,
    [Description("groupList")] GroupList,
    [Description("memberList")] MemberList,
    [Description("botProfile")] BotProfile,
    [Description("friendProfile")] FriendProfile,
    [Description("memberProfile")] MemberProfile,

    [Description("sendFriendMessage")] SendFriendMessage,
    [Description("sendGroupMessage")] SendGroupMessage,
    [Description("sendTempMessage")] SendTempMessage,
    [Description("sendNudge")] SendNudge,
    [Description("recall")] Recall,
    [Description("roamingMessages")] RoamingMessages,

    [Description("file/list")] FileList,
    [Description("file/info")] FileInfo,
    [Description("file/mkdir")] FileCreate,
    [Description("file/delete")] FileDelete,
    [Description("file/move")] FileMove,
    [Description("file/rename")] FileRename,
    [Description("file/upload")] FileUpload,

    [Description("uploadImage")] UploadImage,
    [Description("uploadVoice")] UploadVoice,

    [Description("deleteFriend")] DeleteFriend,

    [Description("mute")] Mute,
    [Description("unmute")] Unmute,
    [Description("kick")] Kick,
    [Description("quit")] Leave,
    [Description("muteAll")] MuteAll,
    [Description("unmuteAll")] UnmuteAll,
    [Description("setEssence")] SetEssence,
    [Description("groupConfig")] GroupConfig,
    [Description("memberInfo")] MemberInfo,
    [Description("userProfile")] UserProfile,

    [Description("anno/list")] GetAnnouncement,
    [Description("anno/publish")] PubAnnouncement,
    [Description("anno/delete")] DelAnnouncement,

    [Description("resp/newFriendRequestEvent")]
    NewFriendRequested,

    [Description("resp/memberJoinRequestEvent")]
    MemberJoinRequested,

    [Description("resp/botInvitedJoinGroupRequestEvent")]
    BotInvited,

    [Description("messageFromId")]
    MessageFromId,

    [Description("cmd/execute")] ExecuteCommand,
    [Description("cmd/register")] RegisterCommand
}