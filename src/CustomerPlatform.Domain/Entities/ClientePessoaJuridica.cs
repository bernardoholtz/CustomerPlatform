namespace CustomerPlatform.Domain.Entities;

/// <summary>
/// Cliente Pessoa Jurídica
/// </summary>
public class ClientePessoaJuridica : Customer
{
    public string RazaoSocial { get; }
    public string NomeFantasia { get; }
    public string CNPJ { get; }

    protected ClientePessoaJuridica() { }

    public ClientePessoaJuridica(
        Guid id,
        string razaoSocial,
        string nomeFantasia,
        string cnpj,
        string email,
        string telefone,
        Endereco endereco)
        : base(id, email, telefone, endereco)
    {
        RazaoSocial = razaoSocial;
        NomeFantasia = nomeFantasia;
        CNPJ = cnpj;

        if (!ValidarDocumento())
            throw new ArgumentException("CNPJ inválido");
    }

    public override string GetDocumento() => CNPJ;
    public override string GetNome() => RazaoSocial;

    public override bool ValidarDocumento()
    {
        var cnpj = CNPJ.Replace(".", "").Replace("-", "").Replace("/", "");

        if (cnpj.Length != 14)
            return false;

        if (new string(cnpj[0], cnpj.Length) == cnpj)
            return false;

        int[] peso1 = { 5, 4, 3, 2, 9, 8, 7, 6, 5, 4, 3, 2 };
        int[] peso2 = { 6, 5, 4, 3, 2, 9, 8, 7, 6, 5, 4, 3, 2 };

        int soma = 0;
        for (int i = 0; i < 12; i++)
            soma += (cnpj[i] - '0') * peso1[i];

        int resto = soma % 11;
        int digito1 = resto < 2 ? 0 : 11 - resto;

        soma = 0;
        for (int i = 0; i < 13; i++)
            soma += (cnpj[i] - '0') * peso2[i];

        resto = soma % 11;
        int digito2 = resto < 2 ? 0 : 11 - resto;

        return cnpj[12] - '0' == digito1 && cnpj[13] - '0' == digito2;
    }
}
