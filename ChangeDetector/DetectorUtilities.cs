using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Formatters.Binary;
using System.Security.Cryptography;
using Microsoft.Win32;

namespace ChangeDetector
{
    public static class DetectorUtilities
    {
        public static List<Difference<T>> GetDifferences<T>(ICollection<T> originalSnapshot, ICollection<T> newSnapshot) where T : class, IKeyItem, IEquatable<T>
        {
            List<Difference<T>> res = new List<Difference<T>>();

            // Prep
            Dictionary<string, T> newItems = new Dictionary<string, T>();

            foreach (T item in newSnapshot)
            {
                newItems.Add(item.PrimaryKey, item);
            }

            // Iterate old
            foreach (T original in originalSnapshot)
            {
                T newItem;
                if (!newItems.TryGetValue(original.PrimaryKey, out newItem))
                {
                    // Deleted
                    Difference<T> difference = new Difference<T>();

                    difference.DifferenceType = DifferenceType.Deleted;
                    difference.Original = original;
                    difference.New = null;

                    res.Add(difference);
                }
                else
                {
                    if (!newItem.Equals(original))
                    {
                        // Modified
                        Difference<T> difference = new Difference<T>();

                        difference.DifferenceType = DifferenceType.Modified;
                        difference.Original = original;
                        difference.New = newItem;

                        res.Add(difference);
                    }

                    // Remove it from newItems
                    newItems.Remove(newItem.PrimaryKey);
                }
            }

            // Iterate remaining new items
            foreach (KeyValuePair<string, T> newItem in newItems)
            {
                // Added
                Difference<T> difference = new Difference<T>();

                difference.DifferenceType = DifferenceType.Added;
                difference.Original = null;
                difference.New = newItem.Value;

                res.Add(difference);
            }

            return res;
        }
    }

    public static class DetectorUtilitiesFS
    {
        private static MD5CryptoServiceProvider _md5 = new MD5CryptoServiceProvider();

        public static LinkedList<SnapshotFilesystemItem> MakeFsSnapshot(DirectoryInfo baseDirectory)
        {
            if (!baseDirectory.Exists)
                throw new ArgumentException("Base directory doesn't exist");

            LinkedList<SnapshotFilesystemItem> res = new LinkedList<SnapshotFilesystemItem>();
            DoFsSnapshot(baseDirectory, res);

            return res;
        }

        private static void DoFsSnapshot(DirectoryInfo currentDirectory, LinkedList<SnapshotFilesystemItem> results)
        {
            // Self
            SnapshotDirectoryInfo current = new SnapshotDirectoryInfo();
            current.FullName = currentDirectory.FullName;

            try
            {
                current.LastAccess = currentDirectory.LastAccessTimeUtc;
                current.LastModified = currentDirectory.LastWriteTimeUtc;
                current.Attributes = currentDirectory.Attributes;

                current.WasReadable = true;
            }
            catch (IOException)
            {
                // Couldn't read details
                current.WasReadable = false;
            }

            // Subfiles
            try
            {
                FileInfo[] files = currentDirectory.GetFiles();

                foreach (FileInfo fileInfo in files)
                {
                    // Single file
                    SnapshotFileInfo file = new SnapshotFileInfo();
                    file.FullName = fileInfo.FullName;

                    try
                    {
                        file.LastAccess = fileInfo.LastAccessTimeUtc;
                        file.LastModified = fileInfo.LastWriteTimeUtc;
                        file.Attributes = fileInfo.Attributes;

                        // Make hash
                        using (FileStream fileStream = fileInfo.OpenRead())
                        {
                            byte[] hash = _md5.ComputeHash(fileStream);
                            file.Hash = BitConverter.ToString(hash).Replace("-", "");
                        }

                        file.WasReadable = true;
                    }
                    catch (Exception)
                    {
                        // Couldn't read details
                        file.WasReadable = false;
                    }

                    results.AddLast(file);
                }
            }
            catch (IOException)
            {
                // Couldn't read files
                current.WasReadable = false;
            }

            // Subdirs
            try
            {
                DirectoryInfo[] directories = currentDirectory.GetDirectories();

                foreach (DirectoryInfo directoryInfo in directories)
                {
                    DoFsSnapshot(directoryInfo, results);
                }
            }
            catch (IOException)
            {
                // Couldn't read directories
                current.WasReadable = false;
            }

            results.AddLast(current);
        }
    }

    public static class DetectorUtilitiesRegistry
    {
        private static bool _isWin64 = Environment.Is64BitOperatingSystem;

        public static LinkedList<SnapshotRegistryItem> MakeRegistrySnapshot()
        {
            LinkedList<SnapshotRegistryItem> results = new LinkedList<SnapshotRegistryItem>();

            DoRegistrySnapshotKey(RegistryHive.ClassesRoot, results);
            DoRegistrySnapshotKey(RegistryHive.CurrentConfig, results);
            DoRegistrySnapshotKey(RegistryHive.CurrentUser, results);
            DoRegistrySnapshotKey(RegistryHive.LocalMachine, results);
            DoRegistrySnapshotKey(RegistryHive.PerformanceData, results);
            DoRegistrySnapshotKey(RegistryHive.Users, results);

            return results;
        }

        private static void DoRegistrySnapshotKey(RegistryHive hive, LinkedList<SnapshotRegistryItem> results)
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

                    if (item.Value.Length > 20)
                        item.Value = item.Value.Substring(0, 20) + "...";

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

    public interface IKeyItem
    {
        string PrimaryKey { get; }
    }

    public class Difference<T> where T : IKeyItem
    {
        public DifferenceType DifferenceType { get; set; }
        public T Original { get; set; }
        public T New { get; set; }
    }

    public enum DifferenceType
    {
        Added, Modified, Deleted
    }

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

    [Serializable]
    public class SnapshotRegistryKey : SnapshotRegistryItem
    {

    }

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


    [Serializable]
    public abstract class SnapshotFilesystemItem : IEquatable<SnapshotFilesystemItem>, IKeyItem
    {
        public string FullName { get; set; }
        public DateTime LastModified { get; set; }
        public DateTime LastAccess { get; set; }
        public FileAttributes Attributes { get; set; }
        public bool WasReadable { get; set; }

        public bool Equals(SnapshotFilesystemItem other)
        {
            if (other == null)
                return false;

            return FullName.Equals(other.FullName, StringComparison.InvariantCultureIgnoreCase)
                   && LastModified == other.LastModified
                   && LastAccess == other.LastAccess
                   && Attributes == other.Attributes
                   && WasReadable == other.WasReadable;
        }

        public string PrimaryKey
        {
            get { return FullName; }
        }
    }

    [Serializable]
    public class SnapshotFileInfo : SnapshotFilesystemItem, IEquatable<SnapshotFileInfo>
    {
        public string Hash { get; set; }

        public bool Equals(SnapshotFileInfo other)
        {
            return base.Equals(other) && Hash == other.Hash;
        }
    }

    [Serializable]
    public class SnapshotDirectoryInfo : SnapshotFilesystemItem, IEquatable<SnapshotFileInfo>
    {
        public bool Equals(SnapshotFileInfo other)
        {
            return base.Equals(other);
        }
    }
}
