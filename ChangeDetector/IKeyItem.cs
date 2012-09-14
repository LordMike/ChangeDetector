namespace ChangeDetector
{
    /// <summary>
    /// Primary interface used by the differences detector.
    /// </summary>
    public interface IKeyItem
    {
        string PrimaryKey { get; }
    }
}