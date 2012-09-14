using System;

namespace ChangeDetector
{
    /// <summary>
    /// Represents a File System file. A file also includes its file hash.
    /// </summary>
    [Serializable]
    public class SnapshotFileInfo : SnapshotFilesystemItem, IEquatable<SnapshotFileInfo>
    {
        /// <summary>
        /// MD5 hash of the file. 
        /// May be null (if the file was unreadable).
        /// </summary>
        public string Hash { get; set; }

        public bool Equals(SnapshotFileInfo other)
        {
            return base.Equals(other) && Hash == other.Hash;
        }
    }
}