using System;
using System.Collections.Generic;

namespace ChangeDetector
{
    /// <summary>
    /// Represents a File System snapshot. All file system snapshots are relative, 
    /// so this snapshot contains base path for all its items.
    /// </summary>
    [Serializable]
    public class SnapshotFilesystem
    {
        public string BasePath { get; set; }
        public LinkedList<SnapshotFilesystemItem> Items { get; set; }
    }
}