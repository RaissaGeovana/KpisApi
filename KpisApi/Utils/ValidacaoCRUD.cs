namespace KpisApi.Utils
{
    public class ValidacaoCRUD
    {
        public static bool IsNome(string nome)
        {
            if (nome.Length >= 5
                && nome.Length <= 255)
            {
                return true;
            }
            return false;
        }

        public static bool IsUnidadeMedida(string unidadeMedida)
        {
            if (unidadeMedida.Length >= 1
                && unidadeMedida.Length <= 255)
            {
                return true;
            }
            return false;
        }
    }

}
