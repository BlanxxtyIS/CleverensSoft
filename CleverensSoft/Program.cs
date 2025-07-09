using System.Text;

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

    Введите количество итерация с классом:
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

//Не стал выносить в новый файл
public static class Server
{
    private static int _count = 0;
    private static readonly ReaderWriterLockSlim _lock = new ReaderWriterLockSlim();    

    public static void GetCount()
    {
        _lock.EnterReadLock();
        try
        {
            Console.WriteLine($"Количество: {_count}");
        }
        finally
        {
            _lock.ExitReadLock();
        }
    }

    public static void AddToCount(int value)
    {
        _lock.EnterWriteLock();
        try
        {
            _count += value;
        } 
        finally
        {
            _lock.ExitWriteLock();
        }
    }
}