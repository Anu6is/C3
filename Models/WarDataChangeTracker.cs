namespace C3.Models;

public class WarDataChangeTracker
{
    private int _version;
    public int Version => _version;

    public void NotifyChanged() => Interlocked.Increment(ref _version);
}
