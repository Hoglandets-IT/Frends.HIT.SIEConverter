using jsiSIE;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Text;

// This should be the root namespace of the package
namespace Frends.HIT.SIEConverter;

/// <summary>
/// Parameters for reading SIE file
/// </summary>
[DisplayName("Parameters")]
public class ReadParams
{

    /// <summary>
    /// The path to the file for which to retrieve the content
    /// </summary>
    [DefaultValue("")]
    [DisplayFormat(DataFormatString = "Text")]
    public string Path { get; set; }

    /// <summary>
    /// The name of the file
    /// </summary>
    [DefaultValue("")]
    [DisplayFormat(DataFormatString = "Text")]
    public string FileName { get; set; }

    /// <summary>
    /// The file as a byte array
    /// </summary>
    [DefaultValue("")]
    [DisplayFormat(DataFormatString = "Text")]
    public byte[] File { get; set; }

    /// <summary>
    /// If true the parser will not flag a missing #OMFATTN as an error
    /// </summary>
    //[DisplayFormat(DataFormatString = "Boolean")]
    public bool IgnoreMissingOMFATTNING { get; set; }

    /// <summary>
    /// If true #BTRANS (removed voucher rows) will be ignored
    /// </summary>
    //[DisplayFormat(DataFormatString = "Boolean")]
    public bool IgnoreBTRANS { get; set; }

    /// <summary>
    /// If true #RTRANS (added voucher rows) will be ignored
    /// </summary>
    //[DisplayFormat(DataFormatString = "Boolean")]
    public bool IgnoreRTRANS { get; set; }

    /// <summary>
    /// If true some errors for missing dates will be ignored
    /// </summary>
    //[DisplayFormat(DataFormatString = "Boolean")]
    public bool IgnoreMissingDate { get; set; }

    /// <summary>
    /// If true don't store values internally. The user has to use the Callback class to get the values. Usefull for large files
    /// </summary>
    //[DisplayFormat(DataFormatString = "Boolean")]
    public bool StreamValues { get; set; }

    /// <summary>
    /// If false then cache all Exceptions in SieDocument.ValidationExceptions
    /// </summary>
    //[DisplayFormat(DataFormatString = "Boolean")]
    public bool ThrowErrors { get; set; }

    /// <summary>
    /// The standard says yyyyMMdd and parser will default to that, but you can change the format to whatever you want
    /// </summary>
    [DisplayFormat(DataFormatString = "Text")]
    [DefaultValue("yyyyMMdd")]
    public string DateFormat { get; set; }
    /// <summary>
    /// The standard says codepage 437, but that is not supported by dotnet core. You can change it to whatever you want. It will default to codepage 437 when running in Dotnet Framework and 28591 (ISO-8859-1) when running dotnet core. Please note that #KSUMMA will not be caclutated if you choose a multibyte encoding
    /// </summary>
    [DisplayFormat(DataFormatString = "Text")]
    public string Encoding { get; set; }
}

/// <summary>
/// The result for a file read operation
/// </summary>
public class ReadResult
{
    /// <summary>
    /// The content of the read file
    /// </summary>
    public SieDocument Content { get; set; }

    /// <summary>
    /// The path to the file that was read
    /// </summary>
    public string Path { get; set; }

    /// <summary>
    /// Create a new ReadResult object
    /// </summary>
    public ReadResult(
        SieDocument content,
        string path
    )
    {
        Content = content;
        Path = path;
    }
}

public enum MultipleChoiceOption
{
    /// <summary>
    /// This is the text that is shown in the control panel when hovering over the option
    /// </summary>
    [Display(Name = "Option A Display Name")]
    OptionA,

    /// <summary>
    /// This is the text that is shown in the control panel when hovering over the option
    /// </summary>
    [Display(Name = "Option B Display Name")]
    OptionB,

    /// <summary>
    /// This is the text that is shown in the control panel when hovering over the option
    /// </summary>
    [Display(Name = "Option C Display Name")]
    OptionC,
}

/// <summary>
/// This is the information shown about the input group in the control panel
/// Since this is set via the interface and not programmatically, it doesn't need a public class for instantiating.
/// </summary>
public class TaskInput
{
    /// <summary>
    /// This is the information shown about the parameter in the control panel
    /// </summary>
    [Display(Name = "Multiple Choice Option Name")]
    public MultipleChoiceOption ChoiceOption { get; set; }

    /// <summary>
    /// This is the information shown about the text parameter in the panel
    /// If no text is provided, the default value is "ABCDEFG" (also shown in control panel)
    /// </summary>
    [DefaultValue("ABCDEFG")]
    [Display(Name = "Text Parameter Name")]
    public string NameOption { get; set; }

    /// <summary>
    /// The information shown about this parameter in the panel.
    /// With the PasswordPropertyText attribute, the content is hidden by default
    /// </summary>
    [PasswordPropertyText]
    public string PasswordOption { get; set; }

    /// <summary>
    /// The default editor box for this parameter will be of the "Expression" type
    /// </summary>
    [DisplayFormat(DataFormatString = "Expression")]
    public string ExpressionTextOption { get; set; }

    /// <summary>
    /// A boolean option
    /// </summary>
    public bool BooleanOption { get; set; }

    /// <summary>
    /// This field will only be shown if the "ChoiceOption" field is A
    /// </summary>
    [UIHint(nameof(ChoiceOption), "", MultipleChoiceOption.OptionA, MultipleChoiceOption.OptionB)]
    public string HiddenTextOption { get; set; }

    /// <summary>
    /// This field will only be shown if the BooleanOption field is false
    /// </summary>
    [UIHint(nameof(BooleanOption), "", false)]
    public string HiddenBooleanOption { get; set; }
}

/// <summary>
/// This is the output from the task.
/// Since this will be defined programmatically, there is an upside to using
/// a public facing class for creating an instance for the output. 
/// </summary>
public class TaskOutput
{
    /// <summary>
    /// Whether the operation was successful
    /// </summary>
    public bool Success { get; set; }

    /// <summary>
    /// Any messages returned by the task
    /// </summary>
    public string Info { get; set; }

    public TaskOutput(
        bool success,
        string info
    )
    {
        Success = success;
        Info = info;
    }
}