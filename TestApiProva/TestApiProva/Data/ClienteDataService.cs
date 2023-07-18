using TestApiProva.Data;
using System.Text.Json;
using TestApiProva.Classi;

namespace TestApiProva.Data
{
    public class ClienteDataService
    {
        private IConfiguration _configuration;
        private string _json;
        public ClienteDataService(IConfiguration configuration)
        {
            _configuration = configuration;
            _json = _configuration.GetValue<string>("JsonPaths:ClientiJson");
        }
        public List<Cliente>? GetAll() 
        {
            string json = "[]";
            if(!File.Exists(_json))
            {
                File.WriteAllText(_json, json);
            }
            else
            {
                json = File.ReadAllText(_json);
            }
            List<Cliente> risultato = JsonSerializer.Deserialize<List<Cliente>>(json) ?? new List<Cliente>();
            return risultato;
        }
        public Cliente? Get(int id)
        {
            List<Cliente> risultato = GetAll();
            return risultato.SingleOrDefault(c => c.ClienteId == id);
        }
        public void Write(Cliente cliente)
        {
            List<Cliente> clienti = GetAll();
            if(cliente == null)
            {
                clienti = new List<Cliente>();
            }
            clienti.Add(cliente);
            string json = JsonSerializer.Serialize(clienti);
            File.WriteAllText(_json, json);
        }
        public void Update(Cliente cliente)
        {
            List<Cliente>? clienti = GetAll();
            Cliente? vecchi = clienti.SingleOrDefault(c => c.ClienteId == cliente.ClienteId);
            if (vecchi == null)
            {
                cliente = new Cliente();
            }
            vecchi.ClienteId = cliente.ClienteId;
            string json = JsonSerializer.Serialize(clienti);
            File.WriteAllText(_json, json);
        }
        public void Delete(Cliente cliente)
        {
            List<Cliente> clienti = GetAll();
            Cliente? vecchi = clienti.SingleOrDefault(C => C.ClienteId == cliente.ClienteId);
            if(vecchi == null)
            {
                cliente = new Cliente();
            }
            clienti.Remove(vecchi);
            string json = JsonSerializer.Serialize(clienti);
            File.WriteAllText(_json, json);
        }
    }
}
