using DevIO.Business.Intefaces;
using DevIO.Business.Models;
using DevIO.Business.Notificacoes;
using FluentValidation;
using FluentValidation.Results;

namespace DevIO.Business.Services
{
    public abstract class BaseService
    {
        private readonly INotificador _notificador;

        protected BaseService(INotificador notificador)
        {
            _notificador = notificador;
        }

        protected void Notificar(ValidationResult validationResult)
        {
            foreach (var error in validationResult.Errors)
            {
                Notificar(error.ErrorMessage);
            }
        }

        protected void Notificar(string mensagem)//sobrescrita
        {
            _notificador.Handle(new Notificacao(mensagem));
        }

        //Mais Importante
        protected bool ExecutarValidacao<TV, TE>(TV validacao, TE entidade) where TV : AbstractValidator<TE> where TE : Entity
        {
            var validator = validacao.Validate(entidade);//Método do FluentValidation, retornando um ValidateResult

            if(validator.IsValid) return true;

            Notificar(validator);//Serie de métodos para tranformar o ValidateResult em String e Adicionar em uma lista que posso acessa-lá depois

            return false;
        }
    }
}