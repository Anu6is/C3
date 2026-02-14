using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;

namespace C3.Services;

public class TimerService(ILogger<TimerService> logger) : IAsyncDisposable
{
    private PeriodicTimer? _timer;
    private Task? _timerTask;
    private readonly CancellationTokenSource _cts = new();
    private readonly Dictionary<string, TimerSubscription> _subscriptions = [];
    private readonly HashSet<Action> _pendingCallbacks = [];
    private bool _isProcessingCallbacks;
    private long _tickCount;
    private readonly object _lock = new();

    public event Action? OnTick;

    public void Start()
    {
        lock (_lock)
        {
            if (_timerTask is not null) return;

            _timer = new PeriodicTimer(TimeSpan.FromSeconds(1));
            _timerTask = RunTimerAsync();
        }
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
                    logger.LogError(ex, "Error in TimerService OnTick");
                }

                // 2. Notify specific interval subscribers
                List<TimerSubscription> subsToRun;
                lock (_lock)
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
            logger.LogError(ex, "TimerService loop failed");
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
            logger.LogError(ex, "Error in TimerService subscription");
        }
    }

    private void ProcessPendingCallbacks()
    {
        if (_isProcessingCallbacks) return;
        _isProcessingCallbacks = true;
        try
        {
            Action[] callbacks;
            lock (_lock)
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
                    logger.LogError(ex, "Error in TimerService callback");
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
        if (intervalTicks <= 0)
        {
            throw new ArgumentOutOfRangeException(nameof(intervalTicks), "Interval must be greater than zero.");
        }

        var id = Guid.NewGuid().ToString();
        lock (_lock)
        {
            _subscriptions[id] = new TimerSubscription(action, intervalTicks);
        }
        return id;
    }

    public void Unsubscribe(string id)
    {
        lock (_lock)
        {
            _subscriptions.Remove(id);
        }
    }

    public void RequestUpdate(Action callback)
    {
        lock (_lock)
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
                logger.LogError(ex, "Error disposing TimerService");
            }
        }
        _cts.Dispose();
    }

    private record TimerSubscription(Func<Task> Action, int IntervalTicks);
}
