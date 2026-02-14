using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace C3.Services;

public class TimerService : IAsyncDisposable
{
    private PeriodicTimer? _timer;
    private Task? _timerTask;
    private readonly CancellationTokenSource _cts = new();
    private readonly Dictionary<string, TimerSubscription> _subscriptions = [];
    private readonly HashSet<Action> _pendingCallbacks = [];
    private bool _isProcessingCallbacks;
    private long _tickCount;

    public event Action? OnTick;

    public void Start()
    {
        if (_timerTask is not null) return;

        _timer = new PeriodicTimer(TimeSpan.FromSeconds(1));
        _timerTask = RunTimerAsync();
    }

    private async Task RunTimerAsync()
    {
        try
        {
            while (_timer != null && await _timer.WaitForNextTickAsync(_cts.Token))
            {
                _tickCount++;

                // 1. Notify general subscribers
                try
                {
                    OnTick?.Invoke();
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Error in TimerService OnTick: {ex.Message}");
                }

                // 2. Notify specific interval subscribers
                List<TimerSubscription> subsToRun;
                lock (_subscriptions)
                {
                    subsToRun = _subscriptions.Values
                        .Where(s => _tickCount % s.IntervalTicks == 0)
                        .ToList();
                }

                foreach (var sub in subsToRun)
                {
                    _ = ExecuteSafe(sub.Action);
                }

                // 3. Process batched UI updates
                ProcessPendingCallbacks();
            }
        }
        catch (OperationCanceledException)
        {
            // Expected on disposal
        }
        catch (Exception ex)
        {
            Console.WriteLine($"TimerService loop failed: {ex.Message}");
        }
    }

    private async Task ExecuteSafe(Func<Task> action)
    {
        try
        {
            await action();
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error in TimerService subscription: {ex.Message}");
        }
    }

    private void ProcessPendingCallbacks()
    {
        if (_isProcessingCallbacks) return;
        _isProcessingCallbacks = true;
        try
        {
            Action[] callbacks;
            lock (_pendingCallbacks)
            {
                if (_pendingCallbacks.Count == 0) return;
                callbacks = _pendingCallbacks.ToArray();
                _pendingCallbacks.Clear();
            }

            foreach (var callback in callbacks)
            {
                try
                {
                    callback();
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Error in TimerService callback: {ex.Message}");
                }
            }
        }
        finally
        {
            _isProcessingCallbacks = false;
        }
    }

    public string Subscribe(Action action, int intervalTicks = 1)
    {
        return Subscribe(() =>
        {
            action();
            return Task.CompletedTask;
        }, intervalTicks);
    }

    public string Subscribe(Func<Task> action, int intervalTicks = 1)
    {
        var id = Guid.NewGuid().ToString();
        lock (_subscriptions)
        {
            _subscriptions[id] = new TimerSubscription(action, intervalTicks);
        }
        return id;
    }

    public void Unsubscribe(string id)
    {
        lock (_subscriptions)
        {
            _subscriptions.Remove(id);
        }
    }

    public void RequestUpdate(Action callback)
    {
        lock (_pendingCallbacks)
        {
            _pendingCallbacks.Add(callback);
        }
    }

    public async ValueTask DisposeAsync()
    {
        _cts.Cancel();
        _timer?.Dispose();
        if (_timerTask is not null)
        {
            try
            {
                await _timerTask;
            }
            catch (OperationCanceledException) { }
            catch (Exception ex)
            {
                Console.WriteLine($"Error disposing TimerService: {ex.Message}");
            }
        }
        _cts.Dispose();
        GC.SuppressFinalize(this);
    }

    private record TimerSubscription(Func<Task> Action, int IntervalTicks);
}
