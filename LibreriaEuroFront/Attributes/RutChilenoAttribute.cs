using System.ComponentModel.DataAnnotations;
using System.Text.RegularExpressions;

public class RutChilenoAttribute : ValidationAttribute
{
    public override bool IsValid(object? value)
    {
        if (value is not string rut)
            return false;

        rut = rut.Replace(".", "").Replace("-", "").ToUpper();

        if (rut.Length < 2)
            return false;

        var cuerpo = rut[..^1];
        var dvIngresado = rut[^1].ToString();

        if (!int.TryParse(cuerpo, out var rutNumerico))
            return false;

        var dvCalculado = CalcularDigitoVerificador(rutNumerico);

        return dvIngresado == dvCalculado;
    }

    private string CalcularDigitoVerificador(int rut)
    {
        int suma = 0;
        int multiplicador = 2;

        while (rut > 0)
        {
            int digito = rut % 10;
            suma += digito * multiplicador;
            rut /= 10;
            multiplicador = multiplicador == 7 ? 2 : multiplicador + 1;
        }

        int resto = 11 - (suma % 11);
        if (resto == 11) return "0";
        if (resto == 10) return "K";
        return resto.ToString();
    }
}
