using System.Collections.Generic;

namespace APIAlturas.ViewModels
{
    public class RootResult
    {
        public int TotalPage { get; set; }
        public IEnumerable<object> Results { get; set; }
        public IEnumerable<object> Extras { get; set; }
    }
}