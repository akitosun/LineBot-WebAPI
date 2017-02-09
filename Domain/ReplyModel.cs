using System.Collections.Generic;

namespace Domain
{
    public class ReplyModel
    {
        public string replyToken { get; set; }
        public List<ReplyMessage> messages { get; set; }
    }

    public class ReplyMessage
    {
        public string type { get; set; }
        public string text { get; set; }
        public string packageId { get; set; }
        public string stickerId { get; set; }
        public string altText { get; set; }
        public Template template { get; set; }
    }

    public class Template
    {
        public string type { get; set; }
        public List<Column> columns { get; set; }
        public string thumbnailImageUrl { get; set; }
        public string title { get; set; }
        public string text { get; set; }
        public List<Action> actions { get; set; }
    }

    public class Action
    {
        public string type { get; set; }
        public string label { get; set; }
        public string data { get; set; }
        public string uri { get; set; }
    }

    public class Column
    {
        public string thumbnailImageUrl { get; set; }
        public string title { get; set; }
        public string text { get; set; }
        public List<Action> actions { get; set; }
    }
}