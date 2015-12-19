namespace Dal.Core.Entities.Interfaces
{
    public interface IEntity<TKey>
    {
        /// <summary>
        /// Идентификатор объекта
        /// </summary>
        TKey Id { get; set; }
    }
}
