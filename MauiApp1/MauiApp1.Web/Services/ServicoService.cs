using Dapper;
using MauiApp1.Web.Data;
using MauiApp1.Web.Models;
using Npgsql;

namespace MauiApp1.Web.Services;

public class ServicoService : IServicoService
{
    private readonly DatabaseConnection _dbConnection;

    public ServicoService(DatabaseConnection dbConnection)
    {
        _dbConnection = dbConnection;
    }

    public async Task<PagedResult<Servico>> GetPagedAsync(FilterRequest filter)
    {
        using var connection = _dbConnection.CreateConnection();
        await connection.OpenAsync();

        var whereClause = "WHERE 1=1";
        var parameters = new DynamicParameters();

        if (!string.IsNullOrEmpty(filter.SearchTerm))
        {
            whereClause += " AND (Nome ILIKE @searchTerm OR Descricao ILIKE @searchTerm OR Categoria ILIKE @searchTerm)";
            parameters.Add("searchTerm", $"%{filter.SearchTerm}%");
        }

        var orderBy = "ORDER BY DataCriacao DESC";
        if (!string.IsNullOrEmpty(filter.SortBy))
        {
            orderBy = $"ORDER BY {filter.SortBy} {(filter.SortDescending ? "DESC" : "ASC")}";
        }

        var offset = (filter.PageNumber - 1) * filter.PageSize;

        var countQuery = $@"
            SELECT COUNT(*) 
            FROM Servicos 
            {whereClause}";

        var dataQuery = $@"
            SELECT Id, Nome, Descricao, Preco, DuracaoMinutos, Categoria, DataCriacao, DataAtualizacao, Ativo
            FROM Servicos 
            {whereClause}
            {orderBy}
            LIMIT @pageSize OFFSET @offset";

        parameters.Add("pageSize", filter.PageSize);
        parameters.Add("offset", offset);

        var totalCount = await connection.QuerySingleAsync<int>(countQuery, parameters);
        var items = await connection.QueryAsync<Servico>(dataQuery, parameters);

        return new PagedResult<Servico>
        {
            Items = items.ToList(),
            TotalCount = totalCount,
            PageNumber = filter.PageNumber,
            PageSize = filter.PageSize
        };
    }

    public async Task<Servico?> GetByIdAsync(int id)
    {
        using var connection = _dbConnection.CreateConnection();
        await connection.OpenAsync();

        var query = @"
            SELECT Id, Nome, Descricao, Preco, DuracaoMinutos, Categoria, DataCriacao, DataAtualizacao, Ativo
            FROM Servicos 
            WHERE Id = @id";

        return await connection.QueryFirstOrDefaultAsync<Servico>(query, new { id });
    }

    public async Task<Servico> CreateAsync(Servico servico)
    {
        using var connection = _dbConnection.CreateConnection();
        await connection.OpenAsync();

        var query = @"
            INSERT INTO Servicos (Nome, Descricao, Preco, DuracaoMinutos, Categoria, DataCriacao, Ativo)
            VALUES (@Nome, @Descricao, @Preco, @DuracaoMinutos, @Categoria, @DataCriacao, @Ativo)
            RETURNING Id";

        var id = await connection.QuerySingleAsync<int>(query, servico);
        servico.Id = id;

        return servico;
    }

    public async Task<Servico> UpdateAsync(Servico servico)
    {
        using var connection = _dbConnection.CreateConnection();
        await connection.OpenAsync();

        var query = @"
            UPDATE Servicos 
            SET Nome = @Nome, Descricao = @Descricao, Preco = @Preco, 
                DuracaoMinutos = @DuracaoMinutos, Categoria = @Categoria, 
                DataAtualizacao = @DataAtualizacao, Ativo = @Ativo
            WHERE Id = @Id";

        await connection.ExecuteAsync(query, servico);
        return servico;
    }

    public async Task<bool> DeleteAsync(int id)
    {
        using var connection = _dbConnection.CreateConnection();
        await connection.OpenAsync();

        var query = "DELETE FROM Servicos WHERE Id = @id";
        var rowsAffected = await connection.ExecuteAsync(query, new { id });

        return rowsAffected > 0;
    }
}