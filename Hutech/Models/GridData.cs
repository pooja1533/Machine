using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hutech.Models
{
    public class GridData<T>
    {
        public GridData() { }
        public List<T> GridRecords { get; set; }
        public int TotalRecords { get; set; }
        public int TotalPages { get; set; }
        public int CurrentPage { get; set; }
    }
}
