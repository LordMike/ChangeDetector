using System;
using System.IO;

namespace ChangeDetector
{
    /// <summary>
    /// Represents a Filesystem item. This is subclassed into both files and folders.
    /// </summary>
    [Serializable]
    public abstract class SnapshotFilesystemItem : IEquatable<SnapshotFilesystemItem>, IKeyItem
    {
        public string RelativePath { get; set; }
        public DateTime LastModified { get; set; }
        public DateTime LastAccess { get; set; }
        public FileAttributes Attributes { get; set; }
        public bool WasReadable { get; set; }

        public bool Equals(SnapshotFilesystemItem other)
        {
            if (other == null)
                return false;

            return RelativePath.Equals(other.RelativePath, StringComparison.InvariantCultureIgnoreCase)
                   && LastModified == other.LastModified
                   && LastAccess == other.LastAccess
                   && Attributes == other.Attributes
                   && WasReadable == other.WasReadable;
        }

        public string PrimaryKey
        {
            get { return RelativePath; }
        }
    }
}