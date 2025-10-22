using Dapper;
using MauiApp1.Web.Data;
using MauiApp1.Web.Models;
using Npgsql;

namespace MauiApp1.Web.Services;

public class InsumoService : IInsumoService
{
    private readonly DatabaseConnection _dbConnection;

    public InsumoService(DatabaseConnection dbConnection)
    {
        _dbConnection = dbConnection;
    }

    public async Task<PagedResult<Insumo>> GetPagedAsync(FilterRequest filter)
    {
        using var connection = _dbConnection.CreateConnection();
        await connection.OpenAsync();

        var whereClause = "WHERE 1=1";
        var parameters = new DynamicParameters();

        if (!string.IsNullOrEmpty(filter.SearchTerm))
        {
            whereClause += " AND (Nome ILIKE @searchTerm OR Descricao ILIKE @searchTerm)";
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
            FROM Insumos 
            {whereClause}";

        var dataQuery = $@"
            SELECT Id, Nome, Descricao, Preco, Quantidade, Unidade, DataCriacao, DataAtualizacao, Ativo
            FROM Insumos 
            {whereClause}
            {orderBy}
            LIMIT @pageSize OFFSET @offset";

        parameters.Add("pageSize", filter.PageSize);
        parameters.Add("offset", offset);

        var totalCount = await connection.QuerySingleAsync<int>(countQuery, parameters);
        var items = await connection.QueryAsync<Insumo>(dataQuery, parameters);

        return new PagedResult<Insumo>
        {
            Items = items.ToList(),
            TotalCount = totalCount,
            PageNumber = filter.PageNumber,
            PageSize = filter.PageSize
        };
    }

    public async Task<Insumo?> GetByIdAsync(int id)
    {
        using var connection = _dbConnection.CreateConnection();
        await connection.OpenAsync();

        var query = @"
            SELECT Id, Nome, Descricao, Preco, Quantidade, Unidade, DataCriacao, DataAtualizacao, Ativo
            FROM Insumos 
            WHERE Id = @id";

        return await connection.QueryFirstOrDefaultAsync<Insumo>(query, new { id });
    }

    public async Task<Insumo> CreateAsync(Insumo insumo)
    {
        using var connection = _dbConnection.CreateConnection();
        await connection.OpenAsync();

        var query = @"
            INSERT INTO Insumos (Nome, Descricao, Preco, Quantidade, Unidade, DataCriacao, Ativo)
            VALUES (@Nome, @Descricao, @Preco, @Quantidade, @Unidade, @DataCriacao, @Ativo)
            RETURNING Id";

        var id = await connection.QuerySingleAsync<int>(query, insumo);
        insumo.Id = id;

        return insumo;
    }

    public async Task<Insumo> UpdateAsync(Insumo insumo)
    {
        using var connection = _dbConnection.CreateConnection();
        await connection.OpenAsync();

        var query = @"
            UPDATE Insumos 
            SET Nome = @Nome, Descricao = @Descricao, Preco = @Preco, 
                Quantidade = @Quantidade, Unidade = @Unidade, 
                DataAtualizacao = @DataAtualizacao, Ativo = @Ativo
            WHERE Id = @Id";

        await connection.ExecuteAsync(query, insumo);
        return insumo;
    }

    public async Task<bool> DeleteAsync(int id)
    {
        using var connection = _dbConnection.CreateConnection();
        await connection.OpenAsync();

        var query = "DELETE FROM Insumos WHERE Id = @id";
        var rowsAffected = await connection.ExecuteAsync(query, new { id });

        return rowsAffected > 0;
    }
}