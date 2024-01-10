using jsiSIE;
using System.ComponentModel;
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
        //File.WriteAllBytes(input.FileName, input.File);
        Encoding encoding = EncodingHelper.GetDefault();
        //if (input.Encoding != null)
        //{
        //    encoding = Encoding.GetEncoding(input.Encoding);
        //}

        MemoryStream stream = Helpers.GenerateStreamFromByteArray(input.File);
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
        doc.ReadDocument(stream);
        return new ReadResult(
            //content: doc,
            result: ParseSIEFile(doc),
            path: string.Join("/", new string[] { input.Path, input.FileName })
            
        );
    }

    private static List<string> ParseSIEFile(SieDocument document)
    {
        Dictionary<string, SieDimension> documentDimDictionery = document.DIM;
        List<string> list = new List<string>();

        foreach (var sieVoucher in document.VER)
        {
            string result = "H ";
            result = result + sieVoucher.Rows[0].RowDate + " " + "Momentum" + " " + Helpers.Truncate(sieVoucher.Number, 21) + "\r\n"; //Hårdkodat, ska fixas.

            foreach (var sieVoucherRow in sieVoucher.Rows)
            {
                string ver = "K ";
                ver += Helpers.Truncate(sieVoucherRow.Account.Number, 10);
                ver = ver.PadRight(12);
                string dimRowString = AddRowDimensions(sieVoucherRow, documentDimDictionery);
                ver += dimRowString;

                ver += "EK24";
                ver = ver.PadRight(124);
                string amountString = sieVoucherRow.Amount.ToString().Replace(",", ".");
                if (sieVoucherRow.Amount > 0)
                {
                    ver += (" " + amountString);
                }
                else
                {
                    ver += amountString;
                }
                ver = ver.PadRight(141);
                ver += Helpers.Truncate(sieVoucher.Text, 30);
                ver += "\r\n";
                result += ver;
            }

            list.Add(result);
            
        }
        return list;
    }

    private static string AddRowDimensions(SieVoucherRow row, Dictionary<string, SieDimension> dictionary)
    {
        string[] dimensions = { "20", "21", "22", "23", "24", "25", "26" };
        string rowString = "";
        int counter = 0;
        foreach (string dim in dimensions)
        {
            SieDimension sieDimension = dictionary[dim];
            foreach (var sieObject in row.Objects)
            {
                if (sieObject.Dimension.Number == sieDimension.Number)
                {
                    rowString += Helpers.Truncate(sieObject.Number, 10);
                }
            }
            counter += 10;
            rowString = rowString.PadRight(counter);
        }
        return rowString;
    }
}