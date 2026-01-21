using Bogus;

namespace CustomerPlatform.Script
{
    public static class DadosFakePessoaFisica
    {
        public static List<object> GerarMassaDeDados(int quantidade)
        {
            var faker = new Faker("pt_BR");
            var lista = new List<object>();

            for (int i = 0; i < quantidade; i++)
            {
                var registro = new
                {
                    email = faker.Internet.Email().ToLower(),
                    telefone = faker.Phone.PhoneNumber("###########"), // Formato: 21991899023
                    endereco = new
                    {
                        logradouro = faker.Address.StreetName(),
                        numero = faker.Address.BuildingNumber(),
                        complemento = faker.Address.SecondaryAddress(),
                        cep = faker.Address.ZipCode("#####-###"),
                        cidade = faker.Address.City(),
                        estado = faker.Address.StateAbbr()
                    },

                    nome = faker.Name.FullName() ,
                    cpf = GerarCpfValido(),
                    dataNascimento =  faker.Date.Past(30, DateTime.Now.AddYears(-18)) 

                };

                lista.Add(registro);
            }

            return lista;
        }

        private static string GerarCpfValido()
        {
            Random random = new Random();
            int[] multiplicador1 = new int[9] { 10, 9, 8, 7, 6, 5, 4, 3, 2 };
            int[] multiplicador2 = new int[10] { 11, 10, 9, 8, 7, 6, 5, 4, 3, 2 };

            string semente = random.Next(100000000, 999999999).ToString();

            // Calcula primeiro dígito
            int soma = 0;
            for (int i = 0; i < 9; i++)
                soma += int.Parse(semente[i].ToString()) * multiplicador1[i];

            int resto = soma % 11;
            int dg1 = resto < 2 ? 0 : 11 - resto;

            semente += dg1;

            // Calcula segundo dígito
            soma = 0;
            for (int i = 0; i < 10; i++)
                soma += int.Parse(semente[i].ToString()) * multiplicador2[i];

            resto = soma % 11;
            int dg2 = resto < 2 ? 0 : 11 - resto;

            return semente + dg2;
        }
    }
}
