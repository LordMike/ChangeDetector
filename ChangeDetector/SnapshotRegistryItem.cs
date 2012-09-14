using System;
using Microsoft.Win32;

namespace ChangeDetector
{
    /// <summary>
    /// Represents a Registry item. This is subclassed into both Key and Value items.
    /// </summary>
    [Serializable]
    public abstract class SnapshotRegistryItem : IEquatable<SnapshotRegistryItem>, IKeyItem
    {
        public string FullPath { get; set; }
        public RegistryView RegistryView { get; set; }
        public bool WasReadable { get; set; }

        public bool Equals(SnapshotRegistryItem other)
        {
            if (other == null)
                return false;

            return FullPath.Equals(other.FullPath, StringComparison.InvariantCultureIgnoreCase)
                   && RegistryView == other.RegistryView
                   && WasReadable == other.WasReadable;
        }

        public string PrimaryKey
        {
            get { return FullPath + RegistryView; }
        }
    }
}