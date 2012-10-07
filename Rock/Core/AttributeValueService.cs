//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by the Rock.CodeGeneration project
//     Changes to this file will be lost when the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------
//
// THIS WORK IS LICENSED UNDER A CREATIVE COMMONS ATTRIBUTION-NONCOMMERCIAL-
// SHAREALIKE 3.0 UNPORTED LICENSE:
// http://creativecommons.org/licenses/by-nc-sa/3.0/
//

using System;
using System.Linq;

using Rock.Data;

namespace Rock.Core
{
	/// <summary>
	/// AttributeValue Service class
	/// </summary>
	public partial class AttributeValueService : Service<AttributeValue, AttributeValueDto>
	{
		/// <summary>
		/// Initializes a new instance of the <see cref="AttributeValueService"/> class
		/// </summary>
		public AttributeValueService()
			: base()
		{
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="AttributeValueService"/> class
		/// </summary>
		public AttributeValueService(IRepository<AttributeValue> repository) : base(repository)
		{
		}

		/// <summary>
		/// Creates a new model
		/// </summary>
		public override AttributeValue CreateNew()
		{
			return new AttributeValue();
		}

		/// <summary>
		/// Query DTO objects
		/// </summary>
		/// <returns>A queryable list of DTO objects</returns>
		public override IQueryable<AttributeValueDto> QueryableDto( )
		{
			return QueryableDto( this.Queryable() );
		}

		/// <summary>
		/// Query DTO objects
		/// </summary>
		/// <returns>A queryable list of DTO objects</returns>
		public IQueryable<AttributeValueDto> QueryableDto( IQueryable<AttributeValue> items )
		{
			return items.Select( m => new AttributeValueDto()
				{
					IsSystem = m.IsSystem,
					AttributeId = m.AttributeId,
					EntityId = m.EntityId,
					Order = m.Order,
					Value = m.Value,
					Id = m.Id,
					Guid = m.Guid,				});
		}
	}
}
