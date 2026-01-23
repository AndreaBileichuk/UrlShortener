using System.Text;

namespace UrlShortener.BLL.Services.UrlShortener;

public static class UrlShortenetAlgorithm
{
    private const string Alphabet = "0123456789abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ";

    public static string GenerateShortCode(int id)
    {
        int baseLen = Alphabet.Length;

        if (id == 0) return Alphabet[0].ToString();
        
        if (id < 0) id = Math.Abs(id);

        var sb = new StringBuilder();

        while (id > 0)
        {
            var remainder = id % baseLen; 
            sb.Insert(0, Alphabet[remainder]);
            id /= baseLen;
        }

        return sb.ToString();
    }
}