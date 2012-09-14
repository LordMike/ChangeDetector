using System;
using System.Collections.Generic;

namespace ChangeDetector
{
    /// <summary>
    /// Represents a Registry snapshot. All registry snapshots are relative, 
    /// so this snapshot contains base path for all its items.
    /// </summary>
    [Serializable]
    public class SnapshotRegistry
    {
        public List<SnapshotRegistryPartial> PartialSnapshots { get; set; }

        public SnapshotRegistry()
        {
            PartialSnapshots = new List<SnapshotRegistryPartial>();
        }
    }
}