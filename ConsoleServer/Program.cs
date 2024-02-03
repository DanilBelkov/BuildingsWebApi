using System.Threading;

public static class Server
{
    private static int _count;
    private static ReaderWriterLockSlim locker = new ReaderWriterLockSlim();
    public static int GetCount()
    {
        locker.EnterReadLock();
        try
        {
            return _count;
        }
        finally
        {
            locker.ExitReadLock();
        }
    }
    public static void AddCount(int value)
    {
        locker.EnterWriteLock();
        _count = value;
        locker.ExitWriteLock();
    }
}
