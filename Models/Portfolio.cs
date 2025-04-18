using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace api.Models
{
    // Portfolio is a many to many join table
    [Table("Portfolios")]
    public class Portfolio
    {
        public string AppUserId { get; set; }
        public int StockId { get; set; }

        // Navigation
        public AppUser AppUser { get; set; }
        public Stock Stock { get; set; }
    }
}