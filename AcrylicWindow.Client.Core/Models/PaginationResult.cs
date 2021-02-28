using System.Collections.Generic;

namespace AcrylicWindow.Client.Core.Models
{
    public class PaginationResult<TModel>
    {
        public IEnumerable<TModel> Values { get; set; }

        public long TotalCount { get; set; }
    }
}
