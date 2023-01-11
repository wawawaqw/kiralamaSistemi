using Newtonsoft.Json;

namespace kiralamaSistemi.API.Model
{
    public class ReqDataTable
    {

        [JsonProperty("pageSize")]
        public int? PageSize { get; set; }
        [JsonProperty("currentPage")]
        public int? CurrentPage { get; set; }
        [JsonProperty("search")]
        public string? Search { get; set; }
        [JsonProperty("sortField")]
        public string? SortColumn { get; set; }
        [JsonProperty("sortOrder")]
        public string? SortColumnDir { get; set; }
        public int? Skip { get => PageSize * (CurrentPage - 1); }
    }
}
