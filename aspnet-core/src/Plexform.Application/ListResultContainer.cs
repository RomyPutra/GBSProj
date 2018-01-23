using System.Collections.Generic;

namespace Plexform
{
    public class ListResultContainer<T>
    {
        public ListResultContainer(IEnumerable<T> items, int totalCount) {
            this.Items = items;
            this.TotalCount = totalCount;
        }

        public IEnumerable<T> Items { get; set; }
        public int TotalCount { get; set; }
    }
}