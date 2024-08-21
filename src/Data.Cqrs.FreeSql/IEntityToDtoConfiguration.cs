namespace Olbrasoft.Data.Cqrs.FreeSql;

public interface IEntityToDtoConfiguration<TEntity, TDto> : IConfiguration
{
    Expression<Func<TEntity, TDto>> Configure();
}
