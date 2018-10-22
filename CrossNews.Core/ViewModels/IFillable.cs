namespace CrossNews.Core.ViewModels
{
    public interface IFillable<in T>
    {
        bool Filled { get; }
        void Fill(T item);
    }
}