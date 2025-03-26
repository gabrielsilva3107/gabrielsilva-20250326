namespace SISTEMARH_BACKEND.Models
{
    public class Usuario : BaseEntity
    {
        public string Login { get; set; }
        public string Senha { get; set; }
        public bool Ativo { get; set; }

        public Colaborador? Colaborador { get; set; }
    }
}