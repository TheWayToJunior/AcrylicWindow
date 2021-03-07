namespace AcrylicWindow.Client.Entity
{
    public interface IEntity<out TKey>
    {
        TKey Id { get; }
    }
}
