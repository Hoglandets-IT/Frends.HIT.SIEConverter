namespace Frends.HIT.SIEConverter;

/// <summary>
/// A class containing functions that can be used in the main class for repeated tasks
/// </summary>
public class Helpers
{
    /// <summary>
    /// Generates MemoryStream from byte array
    /// </summary>
    public static MemoryStream GenerateStreamFromByteArray(byte[] b)
    {
        MemoryStream stream = new MemoryStream(b);
        //var writer = new StreamWriter(stream);
        //writer.Write(b, 0, b.Length);
        //writer.Flush();
        //stream.Position = 0;
        return stream;
    }

    /// <summary>
    /// Truncates a string to specified length
    /// </summary>
    public static string Truncate(string value, int maxLength)
    {
        if (string.IsNullOrEmpty(value)) return value;
        return value.Length <= maxLength ? value : value.Substring(0, maxLength);
    }

}