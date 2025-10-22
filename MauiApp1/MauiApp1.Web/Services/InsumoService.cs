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
        using var connection = _dbConnection.GetConnection();
        await connection.OpenAsync();

        var whereClause = "WHERE 1=1";
        var parameters = new DynamicParameters();

        if (!string.IsNullOrEmpty(filter.SearchTerm))
        {
            whereClause += " AND (insu_tx_descricao ILIKE @searchTerm OR insu_tx_descricao_coleta ILIKE @searchTerm)";
            parameters.Add("searchTerm", $"%{filter.SearchTerm}%");
        }

        var orderBy = "ORDER BY insu_dt_cadastro DESC";
        if (!string.IsNullOrEmpty(filter.SortColumn))
        {
            orderBy = $"ORDER BY {filter.SortColumn} {(filter.SortOrder == "desc" ? "DESC" : "ASC")}";
        }

        var offset = (filter.PageNumber - 1) * filter.PageSize;

        var countQuery = $@"
            SELECT COUNT(*) 
            FROM tb_insumo 
            {whereClause}";

        var dataQuery = $@"
            SELECT font_sg_fonte, insu_nr_codigo, insu_tx_descricao, insu_tx_descricao_coleta, 
                   insu_sg_unidade, insu_sg_unidade_coleta, insu_vl_fator_conversao, gpin_nr_codigo,
                   insu_in_cesta_basica, insu_tx_responsavel, insu_dt_ult_revisao, insu_dt_cadastro,
                   insu_in_desativado, font_sg_fonte_ant, insu_nr_codigo_ant, insu_dt_ult_coleta,
                   insu_tx_descricao_complementar
            FROM tb_insumo 
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

    public async Task<Insumo?> GetByIdAsync(string fontSgFonte, int insuNrCodigo)
    {
        using var connection = _dbConnection.GetConnection();
        await connection.OpenAsync();

        var query = @"
            SELECT font_sg_fonte, insu_nr_codigo, insu_tx_descricao, insu_tx_descricao_coleta, 
                   insu_sg_unidade, insu_sg_unidade_coleta, insu_vl_fator_conversao, gpin_nr_codigo,
                   insu_in_cesta_basica, insu_tx_responsavel, insu_dt_ult_revisao, insu_dt_cadastro,
                   insu_in_desativado, font_sg_fonte_ant, insu_nr_codigo_ant, insu_dt_ult_coleta,
                   insu_tx_descricao_complementar
            FROM tb_insumo 
            WHERE font_sg_fonte = @fontSgFonte AND insu_nr_codigo = @insuNrCodigo";

        return await connection.QueryFirstOrDefaultAsync<Insumo>(query, new { fontSgFonte, insuNrCodigo });
    }

    public async Task<Insumo> CreateAsync(Insumo insumo)
    {
        using var connection = _dbConnection.GetConnection();
        await connection.OpenAsync();

        var query = @"
            INSERT INTO tb_insumo (font_sg_fonte, insu_nr_codigo, insu_tx_descricao, insu_tx_descricao_coleta,
                                  insu_sg_unidade, insu_sg_unidade_coleta, insu_vl_fator_conversao, gpin_nr_codigo,
                                  insu_in_cesta_basica, insu_tx_responsavel, insu_dt_ult_revisao, insu_dt_cadastro,
                                  insu_in_desativado, font_sg_fonte_ant, insu_nr_codigo_ant, insu_dt_ult_coleta,
                                  insu_tx_descricao_complementar)
            VALUES (@FontSgFonte, @InsuNrCodigo, @InsuTxDescricao, @InsuTxDescricaoColeta,
                    @InsuSgUnidade, @InsuSgUnidadeColeta, @InsuVlFatorConversao, @GpinNrCodigo,
                    @InsuInCestaBasica, @InsuTxResponsavel, @InsuDtUltRevisao, @InsuDtCadastro,
                    @InsuInDesativado, @FontSgFonteAnt, @InsuNrCodigoAnt, @InsuDtUltColeta,
                    @InsuTxDescricaoComplementar)";

        await connection.ExecuteAsync(query, insumo);
        return insumo;
    }

    public async Task<Insumo> UpdateAsync(Insumo insumo)
    {
        using var connection = _dbConnection.GetConnection();
        await connection.OpenAsync();

        var query = @"
            UPDATE tb_insumo 
            SET insu_tx_descricao = @InsuTxDescricao, insu_tx_descricao_coleta = @InsuTxDescricaoColeta,
                insu_sg_unidade = @InsuSgUnidade, insu_sg_unidade_coleta = @InsuSgUnidadeColeta,
                insu_vl_fator_conversao = @InsuVlFatorConversao, gpin_nr_codigo = @GpinNrCodigo,
                insu_in_cesta_basica = @InsuInCestaBasica, insu_tx_responsavel = @InsuTxResponsavel,
                insu_dt_ult_revisao = @InsuDtUltRevisao, insu_in_desativado = @InsuInDesativado,
                font_sg_fonte_ant = @FontSgFonteAnt, insu_nr_codigo_ant = @InsuNrCodigoAnt,
                insu_dt_ult_coleta = @InsuDtUltColeta, insu_tx_descricao_complementar = @InsuTxDescricaoComplementar
            WHERE font_sg_fonte = @FontSgFonte AND insu_nr_codigo = @InsuNrCodigo";

        await connection.ExecuteAsync(query, insumo);
        return insumo;
    }

    public async Task<bool> DeleteAsync(string fontSgFonte, int insuNrCodigo)
    {
        using var connection = _dbConnection.GetConnection();
        await connection.OpenAsync();

        var query = "DELETE FROM tb_insumo WHERE font_sg_fonte = @fontSgFonte AND insu_nr_codigo = @insuNrCodigo";
        var rowsAffected = await connection.ExecuteAsync(query, new { fontSgFonte, insuNrCodigo });

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

    public async Task<IEnumerable<GrupoInsumo>> GetGruposInsumoAsync()
    {
        using var connection = _dbConnection.GetConnection();
        await connection.OpenAsync();

        var query = @"
            SELECT gpin_nr_codigo, gpin_tx_descricao, gpin_in_ativo, gpin_dt_cadastro
            FROM tb_grupo_insumo 
            WHERE gpin_in_ativo = 'S'
            ORDER BY gpin_tx_descricao";

        return await connection.QueryAsync<GrupoInsumo>(query);
    }
}
