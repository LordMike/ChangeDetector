using System;
using System.Collections.Generic;
using Microsoft.Win32;

namespace ChangeDetector
{
    /// <summary>
    /// Utility to make registry snapshots.
    /// </summary>
    public static class DetectorUtilitiesRegistry
    {
        private static bool _isWin64 = Environment.Is64BitOperatingSystem;

        /// <summary>
        /// Makes a complete snapshot of all registry hives in both the Win64 view (if available) and the Win32 view.
        /// </summary>
        /// <returns>A snapshot</returns>
        public static LinkedList<SnapshotRegistryItem> MakeRegistrySnapshot()
        {
            LinkedList<SnapshotRegistryItem> results = new LinkedList<SnapshotRegistryItem>();

            DoRegistrySnapshotHive(RegistryHive.ClassesRoot, results);
            DoRegistrySnapshotHive(RegistryHive.CurrentConfig, results);
            DoRegistrySnapshotHive(RegistryHive.CurrentUser, results);
            DoRegistrySnapshotHive(RegistryHive.LocalMachine, results);
            DoRegistrySnapshotHive(RegistryHive.PerformanceData, results);
            DoRegistrySnapshotHive(RegistryHive.Users, results);

            return results;
        }

        /// <summary>
        /// Base method to start doing snapshots of a hive.
        /// </summary>
        /// <param name="hive">The hive to make a snapshot of.</param>
        /// <param name="results">The result object to fill out</param>
        private static void DoRegistrySnapshotHive(RegistryHive hive, LinkedList<SnapshotRegistryItem> results)
        {
            if (_isWin64)
            {
                using (RegistryKey currentProjectedKey = RegistryKey.OpenBaseKey(hive, RegistryView.Registry64))
                {
                    DoRegistrySnapshotKey(currentProjectedKey, RegistryView.Registry64, results);
                }
            }

            using (RegistryKey currentProjectedKey = RegistryKey.OpenBaseKey(hive, RegistryView.Registry32))
            {
                DoRegistrySnapshotKey(currentProjectedKey, RegistryView.Registry32, results);
            }
        }

        /// <summary>
        /// Recursively makes a snapshot of a key and its subkeys.
        /// </summary>
        /// <param name="currentKey">The key to snapshot next</param>
        /// <param name="view">The view currently used (Win64 or Win32)</param>
        /// <param name="results">The result object to fill out</param>
        private static void DoRegistrySnapshotKey(RegistryKey currentKey, RegistryView view, LinkedList<SnapshotRegistryItem> results)
        {
            // Values
            string[] valueKeys = currentKey.GetValueNames();

            foreach (string key in valueKeys)
            {
                SnapshotRegistryValue item = new SnapshotRegistryValue();
                item.FullPath = currentKey.Name + "\\" + key;
                item.RegistryView = view;

                try
                {
                    item.RegistryKeyType = currentKey.GetValueKind(key);
                    item.Value = ValueToString(item.RegistryKeyType, currentKey.GetValue(key));

                    item.WasReadable = true;
                }
                catch (Exception)
                {
                    item.WasReadable = false;
                }

                results.AddLast(item);
            }

            // Subkeys
            string[] subKeys = currentKey.GetSubKeyNames();

            foreach (string subKey in subKeys)
            {
                SnapshotRegistryKey item = new SnapshotRegistryKey();
                item.FullPath = currentKey.Name + "\\" + subKey;
                item.RegistryView = view;
                item.WasReadable = false;

                try
                {
                    using (RegistryKey sub = currentKey.OpenSubKey(subKey))
                    {
                        try
                        {
                            DoRegistrySnapshotKey(sub, view, results);
                        }
                        catch (Exception)
                        {
                            // Set item.WasReadable without taking subkeys exceptions into account
                        }
                    }

                    item.WasReadable = true;
                }
                catch (Exception)
                {
                    item.WasReadable = false;
                }

                results.AddLast(item);
            }
        }

        /// <summary>
        /// Helper method to turn any registry value into a string for comparisons.
        /// </summary>
        /// <returns>String representing the value.</returns>
        private static string ValueToString(RegistryValueKind kind, object value)
        {
            if (value == null)
                return string.Empty;

            switch (kind)
            {
                case RegistryValueKind.String:
                    return value as string;
                case RegistryValueKind.ExpandString:
                    return value as string;
                case RegistryValueKind.Binary:
                    byte[] valBinary = (byte[])value;

                    return BitConverter.ToString(valBinary).Replace("-", "");
                case RegistryValueKind.DWord:
                    return ((int)value).ToString();
                case RegistryValueKind.MultiString:
                    return string.Join("\r\n", (string[])value);
                case RegistryValueKind.QWord:
                    return ((long)value).ToString();
                case RegistryValueKind.Unknown:
                    return "unknown: " + value;
                case RegistryValueKind.None:
                    return string.Empty;
                default:
                    throw new ArgumentOutOfRangeException("kind");
            }
        }
    }
}