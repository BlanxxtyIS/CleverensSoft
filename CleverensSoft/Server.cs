namespace CleverensSoft;

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
