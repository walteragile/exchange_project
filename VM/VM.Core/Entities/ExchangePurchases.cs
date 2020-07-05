using System;
using System.ComponentModel.DataAnnotations;

namespace VM.Core.Entities
{
    public class ExchangePurchases
    {
        public int Id { get; set; }

        public int UserId { get; set; }

        [StringLength(3), Required]
        public string Currency { get; set; }

        public DateTime Date { get; set; }

        public decimal Amount { get; set; }
    }
}