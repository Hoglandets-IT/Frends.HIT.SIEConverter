using jsiSIE;
using System.Collections.Generic;

namespace Frends.HIT.SIEConverter;

/// <summary>
/// A class containing functions that can be used in the main class for repeated tasks
/// </summary>
public class Helpers
{
    /// <summary>
    /// Returns True if the task was successful
    /// </summary>
    /// <returns></returns>
    public static bool IsSuccessful()
    {
        return true == true;
    }

    public static MemoryStream GenerateStreamFromByteArray(byte[] b)
    {
        MemoryStream stream = new MemoryStream(b);
        //var writer = new StreamWriter(stream);
        //writer.Write(b, 0, b.Length);
        //writer.Flush();
        //stream.Position = 0;
        return stream;
    }

    public static List<string> parseSIEFile(SieDocument document) 
    {
        List<string> list = new List<string>();
        
        foreach (var sieVoucher in document.VER)
        {
            string result = "H ";
            var date = sieVoucher.Text.Split(' ').LastOrDefault();
            date = date.Replace("-", "");
            date = date.Remove(0, 2);
            result = result + date + " Momentum Faktura\r\n"; //Hårdkodat, ska fixas.

            foreach (var sieVoucherRow in sieVoucher.Rows)
            {
                string ver = "K ";
  
                ver = ver + sieVoucherRow.Account.Number;
                ver = ver.PadRight(12);
              

                foreach (var sieObject in sieVoucherRow.Objects)
                {
                    ver = ver + sieObject.Number;
                    ver = ver.PadRight(82);
               
                }
                ver += "EK24";
                ver = ver.PadRight(124);
                if (sieVoucherRow.Amount > 0)
                {

                    ver += (" " + sieVoucherRow.Amount);

                }
                else
                {
                    ver += sieVoucherRow.Amount;
                }
                ver = ver.PadRight(141);
                ver += sieVoucher.Text;
                ver += "\r\n";
                result = result + ver;
            }

            list.Add(result);

        }

        return list;

    }
}