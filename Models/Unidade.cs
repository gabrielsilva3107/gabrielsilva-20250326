namespace SISTEMARH_BACKEND.Models
{
    public class Unidade : BaseEntity
    {
        public string Codigo { get; set; }
        public string Nome { get; set; }
        public bool Ativo { get; set; }

        public ICollection<Colaborador>? Colaboradores { get; set; }
    }
}