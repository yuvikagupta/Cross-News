using System.Runtime.Serialization;

namespace CrossNews.Core.Model.Api
{
    public enum ItemType
    {
        [EnumMember(Value = "story")] Story,
        [EnumMember(Value = "comment")] Comment,
        [EnumMember(Value = "job")] Job,
        [EnumMember(Value = "poll")] Poll,
        [EnumMember(Value = "pollopt")] PollOption,
    }
}