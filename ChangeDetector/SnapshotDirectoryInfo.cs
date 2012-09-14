using System;

namespace ChangeDetector
{
    /// <summary>
    /// Represents a File System Directory.
    /// </summary>
    [Serializable]
    public class SnapshotDirectoryInfo : SnapshotFilesystemItem, IEquatable<SnapshotFileInfo>
    {
        public bool Equals(SnapshotFileInfo other)
        {
            return base.Equals(other);
        }
    }
}