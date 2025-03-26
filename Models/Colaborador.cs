namespace SISTEMARH_BACKEND.Models
{
    public class Colaborador : BaseEntity
    {
        public string Nome { get; set; }

        public int UnidadeId { get; set; }
        public Unidade Unidade { get; set; }

        public int UsuarioId { get; set; }
        public Usuario Usuario { get; set; }
    }
}
