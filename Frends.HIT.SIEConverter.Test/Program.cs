using Frends.HIT.SIEConverter;
using jsiSIE;
using System;
using System.IO;
using System.Text;
using static System.Net.Mime.MediaTypeNames;



ReadParams rp = new ReadParams();
rp.File = "test.si";
rp.Path = "C:\\Users\\linisa005\\source\\repos\\Frends.HIT.SIEConverter\\Frends.HIT.TaskTemplate.Test2\\bin\\Debug\\net6.0";

rp.IgnoreMissingOMFATTNING = true;
string readText = File.ReadAllText("test.si");
File.WriteAllText("temp.si", readText);

var fileA = "BOD10003_20230823_152428.si";

var fileB = "BOD10004_20230823_152403.si";

var fileTest = "temp.si";
Encoding encoding = EncodingHelper.GetDefault();
if (rp.Encoding != null)
{
    encoding = Encoding.GetEncoding(rp.Encoding);
}
var docA = new SieDocument()
{
    ThrowErrors = rp.ThrowErrors,
    IgnoreMissingOMFATTNING = rp.IgnoreMissingOMFATTNING,
    IgnoreBTRANS = rp.IgnoreBTRANS,
    IgnoreRTRANS = rp.IgnoreRTRANS,
    IgnoreMissingDate = rp.IgnoreMissingDate,
    StreamValues = rp.StreamValues,
    DateFormat = rp.DateFormat,
    Encoding = encoding
};

var docB = new SieDocument() { ThrowErrors = false, IgnoreMissingOMFATTNING = true };


docA.ReadDocument(rp.File);

//docB.ReadDocument(fileB);

//Console.WriteLine(docA);

//Console.WriteLine("--------------------------------");

//Console.WriteLine(docB);

Console.WriteLine("DATUM:" + docA.GEN_DATE);
foreach (var sieAccount in docA.KONTO.Values)
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
}