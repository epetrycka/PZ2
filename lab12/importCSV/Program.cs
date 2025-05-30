using sqlite;

var createCsv = new ImportCSV("data/out/measurements_withoutEnclosedObject.csv");

var builder = WebApplication.CreateBuilder(args);
var app = builder.Build();

app.MapGet("/", () => "Hello World!");

app.Run();
