using System;
using Foundation;

namespace CrossNews.Ios.Extensions
{
    public static class StringExtensions
    {
        public static NSString ToNSString(this string self) => (NSString) self;
    }
}
