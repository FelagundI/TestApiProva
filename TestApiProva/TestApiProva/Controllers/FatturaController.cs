using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using TestApiProva.Classi;
using TestApiProva.Data;

namespace TestApiProva.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class FatturaController : ControllerBase
    {
        private ILogger<FatturaController> _logger;
        private readonly FatturaDataService _dataService;
        private readonly PagamentoDataService _pagamentoDataService;
        public FatturaController(ILogger<FatturaController> logger, FatturaDataService dataService, PagamentoDataService pagamentoDataService)
        {
            _logger = logger;
            _dataService = dataService;
            _pagamentoDataService = pagamentoDataService;
        }
        [HttpGet]
        [Route("api/Fatture")]
        public IActionResult GetAll()
        {
            _logger.LogInformation($"Get Fatture/GetAll");
            return Ok(_dataService.GetAll());
        }
        [HttpGet]
        [Route("api/Fatture/FatturaID/{id}")]
        public IActionResult Get(Guid fatturaId)
        {
            _logger.LogInformation($"Get Fattura/{fatturaId}");
            Fattura? fat = _dataService.GetAll().SingleOrDefault(c => c.FatturaId == fatturaId);
            if (fat == null)
            {
                _logger.LogWarning($"{fatturaId} non trovato");
                return BadRequest();
            }
            return Ok(fat);
        }
        [HttpPost]
        public IActionResult Post(Fattura fattura)
        {
            _logger.LogInformation($"Post Fattura/{fattura.FatturaId}");
            if (fattura.FatturaId == null)
            {
                fattura = new Fattura(fattura.FatturaId);
            }
            _dataService.Write(fattura);
            return Ok();
        }
        [HttpPut]
        public IActionResult Put(Fattura fattura)
        {
            _logger.LogInformation($"Put Fattuera/{fattura.FatturaId}");
            Fattura? fat = new Fattura();
            if (fat == null)
            {
                _logger.LogWarning($"{fattura.FatturaId} non trovato");
                return BadRequest();
            }
            _dataService.Update(fattura);
            return Ok(fat);
        }
        [HttpDelete]
        public IActionResult Delete(Fattura fattura)
        {
            _logger.LogInformation($"Delete Fattura/{fattura.FatturaId}");
            Fattura? fat = new Fattura();
            if (fat == null)
            {
                _logger.LogWarning($"{fattura.FatturaId} non trovato");
                return BadRequest();
            }
            if (fat.FatturaId == fattura.FatturaId)
            {
                _dataService.Delete(fattura);
            }
            return BadRequest();
        }
        
        [HttpGet]
        [Route("api/Fatture/ListaFatturePerCliente/")]
        public IActionResult GetList(int clienteId)
        {
            _logger.LogInformation("Lista Fatture Filtrata Per Cliente/");
            List<Fattura>? ordineFatture = (from s
                                            in _dataService.GetAll()
                                            where s.ClienteId == clienteId
                                            select s).ToList();
            if (ordineFatture == null)
            {
                _logger.LogWarning("Lista non presente");
                return BadRequest();
            }
            ordineFatture.ForEach(c => Console.WriteLine($"Cliente ID: {c.ClienteId} - Fattura ID: {c.FatturaId}"));
            return Ok(ordineFatture);
        }
        [HttpGet]
        [Route("api/Fatture/ListaFattureRitardoPagamento/")]
        public IActionResult GetListRit()
        {
            _logger.LogInformation("Lista Fatture Pagate In Ritardo");
            List<Fattura>? ordineFatture = _dataService.GetAll();
            List<Pagamento>? ordinePagamenti = _pagamentoDataService.GetAll().FindAll(c => c.TipologiaPagamenti == TipologiaPagamento.Saldo);
            List<Fattura> pagamentoRitardo = (from s
                                              in ordineFatture
                                              join pag
                                              in ordinePagamenti
                                              on s.FatturaId
                                              equals pag.FatturaId
                                              where s.DataTermine < pag.DataPagamento
                                              select s).Distinct().ToList();

            if(pagamentoRitardo == null)
            {
                _logger.LogWarning("Lista non presente");
                return BadRequest();
            }
            pagamentoRitardo.ForEach(c => Console.WriteLine($"ID fattura da pagare :{c.FatturaId}, ID Cliente: {c.ClienteId}"));
            return Ok(pagamentoRitardo);
        }
    }
}
