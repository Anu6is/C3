namespace C3.Application.State;

public class WarDataChangeTracker
{
    private int _version;
    public int Version => _version;

    public void NotifyChanged() => Interlocked.Increment(ref _version);
}
