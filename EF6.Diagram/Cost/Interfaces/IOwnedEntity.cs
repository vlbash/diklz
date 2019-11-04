using System;

namespace Astum.Core.Data.Interfaces
{
	public interface IOwnedEntity : IEntity
	{
        Guid? OwnerId { get; set; }
	}
}
