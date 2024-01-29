namespace DevIO.Business.Models
{
    public abstract class Entity
    {//Todas as models Herdam dessa class / Na hora que instancia uma das Models, Herda o atributo Id Inicializado pelo construtor
        protected Entity()
        {
            Id = Guid.NewGuid();
        }

        public Guid Id { get; set; }
    }
}