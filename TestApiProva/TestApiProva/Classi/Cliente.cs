
namespace TestApiProva.Classi
{
    public class Cliente
    {
        private int _seed = 0;
        public int ClienteId { get; set; }
        public string RagioneSociale { get; set; }
        public int Indirizzo { get; set; }
        public string PartitaIva { get; set; }
        public string Note { get; set; }
        public Fattura Fatture { get; set; }
        public Cliente()
        {

        }
        public Cliente(string ragioneSociale)
        {
            ClienteId = _seed++;
            RagioneSociale = ragioneSociale;
        }
        public Cliente(int clienteId, string ragioneSociale, int indirizzo, string partitaIva, string note, Fattura fatture)
        {
            clienteId = _seed++;
            ragioneSociale = RagioneSociale;
            indirizzo = Indirizzo;
            partitaIva = PartitaIva;
            note = Note;
            fatture = Fatture;
        }
    }
}
