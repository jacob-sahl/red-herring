using System;
using System.Text;
using Newtonsoft.Json;

public class CardURLGenerator
{
    private const string BaseURL = "https://jacob-sahl.github.io/red-herring/?r=";

    public static string GetCardURL(string background, string foreground, string clue, string secretObjective)
    {
        var card = new
        {
            b = background,
            f = foreground,
            c = clue,
            o = secretObjective
        };

        // convert the json object to a string
        var json = JsonConvert.SerializeObject(card);

        // convert the string to a byte array
        var bytes = Encoding.UTF8.GetBytes(json);

        // convert the byte array to a base64 string
        var base64 = Convert.ToBase64String(bytes);

        // return the base64 string
        return BaseURL + base64;
    }
}