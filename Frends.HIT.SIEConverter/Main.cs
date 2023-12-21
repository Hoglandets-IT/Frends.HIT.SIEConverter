using jsiSIE;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Frends.HIT.SIEConverter;

public class Main
{
    /// <summary>
    /// This is the information shown about the task in the Frends control panel.
    /// The TaskInput input parameters will be shown on a different tab than the verbose option with the PropertyTab attribute.
    /// </summary> 
    /// <param name="verbose">This parameter increases the verbosity of the output</param>
    /// <param name="input">A set of parameters for doing something in the class</param>
    /// <returns>TaskOutput object</returns>
    [DisplayName("Read SIE File")]
    public static ReadResult ReadSIE([PropertyTab] ReadParams input)
    {
        //var file = string.Join("/", new string[] { input.Path, input.File });
        File.WriteAllBytes(input.FileName, input.File);
        Encoding encoding = EncodingHelper.GetDefault();
        //if (input.Encoding != null)
        //{
        //    encoding = Encoding.GetEncoding(input.Encoding);
        //}
        var doc = new SieDocument()
        {
            ThrowErrors = input.ThrowErrors,
            IgnoreMissingOMFATTNING = input.IgnoreMissingOMFATTNING,
            IgnoreBTRANS = input.IgnoreBTRANS,
            IgnoreRTRANS = input.IgnoreRTRANS,
            IgnoreMissingDate = input.IgnoreMissingDate,
            StreamValues = input.StreamValues,
            DateFormat = input.DateFormat,
            Encoding = encoding
        };
        doc.ReadDocument(input.FileName);
        return new ReadResult(
            content: doc,
            path: string.Join("/", new string[] { input.Path, input.FileName })
        );
    }
}