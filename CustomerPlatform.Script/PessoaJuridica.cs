using Bogus;
using Bogus.Extensions.Brazil;

namespace CustomerPlatform.Script
{
    public static class Dados
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

                    razaoSocial = faker.Company.CompanyName() ,
                    nomeFantasia = faker.Company.CompanySuffix() ,
                    cnpj = GerarCnpjValido()
                };

                lista.Add(registro);
            }

            return lista;
        }

        private static string GerarCnpjValido()
        {
            Random random = new Random();
            int[] multiplicador1 = new int[12] { 5, 4, 3, 2, 9, 8, 7, 6, 5, 4, 3, 2 };
            int[] multiplicador2 = new int[13] { 6, 5, 4, 3, 2, 9, 8, 7, 6, 5, 4, 3, 2 };

            // Gera os 8 primeiros dígitos aleatórios
            string semente = random.Next(10000000, 99999999).ToString();
            // Adiciona o sufixo padrão de matriz (0001)
            semente += "0001";

            // Calcula o primeiro dígito verificador
            int soma = 0;
            for (int i = 0; i < 12; i++)
                soma += int.Parse(semente[i].ToString()) * multiplicador1[i];

            int resto = soma % 11;
            int dg1 = resto < 2 ? 0 : 11 - resto;

            semente += dg1;

            // Calcula o segundo dígito verificador
            soma = 0;
            for (int i = 0; i < 13; i++)
                soma += int.Parse(semente[i].ToString()) * multiplicador2[i];

            resto = soma % 11;
            int dg2 = resto < 2 ? 0 : 11 - resto;

            return semente + dg2;
        }
    }
}
