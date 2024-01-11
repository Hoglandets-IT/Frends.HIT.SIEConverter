using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace Frends.HIT.SIEConverter;

/// <summary>
/// Parameters for parsing SIE file
/// </summary>
[DisplayName("Parameters")]
public class ParseParams
{
    /// <summary>
    /// The source SIE file as a byte array
    /// </summary>
    [DefaultValue("")]
    [DisplayFormat(DataFormatString = "Text")]
    public byte[] File { get; set; }

    /// <summary>
    /// If true the parser will not flag a missing #OMFATTN as an error
    /// </summary>
    //[DisplayFormat(DataFormatString = "Boolean")]
    [DefaultValue(false)]
    public bool IgnoreMissingOMFATTNING { get; set; }

    /// <summary>
    /// If true #BTRANS (removed voucher rows) will be ignored
    /// </summary>
    //[DisplayFormat(DataFormatString = "Boolean")]
    [DefaultValue(false)]
    public bool IgnoreBTRANS { get; set; }

    /// <summary>
    /// If true #RTRANS (added voucher rows) will be ignored
    /// </summary>
    //[DisplayFormat(DataFormatString = "Boolean")]
    [DefaultValue(false)]
    public bool IgnoreRTRANS { get; set; }

    /// <summary>
    /// If true some errors for missing dates will be ignored
    /// </summary>
    //[DisplayFormat(DataFormatString = "Boolean")]
    [DefaultValue(true)]
    public bool IgnoreMissingDate { get; set; }

    /// <summary>
    /// If true don't store values internally. The user has to use the Callback class to get the values. Usefull for large files
    /// </summary>
    //[DisplayFormat(DataFormatString = "Boolean")]
    [DefaultValue(false)]
    public bool StreamValues { get; set; }

    /// <summary>
    /// If false then cache all Exceptions in SieDocument.ValidationExceptions
    /// </summary>
    //[DisplayFormat(DataFormatString = "Boolean")]
    [DefaultValue(true)]
    public bool ThrowErrors { get; set; }

    /// <summary>
    /// The standard says yyyyMMdd and parser will default to that, but you can change the format to whatever you want
    /// </summary>
    [DisplayFormat(DataFormatString = "Text")]
    [DefaultValue("yyMMdd")]
    public string DateFormat { get; set; }
    /// <summary>
    /// The standard says codepage 437, but that is not supported by dotnet core. You can change it to whatever you want. It will default to codepage 437 when running in Dotnet Framework and 28591 (ISO-8859-1) when running dotnet core. Please note that #KSUMMA will not be caclutated if you choose a multibyte encoding
    /// </summary>
    [DisplayFormat(DataFormatString = "Text")]
    [DefaultValue("")]
    public string Encoding { get; set; }
}

/// <summary>
/// The result for a file read operation
/// </summary>
public class ParseResult
{
    /// <summary>
    /// List of files from VER rows in SIE document
    /// </summary>
    public List<string> Result { get; set; }

    /// <summary>
    /// Create a new ParseResult object
    /// </summary>
    public ParseResult(
        List<string> result
        )
    {
        Result = result;
    }
}