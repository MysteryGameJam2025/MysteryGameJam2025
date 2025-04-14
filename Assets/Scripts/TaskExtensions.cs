using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

public static class TaskExtensions
{
    public static void RunParallel(this Task task, Action onCompleted = null)
    {
        Func<Task, Task> wrapper = async passedTask =>
        {
            try
            {
                await passedTask;
                onCompleted?.Invoke();
            }
            catch (Exception e)
            {
                AsyncErrorReporter.Instance?.DeferredReportException(e);
                throw e;
            }
        };
        wrapper.Invoke(task);
    }

    public static void RunParallel<TResult>(this Task<TResult> task, Action<TResult> onCompleted = null)
    {
        Func<Task<TResult>, Task> wrapper = async passedTask =>
        {
            try
            {
                TResult result = await passedTask;
                onCompleted?.Invoke(result);
            }
            catch (Exception e)
            {
                AsyncErrorReporter.Instance?.DeferredReportException(e);
                throw e;
            }
        };
        wrapper.Invoke(task);
    }
}

public class AsyncErrorReporter : AbstractMonoBehaviourSingleton<AsyncErrorReporter>
{
    private List<Exception> Exceptions { get; set; }

    private void Awake()
    {
        Exceptions = new List<Exception>();
    }

    private void Update()
    {
        if (Exceptions.Count > 0)
        {
            Exceptions.ForEach(e => Debug.LogException(e));
            Exceptions = new List<Exception>();
        }
    }

    public void DeferredReportException(Exception e)
        => Exceptions.Add(e);
}