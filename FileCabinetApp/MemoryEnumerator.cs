namespace FileCabinetApp;

/// <summary>
/// The records enumerator for memory service.
/// </summary>
/// <typeparam name="T">FileCabinetRecord class.</typeparam>
internal class MemoryEnumerator<T> : IEnumerable<T>
{
    private readonly IEnumerable<T> list;

    /// <summary>
    /// Initializes a new instance of the <see cref="MemoryEnumerator{T}"/> class.
    /// </summary>
    /// <param name="list">The list of records.</param>
    public MemoryEnumerator(IEnumerable<T> list)
    {
        this.list = list;
    }

    /// <summary>
    /// Gets the enumerator.
    /// </summary>
    /// <returns>The enumerator.</returns>
    public IEnumerator<T> GetEnumerator()
    {
        foreach (var record in this.list)
        {
            yield return record;
        }
    }

    /// <summary>
    /// Gets the enumerator.
    /// </summary>
    /// <returns>The enumerator.</returns>
    IEnumerator IEnumerable.GetEnumerator()
    {
        return this.GetEnumerator();
    }
}