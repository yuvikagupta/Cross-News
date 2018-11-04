using System.Windows.Input;

namespace CrossNews.Core.Extensions
{
    public static class CommandExtensions
    {
        public static void TryExecute(this ICommand self)
        {
            if (self.CanExecute(null))
                self.Execute(null);
        }

        public static void TryExecute(this ICommand self, object param)
        {
            if (self.CanExecute(param))
                self.Execute(param);
        }
    }
}
