using TinyViz.Contracts.Model;

namespace TinyViz.Contracts;

public interface IDataSource
{
    Task<RangeQueryResult> QueryRangeAsync(QueryDefinition queryDefinition, CancellationToken cancellationToken = default);
}
