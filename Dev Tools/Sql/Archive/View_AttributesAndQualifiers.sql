SELECT
	a.[Id] as 'Attribute Id'
	, a.[Name] as 'Attribute Name'
	, c.[Name] as 'CategoryName'
	, e.[Name] as 'on EntityTypeName'
	, ft.[Name] as 'Field Type'
	, q.[Key] as 'Attribute Qualifier Key'
	, CASE q.[Key]
		WHEN 'definedtype' THEN (SELECT '(DefinedType:' + cast(dt.[Id] as varchar) + ') ' + dt.[Name] FROM [DefinedType] dt where dt.Id = q.[Value] )
		ELSE q.[Value]
		END as 'Attribute Qualifier Value'
	, CASE ft.[Guid]
		WHEN 'BC48720C-3610-4BCF-AE66-D255A17F1CDF' THEN (SELECT '(DefinedType:' + cast(dt.[Id] as varchar) + ') ' + dt.[Name] FROM [DefinedType] dt where dt.Id = a.DefaultValue )
		ELSE a.[DefaultValue]
		END as 'Default Value'
	, a.[Description]
	, a.[Order]
	, a.[IsSystem]
	, a.[IsRequired]
	, a.[Guid]
FROM [Attribute] a
left join [AttributeCategory] ac on ac.[AttributeId] = a.Id
left join [Category] c on c.[Id] = ac.[CategoryId]
join [EntityType] [e] on [e].[Id] = [a].[EntityTypeId]
join [FieldType] [ft] on [ft].[Id] = [a].[FieldTypeId]
left join [AttributeQualifier] q on q.[AttributeId] = a.[Id]
order by c.[Name], e.[Name]