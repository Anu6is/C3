namespace C3.Models;

public class WarDataChangeTracker
{
    private int _version;
    public int Version => _version;

    public void NotifyMembersChanged() => Interlocked.Increment(ref _version);
    public void NotifySpiesChanged() => Interlocked.Increment(ref _version);
    public void NotifyScoresChanged() => Interlocked.Increment(ref _version);
}
