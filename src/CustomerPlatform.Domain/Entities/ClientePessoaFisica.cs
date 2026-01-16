namespace CustomerPlatform.Domain.Entities;

/// <summary>
/// Cliente Pessoa Física
/// </summary>
public class ClientePessoaFisica : Customer
{
    public string Nome { get; }
    public string CPF { get; }
    public DateTime DataNascimento { get; }

    protected ClientePessoaFisica() { }
    public ClientePessoaFisica(
        Guid id,
        string nome,
        string cpf,
        DateTime dataNascimento,
        string email,
        string telefone,
        Endereco endereco)
        : base(id, email, telefone, endereco)
    {
        Nome = nome;
        CPF = cpf;
        DataNascimento = dataNascimento;

        if (!ValidarDocumento())
            throw new ArgumentException("CPF inválido");
    }

    public override string GetDocumento() => CPF;
    public override string GetNome() => Nome;

    public override bool ValidarDocumento()
    {
        var cpf = CPF.Replace(".", "").Replace("-", "");

        if (cpf.Length != 11)
            return false;

        if (new string(cpf[0], cpf.Length) == cpf)
            return false;

        int soma = 0;
        for (int i = 0; i < 9; i++)
            soma += (cpf[i] - '0') * (10 - i);

        int resto = soma % 11;
        int digito1 = resto < 2 ? 0 : 11 - resto;

        soma = 0;
        for (int i = 0; i < 10; i++)
            soma += (cpf[i] - '0') * (11 - i);

        resto = soma % 11;
        int digito2 = resto < 2 ? 0 : 11 - resto;

        return cpf[9] - '0' == digito1 && cpf[10] - '0' == digito2;
    }
}
