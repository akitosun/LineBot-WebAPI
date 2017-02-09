using Domain;
using Utility.Helper;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Service
{
    public class LineHandler : ILineHandler
    {
        /// <summary>
        /// 建構子
        /// </summary>
        public LineHandler()
        {
        }

        /// <summary>
        /// 處理程序
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public async Task ProcessMessage(WebhookModel value)
        {
            foreach (var msg in value.events)
            {
                switch (msg.type)
                {
                    case "follow":
                        await APIHelper.ReplyMessage(GetTextReply(msg.replyToken, "follow event type"));
                        break;

                    case "postback":
                        await APIHelper.ReplyMessage(GetTextReply(msg.replyToken, msg.postback.data));
                        break;

                    case "join":
                        await APIHelper.ReplyMessage(GetTextReply(msg.replyToken, "join event type"));
                        break;

                    case "message":
                        var res = await APIHelper.ReplyMessage(HandleMessageObject(msg));
                        break;

                    default:
                        await APIHelper.ReplyMessage(GetTextReply(msg.replyToken, "not support"));
                        break;
                }
            }
        }

        private ReplyModel HandleMessageObject(Event msg)
        {
            switch (msg.message.type)
            {
                case "sticker":
                    Random rnd = new Random();
                    return GetStickerReply(msg.replyToken, "1", rnd.Next(1, 18).ToString());

                case "text":
                    return GetTextReply(msg.replyToken, $"echo: {msg.message.text}");

                case "image":
                    return GetTextReply(msg.replyToken, "img");

                case "video":
                    return GetTextReply(msg.replyToken, "video");

                case "audio":
                    return GetTextReply(msg.replyToken, "audio");

                case "location":
                    return GetTextReply(msg.replyToken, $"({msg.message.latitude},{msg.message.longitude})");

                default:
                    return GetTextReply(msg.replyToken, msg.message.type);
            }
        }

        private ReplyModel GetTextReply(string token, string text)
        {
            return new ReplyModel()
            {
                replyToken = token,
                messages = new List<ReplyMessage>()
                {
                    new ReplyMessage()
                    {
                        type = "text",
                        text = text
                    }
                }
            };
        }

        private ReplyModel GetStickerReply(string token, string package, string sticker)
        {
            return new ReplyModel()
            {
                replyToken = token,
                messages = new List<ReplyMessage>()
                {
                    new ReplyMessage()
                    {
                        type = "sticker",
                        packageId = package,
                        stickerId = sticker
                    }
                }
            };
        }
    }
}