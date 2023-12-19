using jsiSIE;


 
var fileA = "BOD10003_20230823_152428.si";

var fileB = "BOD10004_20230823_152403.si";

var docA = new SieDocument() { ThrowErrors = false, IgnoreMissingOMFATTNING = true };

var docB = new SieDocument() { ThrowErrors = false, IgnoreMissingOMFATTNING = true };

docA.ReadDocument(fileA);

docB.ReadDocument(fileB);

Console.WriteLine(docA);

Console.WriteLine("--------------------------------");

Console.WriteLine(docB);
