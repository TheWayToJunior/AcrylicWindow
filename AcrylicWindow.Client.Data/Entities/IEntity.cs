namespace AcrylicWindow.Client.Data
{
    public interface IEntity<out TKey>
    {
        TKey Id { get; }
    }
}
