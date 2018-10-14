namespace CrossNews.Core.ViewModels
{
    public interface IFillable<in T>
    {
        void Fill(T item);
    }
}