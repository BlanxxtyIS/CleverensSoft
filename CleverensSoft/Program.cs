using CleverensSoft;
using System.Globalization;
using System.Text;
using System.Text.RegularExpressions;

Console.WriteLine("""
            Тестовое задание, CleverensSoft!
    """);


Console.WriteLine("""

     ------------------------------------------
    |       Задача 1. Компрессия строки.      |
     ------------------------------------------

    Введите строку для компрессии: 
    """);

string userText = Console.ReadLine();

if (string.IsNullOrEmpty(userText))
{
    Console.WriteLine("Пустая строка - нечего сжимать.");
    return;
}

Console.WriteLine($"Изначальная строка: {userText}");
string compressed = CompressString(userText);
Console.WriteLine($"Закомпрессованная строка: {compressed}");

string CompressString(string input)
{
    StringBuilder result = new StringBuilder();
    int count = 1;

    for (int i = 1; i < input.Length; i++) {
        if (input[i] == input[i - 1]) {
            count++;
        } else {
            result.Append(input[i - 1]);
            if (count > 1) {
                result.Append(count);
            }
            count = 1;
        }
    }

    result.Append(input[^1]);
    if (count > 1)
    {
        result.Append(count);
    }
    return result.ToString();
}

Console.WriteLine("""


    -----------------------------------------------------------
    |    Задание 2. Тестирование сервера на многопоточность.   |
    -----------------------------------------------------------

    Введите количество итераций с классом:
    Каждая кратная '5' итерация будет вызывать GetCount, отсальные AddToCount!
    """);

int serverIteration = Convert.ToInt32(Console.ReadLine());

Parallel.For(0, serverIteration, i =>
{
    if (i % 5 == 0)
    {
        Console.WriteLine($"Добавили {i}");
        Server.AddToCount(1);
    }
    else
    {
        Console.WriteLine("Получаем информацию");
        Server.GetCount();
    }
});


Console.WriteLine("""


    -----------------------------------------------------------
    |    Задание 3. Стандартизирование лог-файлов.            |
    -----------------------------------------------------------
    
    """);

string inputPath = "input.log";
string outputPath = "output.log";

if (!File.Exists(inputPath))
{
    Console.WriteLine($"Файл не найден: {inputPath}");
    return;
}


var lines = File.ReadAllLines(inputPath);
using var writer = new StreamWriter(outputPath);

foreach (var line in lines)
{
    if (string.IsNullOrEmpty(line)) 
        continue;

    var result = ParseLogLine(line);
    if (result != null)
    {
        writer.WriteLine(result);
    }

    Console.WriteLine("Файл успешно преобразован.");
}

static string? ParseLogLine(string line)
{
    var format1 = new Regex(@"^(?<date>\d{2}\.\d{2}\.\d{4}) (?<time>\d{2}:\d{2}:\d{2}\.\d{3}) (?<level>\w+)\s+(?<message>.+)$");
    var format2 = new Regex(@"^(?<date>\d{4}-\d{2}-\d{2}) (?<time>\d{2}:\d{2}:\d{2}\.\d{3}) (?<level>\w+)(?: \[!!!\])?(?<method>\S+)?\s+(?<message>.+)$");

    Match match;

    if ((match = format1.Match(line)).Success)
    {
        string date = ConvertDate(match.Groups["date"].Value, "dd.MM.yyyy");
        string time = match.Groups["time"].Value;
        string level = NormalizeLevel(match.Groups["level"].Value);
        string method = "DEFAULT";
        string message = match.Groups["message"].Value.Trim();

        return $"{date}\t{time}\t{level}\tDEFAULT\t{method}\t{message}";
    }
    else if ((match = format2.Match(line)).Success)
    {
        string date = ConvertDate(match.Groups["date"].Value, "yyyy-MM-dd");
        string time = match.Groups["time"].Value;
        string level = NormalizeLevel(match.Groups["level"].Value);
        string method = string.IsNullOrWhiteSpace(match.Groups["method"].Value) ? "DEFAULT" : match.Groups["method"].Value.Trim();
        string message = match.Groups["message"].Value.Trim();

        return $"{date}\t{time}\t{level}\tDEFAULT\t{method}\t{message}";
    }

    return null; // Неизвестный формат строки
}

static string ConvertDate(string input, string format)
{
    DateTime date = DateTime.ParseExact(input, format, CultureInfo.InvariantCulture);
    return date.ToString("dd-MM-yyyy");
}

static string NormalizeLevel(string level)
{
    return level.ToUpper() switch
    {
        "INFO" or "INFORMATION" => "INFO",
        "WARN" or "WARNING" => "WARN",
        "ERROR" => "ERROR",
        "DEBUG" => "DEBUG",
        _ => "UNKNOWN"
    };
}


