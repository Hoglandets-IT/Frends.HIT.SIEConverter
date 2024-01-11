using Frends.HIT.SIEConverter;
using jsiSIE;
using System;
using System.IO;
using System.Reflection.Metadata;
using System.Runtime.Serialization;
using System.Text;
using static System.Net.Mime.MediaTypeNames;



ParseParams rp = new ParseParams();

string path = "C:\\Users\\frewet001\\source\\repos\\Frends.HIT.SIEConverter\\Frends.HIT.TaskTemplate.Test2\\bin\\Debug\\net6.0";

rp.IgnoreMissingOMFATTNING = true;
//string readText = File.ReadAllText("BOD10003_20230823_152428.si");
//File.WriteAllText("temp.si", readText);

var fileA = "BOD10003_20230823_152428.si";

var fileB = "BOD10004_20230823_152403.si";

//var fileTest = "temp.si";
Encoding encoding = EncodingHelper.GetDefault();
rp.Encoding = "UTF-8";
if (rp.Encoding != null)
{
    encoding = Encoding.GetEncoding(rp.Encoding);
}
Console.WriteLine("Encoding: " + encoding.EncodingName);
var docA = new SieDocument()
{
    ThrowErrors = false,
    IgnoreMissingDate = true
};

var docB = new SieDocument() { ThrowErrors = false, IgnoreMissingOMFATTNING = true };


docA.ReadDocument(fileB);

//docB.ReadDocument(fileB);

//Console.WriteLine(docA);

//Console.WriteLine("--------------------------------");

//Console.WriteLine(docB);

Console.WriteLine("DATUM:" + docA.GEN_DATE);
Console.WriteLine(docA.KONTO.Values.Count);

Console.WriteLine("--------------------------------");

List<string> list = new List<string>();
Dictionary<string, SieDimension> dimensions = docA.DIM;
Dictionary<string, SieAccount> konton = docA.KONTO;
Dictionary<string, SieDimension> documentDimDictionery = docA.DIM;
foreach (KeyValuePair<string, SieDimension> entry in dimensions)
{
    Console.WriteLine("Entry key:" + entry.Key);
    Console.WriteLine("Entry value name:" + entry.Value.Name);
    Console.WriteLine("Entry value number:" + entry.Value.Number);
}

foreach (var sieVoucher in docA.VER)
{
    string result = "H ";
    var date = sieVoucher.Text.Split(' ').LastOrDefault();
    date = date.Replace("-", "");
    date = date.Remove(0, 2);
    DateTime dt = sieVoucher.Rows[0].RowDate;
    Console.WriteLine("ROWDATE: " + dt.ToString());
    result = result + String.Format("{0:yyMMdd}", dt) + " " + "Momentum" + " " + Truncate(sieVoucher.Number, 21) + "\r\n"; //Hårdkodat, ska fixas.

    Console.WriteLine("VER Text:" + sieVoucher.Text);
    
    Console.WriteLine("VER Number:" + sieVoucher.Number);
    Console.WriteLine("VER Date:" + sieVoucher.VoucherDate);
    Console.WriteLine("VER Token:" + sieVoucher.Token);
    Console.WriteLine("VER CreatedBy:" + sieVoucher.CreatedBy);
    Console.WriteLine("VER CreatedDate:" + sieVoucher.CreatedDate);
    Console.WriteLine("VER Series:" + sieVoucher.Series);

    foreach (var sieVoucherRow in sieVoucher.Rows)
    {
        string ver = "K ";
        Console.WriteLine("VER Row Amount:" + sieVoucherRow.Amount);
        Console.WriteLine("VER Row Date:" + sieVoucherRow.RowDate);
        Console.WriteLine("VER Row Text:" + sieVoucherRow.Text);
        Console.WriteLine("VER Row Created By:" + sieVoucherRow.CreatedBy);
        Console.WriteLine("VER Row Quantity:" + sieVoucherRow.Quantity);
        Console.WriteLine("VER Row Token :" + sieVoucherRow.Token);
        ver = ver + Truncate(sieVoucherRow.Account.Number, 10);
        ver = ver.PadRight(12);
        string dimRowString = AddRowDimensions(sieVoucherRow, documentDimDictionery);
        ver = ver + dimRowString;

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
        ver += Truncate(sieVoucher.Text, 30);
        ver += "\r\n";
        result = result + ver;
    }

    list.Add(result);

}

foreach (var result in list)
{
    Console.WriteLine(result);
}

static string AddRowDimensions(SieVoucherRow row, Dictionary<string, SieDimension> dictionary)
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
                rowString = rowString + Truncate(sieObject.Number, 10);
            }
        }
        counter = counter + 10;
        rowString = rowString.PadRight(counter);
    }
    return rowString;
}

static string Truncate(string value, int maxLength)
{
    if (string.IsNullOrEmpty(value)) return value;
    return value.Length <= maxLength ? value : value.Substring(0, maxLength);
}



/*foreach (var sieAccount in docA.KONTO.Values)
{
    Console.WriteLine("KONTO Name:" + sieAccount.Name);
    Console.WriteLine("KONTO Number:" + sieAccount.Number);
    Console.WriteLine("KONTO Unit:" + sieAccount.Unit);
    Console.WriteLine("KONTO SRU:" + sieAccount.SRU);
    Console.WriteLine("KONTO Type:" + sieAccount.Type);
}
var dim20=docA.DIM["20"];
Console.WriteLine("dim20 name:" + dim20.Name);
Console.WriteLine("dim20 number:" + dim20.Number);
Console.WriteLine("Fnamn: " + docA.FNAMN.Name);

foreach (var sieVoucher in docA.VER)
{
    Console.WriteLine("VER Text:" + sieVoucher.Text);
    var date = sieVoucher.Text.Split(' ').LastOrDefault();
    Console.WriteLine(date);
    Console.WriteLine("VER Number:" + sieVoucher.Number);
    Console.WriteLine("VER Date:" + sieVoucher.VoucherDate);
    Console.WriteLine("VER Token:" + sieVoucher.Token);
    Console.WriteLine("VER CreatedBy:" + sieVoucher.CreatedBy);
    Console.WriteLine("VER CreatedDate:" + sieVoucher.CreatedDate);
    Console.WriteLine("VER Series:" + sieVoucher.Series);

    foreach (var sieVoucherRow in sieVoucher.Rows)
    {
        Console.WriteLine("VER Row Amount:" + sieVoucherRow.Amount);
        Console.WriteLine("VER Row Date:" + sieVoucherRow.RowDate);
        Console.WriteLine("VER Row Text:" + sieVoucherRow.Text);
        Console.WriteLine("VER Row Created By:" + sieVoucherRow.CreatedBy);
        Console.WriteLine("VER Row Quantity:" + sieVoucherRow.Quantity);
        Console.WriteLine("VER Row Token :" + sieVoucherRow.Token);

        Console.WriteLine("VER Row Account Number:" + sieVoucherRow.Account.Number);
        Console.WriteLine("VER Row Account Name:" + sieVoucherRow.Account.Name);
        Console.WriteLine("VER Row Account Unit:" + sieVoucherRow.Account.Unit);
        Console.WriteLine("VER Row Account Type:" + sieVoucherRow.Account.Type);

        foreach (var sieObject in sieVoucherRow.Objects)
        {
            Console.WriteLine("Object Name:" + sieObject.Name);
            Console.WriteLine("Object Number:" + sieObject.Number);
            Console.WriteLine("Object Dimension Name:" + sieObject.Dimension.Name);
            Console.WriteLine("Object Dimension Number:" + sieObject.Dimension.Number);
        }
    }
}*/