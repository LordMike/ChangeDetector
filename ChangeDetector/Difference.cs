namespace ChangeDetector
{
    /// <summary>
    /// A difference between two snapshots
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class Difference<T> where T : IKeyItem
    {
        /// <summary>
        /// The type of difference.
        /// </summary>
        public DifferenceType DifferenceType { get; set; }

        /// <summary>
        /// The original data.
        /// This is null if the <see cref="DifferenceType"/> is Added
        /// </summary>
        public T Original { get; set; }

        /// <summary>
        /// The original data.
        /// This is null if the <see cref="DifferenceType"/> is Deleted
        /// </summary>
        public T New { get; set; }
    }
}