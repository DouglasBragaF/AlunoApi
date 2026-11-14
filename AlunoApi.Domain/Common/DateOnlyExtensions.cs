using System;

public static class DateOnlyExtensions
{
    public static int CalcularIdade(this DateOnly nascimento, DateOnly hoje)
    {
        var idade = hoje.Year - nascimento.Year;
        return nascimento > hoje.AddYears(-idade) ? idade - 1 : idade;
    }
}
