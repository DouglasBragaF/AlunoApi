using Xunit;

namespace AlunoApi.Tests.Integration.Database;

[CollectionDefinition("PostgresDatabase")]
public sealed class PostgresCollection
    : ICollectionFixture<PostgresDatabaseFixture>
{
}