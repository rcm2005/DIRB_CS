//dirb program by rafael

using System.ComponentModel.DataAnnotations;
using System.Reflection;
using System.Net.Http;

Console.WriteLine("Bem vindo ao meu dirb");
Console.WriteLine("Informe o nome do arquivo com a lista (nao precisa da extensão)");
var nome = Console.ReadLine();


var lista = File.ReadAllLines($"{nome}.txt");


foreach (var item in lista)
{
    Console.WriteLine(item);
}

// Console.WriteLine(string.Join(", ", lista));
// Console.WriteLine("inté");

Console.WriteLine("Informe a url desejada");
var url = Console.ReadLine();

var client = new HttpClient();

var resultados = new List<(string url, int status)>();


foreach (var itemRaw in lista)
{
    var item = itemRaw.Trim();
    if (string.IsNullOrEmpty(item)) continue;

    var teste = "http://" + url.TrimEnd('/') + "/" + item;
    Console.WriteLine(teste);

    try
    {
        var result = await client.GetAsync(teste);
        var status = (int)result.StatusCode;

        resultados.Add((teste, status));
        Console.WriteLine($"[{status}] {teste}");
    }
    catch (Exception ex)
    {
        Console.WriteLine($"[ERRO] {teste} -> {ex.Message}");
    }

    
}

var achados = resultados.Where(r => r.status != 404).ToList();

Console.WriteLine("\n==== RESUMO ====");
Console.WriteLine($"Total encontrados (≠ 404): {achados.Count}");

foreach (var grupo in achados.GroupBy(r => r.status))
{
    Console.WriteLine($"\nStatus {grupo.Key}:");
    foreach (var r in grupo)
        Console.WriteLine($" - {r.url}");
}
//var result = await client.GetAsync($"https://{url}");



