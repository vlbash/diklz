namespace Astum.Core.Data.Interfaces
{
	public interface IUpdatableEntity
	{
		void UpdateFromSource(IUpdatableEntity sourceEntity);
	}
}
