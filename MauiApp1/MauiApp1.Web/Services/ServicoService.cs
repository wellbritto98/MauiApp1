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
        using var connection = _dbConnection.GetConnection();
        await connection.OpenAsync();

        var whereClause = "WHERE 1=1";
        var parameters = new DynamicParameters();

        if (!string.IsNullOrEmpty(filter.SearchTerm))
        {
            whereClause += " AND (serv_tx_descricao ILIKE @searchTerm OR serv_tx_descricao_coleta ILIKE @searchTerm)";
            parameters.Add("searchTerm", $"%{filter.SearchTerm}%");
        }

        var orderBy = "ORDER BY serv_dt_cadastro DESC";
        if (!string.IsNullOrEmpty(filter.SortColumn))
        {
            orderBy = $"ORDER BY {filter.SortColumn} {(filter.SortOrder == "desc" ? "DESC" : "ASC")}";
        }

        var offset = (filter.PageNumber - 1) * filter.PageSize;

        var countQuery = $@"
            SELECT COUNT(*) 
            FROM tb_servico 
            {whereClause}";

        var dataQuery = $@"
            SELECT font_sg_fonte, serv_nr_codigo, serv_tx_descricao, serv_tx_descricao_coleta, 
                   serv_sg_unidade, serv_sg_unidade_coleta, serv_vl_fator_conversao, gpsr_nr_codigo,
                   serv_in_cesta_basica, serv_tx_responsavel, serv_dt_ult_revisao, serv_dt_cadastro,
                   serv_in_desativado, font_sg_fonte_ant, serv_nr_codigo_ant, serv_dt_ult_coleta,
                   serv_tx_descricao_complementar
            FROM tb_servico 
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

    public async Task<Servico?> GetByIdAsync(string fontSgFonte, int servNrCodigo)
    {
        using var connection = _dbConnection.GetConnection();
        await connection.OpenAsync();

        var query = @"
            SELECT font_sg_fonte, serv_nr_codigo, serv_tx_descricao, serv_tx_descricao_coleta, 
                   serv_sg_unidade, serv_sg_unidade_coleta, serv_vl_fator_conversao, gpsr_nr_codigo,
                   serv_in_cesta_basica, serv_tx_responsavel, serv_dt_ult_revisao, serv_dt_cadastro,
                   serv_in_desativado, font_sg_fonte_ant, serv_nr_codigo_ant, serv_dt_ult_coleta,
                   serv_tx_descricao_complementar
            FROM tb_servico 
            WHERE font_sg_fonte = @fontSgFonte AND serv_nr_codigo = @servNrCodigo";

        return await connection.QueryFirstOrDefaultAsync<Servico>(query, new { fontSgFonte, servNrCodigo });
    }

    public async Task<Servico> CreateAsync(Servico servico)
    {
        using var connection = _dbConnection.GetConnection();
        await connection.OpenAsync();

        var query = @"
            INSERT INTO tb_servico (font_sg_fonte, serv_nr_codigo, serv_tx_descricao, serv_tx_descricao_coleta,
                                   serv_sg_unidade, serv_sg_unidade_coleta, serv_vl_fator_conversao, gpsr_nr_codigo,
                                   serv_in_cesta_basica, serv_tx_responsavel, serv_dt_ult_revisao, serv_dt_cadastro,
                                   serv_in_desativado, font_sg_fonte_ant, serv_nr_codigo_ant, serv_dt_ult_coleta,
                                   serv_tx_descricao_complementar)
            VALUES (@FontSgFonte, @ServNrCodigo, @ServTxDescricao, @ServTxDescricaoColeta,
                    @ServSgUnidade, @ServSgUnidadeColeta, @ServVlFatorConversao, @GpsrNrCodigo,
                    @ServInCestaBasica, @ServTxResponsavel, @ServDtUltRevisao, @ServDtCadastro,
                    @ServInDesativado, @FontSgFonteAnt, @ServNrCodigoAnt, @ServDtUltColeta,
                    @ServTxDescricaoComplementar)";

        await connection.ExecuteAsync(query, servico);
        return servico;
    }

    public async Task<Servico> UpdateAsync(Servico servico)
    {
        using var connection = _dbConnection.GetConnection();
        await connection.OpenAsync();

        var query = @"
            UPDATE tb_servico 
            SET serv_tx_descricao = @ServTxDescricao, serv_tx_descricao_coleta = @ServTxDescricaoColeta,
                serv_sg_unidade = @ServSgUnidade, serv_sg_unidade_coleta = @ServSgUnidadeColeta,
                serv_vl_fator_conversao = @ServVlFatorConversao, gpsr_nr_codigo = @GpsrNrCodigo,
                serv_in_cesta_basica = @ServInCestaBasica, serv_tx_responsavel = @ServTxResponsavel,
                serv_dt_ult_revisao = @ServDtUltRevisao, serv_in_desativado = @ServInDesativado,
                font_sg_fonte_ant = @FontSgFonteAnt, serv_nr_codigo_ant = @ServNrCodigoAnt,
                serv_dt_ult_coleta = @ServDtUltColeta, serv_tx_descricao_complementar = @ServTxDescricaoComplementar
            WHERE font_sg_fonte = @FontSgFonte AND serv_nr_codigo = @ServNrCodigo";

        await connection.ExecuteAsync(query, servico);
        return servico;
    }

    public async Task<bool> DeleteAsync(string fontSgFonte, int servNrCodigo)
    {
        using var connection = _dbConnection.GetConnection();
        await connection.OpenAsync();

        var query = "DELETE FROM tb_servico WHERE font_sg_fonte = @fontSgFonte AND serv_nr_codigo = @servNrCodigo";
        var rowsAffected = await connection.ExecuteAsync(query, new { fontSgFonte, servNrCodigo });

        return rowsAffected > 0;
    }

    public async Task<IEnumerable<Fonte>> GetFontesAsync()
    {
        using var connection = _dbConnection.GetConnection();
        await connection.OpenAsync();

        var query = @"
            SELECT font_sg_fonte, font_tx_descricao, font_tx_endereco, font_tx_telefone, 
                   font_tx_email, font_in_ativo, font_dt_cadastro
            FROM tb_fonte 
            WHERE font_in_ativo = 'S'
            ORDER BY font_tx_descricao";

        return await connection.QueryAsync<Fonte>(query);
    }

    public async Task<IEnumerable<GrupoServico>> GetGruposServicoAsync()
    {
        using var connection = _dbConnection.GetConnection();
        await connection.OpenAsync();

        var query = @"
            SELECT gpsr_nr_codigo, gpsr_tx_descricao, gpsr_in_ativo, gpsr_dt_cadastro
            FROM tb_grupo_servico 
            WHERE gpsr_in_ativo = 'S'
            ORDER BY gpsr_tx_descricao";

        return await connection.QueryAsync<GrupoServico>(query);
    }
}
