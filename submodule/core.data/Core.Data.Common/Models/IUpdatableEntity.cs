namespace Core.Data.Common.Models
{
    public interface IUpdatableEntity
    {
        void UpdateFromSource(IUpdatableEntity sourceEntity);
    }
}
