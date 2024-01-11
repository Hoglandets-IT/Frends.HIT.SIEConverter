using jsiSIE;
using System.ComponentModel;
using System.Text;

namespace Frends.HIT.SIEConverter;

public class Main
{
    /// <summary>
    /// Reads a Sie File as byte array and parses it to Raindance format
    /// </summary> 
    /// <param name="input">Parse parameters</param>
    /// <returns>ParseResult object</returns>
    [DisplayName("Parse Momentum SIE file")]
    public static ParseResult ReadSIE([PropertyTab] ParseParams input)
    {

        Encoding encoding = EncodingHelper.GetDefault();
        if (input.Encoding != "")
        {
            encoding = Encoding.GetEncoding(input.Encoding);
        }

        MemoryStream stream = Helpers.GenerateStreamFromByteArray(input.File);
        var sieDoc = new SieDocument()
        {
            ThrowErrors = input.ThrowErrors,
            IgnoreMissingOMFATTNING = input.IgnoreMissingOMFATTNING,
            IgnoreBTRANS = input.IgnoreBTRANS,
            IgnoreRTRANS = input.IgnoreRTRANS,
            IgnoreMissingDate = input.IgnoreMissingDate,
            StreamValues = input.StreamValues,
            Encoding = encoding
        };
        sieDoc.ReadDocument(stream);
        return new ParseResult(
            result: MapToRaindanceFormat(sieDoc, input.DateFormat)
            
        );
    }

    private static List<string> MapToRaindanceFormat(SieDocument document, string DateFormat)
    {
        Dictionary<string, SieDimension> documentDimDictionery = document.DIM;
        List<string> list = new List<string>();

        foreach (var sieVoucher in document.VER)
        {
            string result = "H ";

            DateTime dt = sieVoucher.Rows[0].RowDate;
            string date = String.Format("{0:" + DateFormat + "}", dt);
            result = result + date + " " + "Momentum" + " " + Helpers.Truncate(sieVoucher.Number, 21) + "\r\n"; //Hårdkodat, ska fixas.

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