using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using TestApiProva.Classi;
using TestApiProva.Data;

namespace TestApiProva.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ClienteController : ControllerBase
    {
        private ILogger<ClienteController> _logger;
        private readonly ClienteDataService _dataService;
        private readonly PagamentoDataService _pagamentoDataService;

        public ClienteController(ILogger<ClienteController> logger, ClienteDataService dataService, PagamentoDataService pagamentoDataService)
        {
            _logger = logger;
            _dataService = dataService;
            _pagamentoDataService = pagamentoDataService;
        }
        [HttpGet]
        [Route("api/Clienti")]
        public IActionResult GetAll() 
        {
            _logger.LogInformation($"Get Cliente/GetAll");
            return Ok(_dataService.GetAll());
        }
        [HttpGet]
        [Route("api/Clienti/ClienteID/{id}")]
        public IActionResult Get(int clienteId)
        {
            _logger.LogInformation($"Get Cliente/{clienteId}");
            Cliente? cli = _dataService.GetAll().SingleOrDefault(c => c.ClienteId == clienteId);
            if(cli == null)
            {
                _logger.LogWarning($"{clienteId} non trovato");
                return BadRequest();
            }
            return Ok(cli);
        }
        [HttpPost]
        public IActionResult Post(Cliente cliente)
        {
            _logger.LogInformation($"Post Cliente/{cliente.ClienteId}");
            if(cliente.ClienteId == 0)
            {
                cliente = new Cliente(cliente.RagioneSociale);
            }
            _dataService.Write(cliente);
            return Ok();
        }
        [HttpPut]
        public IActionResult Put(Cliente cliente)
        {
            _logger.LogInformation($"Put Cliente/{cliente.ClienteId}");
            Cliente? cli = new Cliente();
            if(cli == null)
            {
                _logger.LogWarning($"{cliente.ClienteId} non trovato");
                return BadRequest();
            }
            _dataService.Update(cliente);
            return Ok(cli);
        }
        [HttpDelete]
        public IActionResult Delete(Cliente cliente)
        {
            _logger.LogInformation($"Delete Cliente/{cliente.ClienteId}");
            Cliente? cli = new Cliente();
            if(cli == null)
            {
                _logger.LogWarning($"{cliente.ClienteId} non trovato");
                return BadRequest();
            }
            if (cli.ClienteId == cliente.ClienteId)
            {
                _dataService.Delete(cliente);
            }
            return BadRequest();
        }
        [HttpGet]
        [Route("api/Clienti/ListaClientiRitardatari")]
        public IActionResult Get()
        {
            _logger.LogInformation("Lista Clienti Ritardatari/");
            List<Cliente>? listaCliente = _dataService.GetAll();
            List<Pagamento>? listaPagamento = _pagamentoDataService.GetAll().FindAll(c => c.TipologiaPagamenti == TipologiaPagamento.Acconto);
            List<Cliente> listaRitardatari = (from s
                                              in listaCliente
                                              join pag
                                              in listaPagamento
                                              on s.ClienteId
                                              equals pag.DettagliCliente.ClienteId
                                              select s).Distinct().ToList();

            if (listaRitardatari == null)
            {
                _logger.LogWarning("Lista non presente");
                return BadRequest();
            }
            listaRitardatari.ForEach(c => Console.WriteLine($"ID cliente in ritardo: {c.ClienteId}"));
            return Ok(listaRitardatari);
        }
    }
}
