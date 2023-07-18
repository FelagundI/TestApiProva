using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using TestApiProva.Classi;
using TestApiProva.Data;

namespace TestApiProva.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PagamentoController : ControllerBase
    {
        private ILogger<PagamentoController> _logger;
        private readonly PagamentoDataService _dataService;
        private readonly ClienteDataService _clienteDataService;
        public PagamentoController(ILogger<PagamentoController> logger, PagamentoDataService dataService, ClienteDataService clienteDataService)
        {
            _logger = logger;
            _dataService = dataService;
            _clienteDataService = clienteDataService;
        }
        [HttpGet]
        [Route("api/Pagamenti")]
        public IActionResult GetAll()
        {
            _logger.LogInformation($"Get Pagamento/GetAll");
            return Ok(_dataService.GetAll());
        }
        [HttpGet]
        [Route("api/Pagamenti/PagamentoID/{id}")]
        public IActionResult Get(Guid pagamentoId)
        {
            _logger.LogInformation($"Get Pagamento/{pagamentoId}");
            Pagamento? cli = _dataService.GetAll().SingleOrDefault(c => c.PagamentoId == pagamentoId);
            if (cli == null)
            {
                _logger.LogWarning($"{pagamentoId} non trovato");
                return BadRequest();
            }
            return Ok(cli);
        }
        [HttpPost]
        public IActionResult Post(Pagamento pagamento)
        {
            _logger.LogInformation($"Post Pagamento {pagamento.PagamentoId}");
            if (pagamento.PagamentoId == null)
            {
                pagamento = new Pagamento(pagamento.PagamentoId);
            }
            _dataService.Write(pagamento);
            return Ok();
        }
        [HttpPut]
        public IActionResult Put(Pagamento pagamento)
        {
            _logger.LogInformation($"Put Pagamento {pagamento.PagamentoId}");
            Cliente? pag = new Cliente();
            if (pag == null)
            {
                _logger.LogWarning($"{pagamento.PagamentoId} non trovato");
                return BadRequest();
            }
            _dataService.Update(pagamento);
            return Ok(pag);
        }
        [HttpDelete]
        public IActionResult Delete(Pagamento pagamento)
        {
            _logger.LogInformation($"Delete Cliente/{pagamento.PagamentoId}");
            Pagamento? pag = new Pagamento();
            if (pag == null)
            {
                _logger.LogWarning($"{pagamento.PagamentoId} non trovato");
                return BadRequest();
            }
            if (pag.PagamentoId == pagamento.PagamentoId)
            {
                _dataService.Delete(pagamento);
            }
            return BadRequest();
        }
        [HttpGet]
        [Route("api/Pagamenti/ListaPagamenti/")]
        public IActionResult GetList(int clienteId)
        {
            _logger.LogInformation("Lista Pagamenti Filtrata Per Cliente");
            List<Pagamento>? listaPagamenti = _dataService.GetAll();
            List<Cliente>? listaClienti = _clienteDataService.GetAll().FindAll(c => c.ClienteId == clienteId);
            List<Pagamento>? listaPerClienti = (from s
                                                in listaPagamenti 
                                                join cli
                                                in listaClienti
                                                on s.DettagliCliente.ClienteId
                                                equals cli.ClienteId
                                                select s).Distinct().ToList();

            if(listaPagamenti == null)
            {
                _logger.LogWarning("Lista non presente");
                return BadRequest();
            }
            listaPagamenti.ForEach(c => Console.WriteLine($"ID Cliente: {c.DettagliCliente.ClienteId} - ID Pagamento{c.PagamentoId}"));
            return Ok(listaPagamenti);
        }
        
    }
}
