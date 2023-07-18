namespace TestApiProva.Classi
{
    public class Pagamento
    {
        public Guid PagamentoId { get; set; }
        public Guid FatturaId { get; set; }
        public TipologiaPagamento TipologiaPagamenti { get; set; }
        public double Ammontare { get; set; }
        public DateTime DataPagamento { get; set; }
        public Cliente DettagliCliente { get; set; }
        public Pagamento() 
        {
        
        }
        public Pagamento(Guid pagamentoId)
        {
            pagamentoId = PagamentoId;
        }
        public Pagamento(Guid pagamentoId, Guid fatturaId, TipologiaPagamento tipologiaPagamenti, double ammontare, DateTime dataPagamento, Cliente dettagliCliente)
        {
            pagamentoId = new Guid();
            fatturaId = FatturaId;
            tipologiaPagamenti = TipologiaPagamenti;
            ammontare = Ammontare;
            dataPagamento = DataPagamento;
            dettagliCliente = DettagliCliente;
        }
    }
    public enum TipologiaPagamento
    {
        Saldo,
        Acconto
    }
}
