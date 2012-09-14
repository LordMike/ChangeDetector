using System;
using Microsoft.Win32;

namespace ChangeDetector
{
    /// <summary>
    /// Represents a RegistryValue
    /// Contains a near-complete replica of the actual info in the Registry database
    /// </summary>
    [Serializable]
    public class SnapshotRegistryValue : SnapshotRegistryItem, IEquatable<SnapshotRegistryValue>
    {
        public RegistryValueKind RegistryKeyType { get; set; }
        public string Value { get; set; }

        public bool Equals(SnapshotRegistryValue other)
        {
            return base.Equals(other)
                   && RegistryKeyType == other.RegistryKeyType
                   && Value == other.Value;
        }
    }
}