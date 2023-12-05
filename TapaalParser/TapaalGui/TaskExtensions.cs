namespace TapaalParser.TapaalGui;

public static class TaskExtensions {
    public static void Add<T>(this ICollection<Task<T>> tasks, Func<T> action)
    {
        var t = Task.Run(action.Invoke);
        tasks.Add(t);
    }

}