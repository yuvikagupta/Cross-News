using CrossNews.Core.Model.Api;

namespace CrossNews.Core.Messages
{

    internal class NewsItemMessage : ItemMessage<Item>
    {
        public NewsItemMessage(object sender, Item data) : base(sender, data)
        {
        }
    }
}
