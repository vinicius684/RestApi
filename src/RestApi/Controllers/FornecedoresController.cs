using AutoMapper;
using DevIO.Api.ViewModels;
using DevIO.Business.Intefaces;
using DevIO.Data.Repository;
using Microsoft.AspNetCore.Mvc;

namespace RestApi.Controllers
{
    [Route("api/fornecedores")]
    public class FornecedoresController : MainController
    {
        private readonly IFornecedorRepository _fornecedorRepository;
        private readonly IProdutoRepository _produtoRepository;
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
            //O Parametro é uma lista de model Fornecedor, para isso é utilizado o Automapper para Maper para as ViewModels
            var fornecedor = _mapper.Map<IEnumerable<FornecedorViewModel>>(await _fornecedorRepository.ObterTodos());

            foreach (var fornecedorViewModel in fornecedor)
            {
                // Obtenha os produtos associados a cada fornecedor
                var produtosDoFornecedor = await _produtoRepository.ObterProdutosPorFornecedor(fornecedorViewModel.Id);

                // Atribua os produtos ao fornecedor no ViewModel
                fornecedorViewModel.Produtos = _mapper.Map<IEnumerable<ProdutoViewModel>>(produtosDoFornecedor);
            }

            return fornecedor;
        }

    }
}