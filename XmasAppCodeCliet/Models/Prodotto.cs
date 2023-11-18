using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace XmasAppCodeClient.Models
{
    public class Prodotto
    {
        public int Id { get; set; }
        public string Barcode { get; set; }
        public string Nome { get; set; }
        public string ImageUrl { get; set; }
        public string Descrizione { get; set; }
        public decimal Prezzo { get; set; }
        public decimal PesoLordo { get; set; }
        public int CategoriaId { get; set; }
    }
}
