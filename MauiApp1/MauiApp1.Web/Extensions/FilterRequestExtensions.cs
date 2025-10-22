using System.Web;
using MauiApp1.Web.Models;

namespace MauiApp1.Web.Extensions;

public static class FilterRequestExtensions
{
    public static string ToQueryString(this FilterRequest filter)
    {
        var query = HttpUtility.ParseQueryString(string.Empty);
        
        if (!string.IsNullOrEmpty(filter.SearchTerm))
            query["searchTerm"] = filter.SearchTerm;
            
        query["pageNumber"] = filter.PageNumber.ToString();
        query["pageSize"] = filter.PageSize.ToString();
        
        if (!string.IsNullOrEmpty(filter.SortBy))
            query["sortBy"] = filter.SortBy;
            
        query["sortDescending"] = filter.SortDescending.ToString().ToLower();
        
        return query.ToString() ?? string.Empty;
    }
}