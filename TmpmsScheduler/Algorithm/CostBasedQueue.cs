namespace TMPMSChecker.Algorithm;

/// <summary>
/// A priority queue over the elements.
/// </summary>
/// <typeparam name="T"></typeparam>
public class CostBasedQueue<T>
{
    public CostBasedQueue()
    {
        
    }

    
    private readonly PriorityQueue<T, float> _queue = new();
    
    /// <summary>
    /// Dequeues the element with least cost from the queue.
    /// </summary>
    /// <returns></returns>
    public T Dequeue() => _queue.Dequeue();
    
    /// <summary>
    /// Enqueues an element with a cost. The lower the cost the closer to head of queue.
    /// </summary>
    /// <param name="conf"></param>
    /// <param name="score"></param>
    public void Enqueue(T conf, float score) => _queue.Enqueue(conf, score);

    public int Count => _queue.Count;
}