using TestApiProva.Data;
using System.Text.Json;
using TestApiProva.Classi;
using System.Reflection.Metadata.Ecma335;

namespace TestApiProva.Data
{
    public class PagamentoDataService
    {
        private IConfiguration _configuration;
        private string _json;
        public PagamentoDataService(IConfiguration configuration)
        {
            _configuration = configuration;
            _json = _configuration.GetValue<string>("JsonPaths:PagamentiJson");
        }
        public List<Pagamento>? GetAll()
        {
            string json = "[]";
            if (!File.Exists(_json))
            {
                File.WriteAllText(_json, json);
            }
            else
            {
                json = File.ReadAllText(_json);
            }
            List<Pagamento> risultato = JsonSerializer.Deserialize<List<Pagamento>>(json) ?? new List<Pagamento>();
            return risultato;
        }
        public Pagamento? Get(Guid id)
        {
            List<Pagamento> risultato = GetAll();
            return risultato.SingleOrDefault(c => c.FatturaId == id);
        }
        public void Write(Pagamento pagamento)
        {
            List<Pagamento> pagamenti = GetAll();
            if (pagamento == null)
            {
                pagamenti = new List<Pagamento>();
            }
            string json = JsonSerializer.Serialize(pagamenti);
            File.WriteAllText(_json, json);
        }
        public void Update(Pagamento pagamento)
        {
            List<Pagamento>? pagamenti = GetAll();
            Pagamento? vecchi = pagamenti.SingleOrDefault(c => c.FatturaId == pagamento.FatturaId);
            if (vecchi == null)
            {
                pagamento = new Pagamento();
            }
            vecchi.FatturaId = pagamento.FatturaId;
            string json = JsonSerializer.Serialize(pagamenti);
            File.WriteAllText(_json, json);
        }
        public void Delete(Pagamento pagamento)
        {
            List<Pagamento> pagamenti = GetAll();
            Pagamento? vecchi = pagamenti.SingleOrDefault(C => C.FatturaId == pagamento.FatturaId);
            if (vecchi == null)
            {
                pagamento = new Pagamento();
            }
            pagamenti.Remove(vecchi);
            string json = JsonSerializer.Serialize(pagamenti);
            File.WriteAllText(_json, json);
        }
      
    }
}
