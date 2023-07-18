using Microsoft.AspNetCore.Mvc.RazorPages;
using System.ComponentModel.DataAnnotations;
using System.Security.Cryptography.X509Certificates;

namespace TestApiProva.Classi
{
    public class Fattura
    {
        public Guid FatturaId { get; set; }
        private int _seed = 1;
        public int AnnoFattura = DateTime.Now.Year;
        public int CodiceFattura { get; set; }

        public Cliente Cliente { get; set; }
        public int ClienteId { get; set; }
        public double TotaleNetto { get; set; }
        public Iva Iva { get; set; }
        public string IvaPercentuale()
        {
            return string.Format($"{Iva}%");
        }
        public double TotaleLordo()
        {
            return TotaleNetto * (1 + (int)Iva / 100);
        }
        public DateTime DataEmissione { get; set; }
        public DateTime DataTermine { get; set; }
        public string Note { get; set; }
        public Fattura()
        {

        }
        public Fattura(Guid fatturaId)
        {
            fatturaId = FatturaId;
        }
        public Fattura(Guid id, string codiceFattura, int clienteId, double totaleNetto, string iva, DateTime dataEmissione, DateTime dataTermine, string note)
        {
            id = new Guid();
            codiceFattura = CodiceCompleto();
            clienteId = ClienteId;
            totaleNetto = TotaleNetto;
            iva = IvaPercentuale();
            dataEmissione = DataEmissione;
            dataTermine = DataEmissione.AddDays(30);
            note = Note;
        }
        public string CodiceCompleto()
        {
            return _seed++.ToString()+"/"+AnnoFattura.ToString();
        }     
    }
    public enum Iva
    {
        Ordinaria = 22,
        Alimentari = 4,
        Medicali = 5,
        BeniSignificativi = 10
    }
}
