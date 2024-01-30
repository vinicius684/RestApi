using AutoMapper;
using DevIO.Api.ViewModels;
using DevIO.Business.Intefaces;
using DevIO.Business.Models;
using DevIO.Business.Services;
using DevIO.Data.Repository;
using Microsoft.AspNetCore.Mvc;

namespace RestApi.Controllers
{
    [Route("api/fornecedores")]
    public class FornecedoresController : MainController
    {
        private readonly IFornecedorRepository _fornecedorRepository;
        private readonly IProdutoRepository _produtoRepository;
        private IFornecedorService _fornecedorService;
        private readonly IMapper _mapper;

        public FornecedoresController(IFornecedorRepository fornecedorrepository, IProdutoRepository produtoRepository, IMapper mapper)
        {
            _fornecedorRepository = fornecedorrepository;
            _mapper = mapper;
            _produtoRepository = produtoRepository;
        }


        [HttpGet]
        public async Task<IEnumerable<FornecedorViewModel>> ObterTodos()
        {
            //O Parametro é uma lista de model Fornecedor, é utilizado o Automapper para Maper para as ViewModels
            var fornecedor = _mapper.Map<IEnumerable<FornecedorViewModel>>(await _fornecedorRepository.ObterTodos());
          
            return fornecedor;
        }

        [HttpGet("{id:guid}")]
        public async Task<ActionResult<FornecedorViewModel>> ObterPorId(Guid id)
        {
            var fornecedorViewModel = await ObterFornecedorProdutosEndereco(id);

            if (fornecedorViewModel == null) return NotFound();

            return fornecedorViewModel;
        }

        [HttpPost]
        public async Task<ActionResult<FornecedorViewModel>> Adicionar(FornecedorViewModel fornecedorViewModel)
        {
            if (!ModelState.IsValid) return BadRequest();

            var fornecedor = _mapper.Map<Fornecedor>(fornecedorViewModel);
            var result = await _fornecedorService.Adicionar(fornecedor);//Serviço de Validação + Post

            if (!result) return BadRequest();

            return Ok(fornecedorViewModel);
        }

        [HttpPut("{id:guid}")]
        public async Task<ActionResult<FornecedorViewModel>> Atualizar(Guid id, FornecedorViewModel fornecedorViewModel)
        {
            if (id != fornecedorViewModel.Id)
            {
                return BadRequest();
            }

            if (!ModelState.IsValid) return BadRequest();

            var fornecedor = _mapper.Map<Fornecedor>(fornecedorViewModel);//Mapeando FornecedorViewModel Recebido para Model
            var result = await _fornecedorService.Atualizar(fornecedor);//Serviço de Validação + Post

            if (!result) return BadRequest();

            return Ok(fornecedorViewModel);
        }

        [HttpDelete("{id:guid}")]
        public async Task<ActionResult<FornecedorViewModel>> Excluir(Guid id)
        {
            var fornecedorViewModel = await ObterFornecedorEndereco(id);

            if (fornecedorViewModel == null) return NotFound();

            var result = await _fornecedorService.Remover(id);//Serviço de validação + Remove (não pode excluir se tiver produto)

            if (!result) return BadRequest();

            return Ok(fornecedorViewModel);
        }

        //[HttpGet("obter-endereco/{id:guid}")]
        //public async Task<EnderecoViewModel> ObterEnderecoPorId(Guid id)
        //{
        //    return _mapper.Map<EnderecoViewModel>(await _enderecoRepository.ObterPorId(id));
        //}

        //[HttpPut("atualizar-endereco/{id:guid}")]
        //public async Task<IActionResult> AtualizarEndereco(Guid id, EnderecoViewModel enderecoViewModel)
        //{
        //    if (id != enderecoViewModel.Id)
        //    {
        //        NotificarErro("O id informado não é o mesmo que foi passado na query");
        //        return CustomResponse(enderecoViewModel);
        //    }

        //    if (!ModelState.IsValid) return CustomResponse(ModelState);

        //    await _fornecedorService.AtualizarEndereco(_mapper.Map<Endereco>(enderecoViewModel));

        //    return CustomResponse(enderecoViewModel);
        //}
        [ApiExplorerSettings(IgnoreApi = true)]
        public async Task<FornecedorViewModel> ObterFornecedorProdutosEndereco(Guid id)
        {
            return _mapper.Map<FornecedorViewModel>(await _fornecedorRepository.ObterFornecedorProdutosEndereco(id));
        }

        [ApiExplorerSettings(IgnoreApi = true)]
        public async Task<FornecedorViewModel> ObterFornecedorEndereco(Guid id)
        {
            return _mapper.Map<FornecedorViewModel>(await _fornecedorRepository.ObterFornecedorEndereco(id));
        }
    }
}