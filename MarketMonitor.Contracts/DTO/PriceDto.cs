using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MarketMonitor.Contracts.DTO
{
    public class PriceDto
    {
        public Guid Id { get; set; }
        public DateTime Time { get; set; }
        public decimal Price { get; set; }
        public int Volume { get; set; }
        public decimal Change { get; set; }
        public decimal ChangePercents { get; set; }
        public string Kind { get; set; }
        public string Type { get; set; }
        public string Provider { get; set; }
        public Guid InstrumentId { get; set; }
    }
}
