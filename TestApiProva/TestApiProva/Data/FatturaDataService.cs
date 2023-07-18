using TestApiProva.Data;
using System.Text.Json;
using TestApiProva.Classi;
using System.Security.Cryptography.X509Certificates;
using Microsoft.AspNetCore.Mvc;

namespace TestApiProva.Data
{
    public class FatturaDataService
    {
        private IConfiguration _configuration;
        private string _json;
        public FatturaDataService(IConfiguration configuration)
        {
            _configuration = configuration;
            _json = _configuration.GetValue<string>("JsonPaths:FattureJson");
        }
        public List<Fattura>? GetAll()
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
            List<Fattura> risultato = JsonSerializer.Deserialize<List<Fattura>>(json) ?? new List<Fattura>();
            return risultato;
        }
        public Fattura? Get(Guid id)
        {
            List<Fattura> risultato = GetAll();
            return risultato.SingleOrDefault(c => c.FatturaId == id);
        }
        public void Write(Fattura fattura)
        {
            List<Fattura> fatture = GetAll();
            if (fattura == null)
            {
                fatture = new List<Fattura>();
            }
            fatture.Add(fattura);
            string json = JsonSerializer.Serialize(fatture);
            File.WriteAllText(_json, json);
        }
        public void Update(Fattura fattura)
        {
            List<Fattura>? fatture = GetAll();
            Fattura? vecchi = fatture.SingleOrDefault(c => c.FatturaId == fattura.FatturaId);
            if (vecchi == null)
            {
                fattura = new Fattura();
            }
            vecchi.ClienteId = fattura.ClienteId;
            string json = JsonSerializer.Serialize(fatture);
            File.WriteAllText(_json, json);
        }
        public void Delete(Fattura fattura)
        {
            List<Fattura> clienti = GetAll();
            Fattura? vecchi = clienti.SingleOrDefault(C => C.FatturaId == fattura.FatturaId);
            if (vecchi == null)
            {
                fattura = new Fattura();
            }
            clienti.Remove(vecchi);
            string json = JsonSerializer.Serialize(clienti);
            File.WriteAllText(_json, json);
        }
    }
}
