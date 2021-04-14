using System.Collections.Generic;

namespace FunctionApp.Bussines.Models {
    public sealed class PageResult<TItem> {
        public int Count { get; set; }
        public IEnumerable<TItem> Data { get; set; }
    }
}