using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hutech.Core.Entities
{
    public class MachineDetail
    {
        public long Id { get; set; }
        public string Name { get; set; }
        public DateTime? PurchaseDate { get; set; }
        public decimal Price { get; set; }
        public string Brand { get; set; }
        public string Vednor { get; set; }
        public int Warranty { get; set; }
        public bool IsActive { get; set; }  
        public string? Interval { get; set; }
        public int ServiceInterval { get; set; }
    }
}
