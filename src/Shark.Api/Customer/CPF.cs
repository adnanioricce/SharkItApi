namespace Shark.Domain;

public readonly record struct CPF
{
    private readonly string _cpf;

    private CPF(string cpf)
    {
        _cpf = cpf;
    }    

    public string FormattedString => $"{_cpf.Substring(0, 3)}.{_cpf.Substring(3, 3)}.{_cpf.Substring(6, 3)}-{_cpf.Substring(9, 2)}";

    public override string ToString() => _cpf;

    private static bool ValidateCPF(string cpf)
    {        
        var digitOnlyCpf = new string(cpf.Where(char.IsDigit).ToArray());

        var digits = digitOnlyCpf.Select(c => int.Parse(c.ToString())).ToArray();

        if (digitOnlyCpf.Length != 11)
        {
            return false;
        }

        int firtSum = Enumerable.Range(0, 9).Select(i => digits[i] * (10 - i)).Sum();            
        var firtRemainder = (firtSum % 11);
        int verificationDigit1 = (firtSum % 11 < 2) ? 0 : 11 - firtRemainder;
        
        var secondSum = Enumerable.Range(0, 10).Select(i => digits[i] * (11 - i)).Sum();
        var secondRemainder = (secondSum % 11);
        int verificationDigit2 = (secondSum % 11 < 2) ? 0 : 11 - secondRemainder;

        var verificationDigits = digits.TakeLast(2).ToArray();
        
        return verificationDigits[0] == verificationDigit1 && verificationDigits[1] == verificationDigit2;
    }
    public static CPF Create(string cpf)
    {
        if (!ValidateCPF(cpf ?? ""))
        {
            throw new ArgumentException("Invalid CPF.");
        }

        return new CPF(cpf);
    }
}


