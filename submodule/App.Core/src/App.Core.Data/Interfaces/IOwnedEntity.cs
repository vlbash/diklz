using System;
using App.Core.Base;

namespace App.Core.Data.Interfaces
{
	public interface IOwnedEntity : IEntity
	{
        Guid? OwnerId { get; set; }
	}
}
