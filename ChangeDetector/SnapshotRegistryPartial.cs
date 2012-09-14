using System;
using System.Collections.Generic;

namespace ChangeDetector
{
    /// <summary>
    /// Represents a Partial Registry snapshot. Normally there is one partial snapshot for each hive.
    /// A complete snapshot then consists of a list of partial snapshots - one for each hive.
    /// </summary>
    [Serializable]
    public class SnapshotRegistryPartial
    {
        public string BasePath { get; set; }
        public LinkedList<SnapshotRegistryItem> Items { get; set; }
    }
}