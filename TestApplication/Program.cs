using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Formatters.Binary;
using ChangeDetector;

namespace TestApplication
{
    class Program
    {
        static void Main()
        {
            string tmpFile = Path.GetFullPath("tmp");

            SnapshotFilesystem oldSnapshot = null;
            if (File.Exists(tmpFile))
            {
                oldSnapshot = DeSerializeSingleObject<SnapshotFilesystem>(tmpFile);
            }

            SnapshotFilesystem snapshot = DetectorUtilitiesFS.MakeFsSnapshot(new DirectoryInfo(@"C:\Windows\System32"));
            SerializeSingleObject(tmpFile, snapshot);

            Console.WriteLine(snapshot.Items.Count + " items");

            if (oldSnapshot != null)
            {
                List<Difference<SnapshotFilesystemItem>> differences = DetectorUtilities.GetDifferences(oldSnapshot.Items, snapshot.Items);
                Console.WriteLine("Differences ({0}): ", differences.Count);

                foreach (Difference<SnapshotFilesystemItem> difference in differences)
                {
                    Console.WriteLine("Difference ({0}):", difference.DifferenceType);
                    Console.WriteLine("  Path: {0}", (difference.Original ?? difference.New).RelativePath);

                    WriteDiffToConsole(difference.Original, difference.New, "LastAccess", filesystem => filesystem.LastAccess);
                    WriteDiffToConsole(difference.Original, difference.New, "LastModified", filesystem => filesystem.LastModified);
                    WriteDiffToConsole(difference.Original, difference.New, "Attributes", filesystem => filesystem.Attributes);
                    WriteDiffToConsole(difference.Original, difference.New, "Was Readable", filesystem => filesystem.WasReadable);

                    if (difference.Original is SnapshotFileInfo || difference.New is SnapshotFileInfo)
                    {
                        WriteDiffToConsole(difference.Original as SnapshotFileInfo, difference.New as SnapshotFileInfo, "Hash", filesystem => filesystem.Hash);
                    }

                    Console.WriteLine();
                }
            }

            SnapshotRegistry oldRegSnapshot = null;
            if (File.Exists(tmpFile))
            {
                oldRegSnapshot = DeSerializeSingleObject<SnapshotRegistry>(tmpFile);
            }

            SnapshotRegistry regSnapshot = DetectorUtilitiesRegistry.MakeRegistrySnapshot();
            SerializeSingleObject(tmpFile, regSnapshot);

            Console.WriteLine(regSnapshot.PartialSnapshots.Sum(s => s.Items.Count) + " items");

            if (oldRegSnapshot != null)
            {
                // Iterate each hive
                foreach (SnapshotRegistryPartial registrySnapshot in oldRegSnapshot.PartialSnapshots)
                {
                    SnapshotRegistryPartial corresponding = regSnapshot.PartialSnapshots.Single(s => s.BasePath == registrySnapshot.BasePath);

                    Console.WriteLine("RegistryHive ({0} vs. {1} items): {2}", registrySnapshot.Items.Count, corresponding.Items.Count, registrySnapshot.BasePath);

                    List<Difference<SnapshotRegistryItem>> differences = DetectorUtilities.GetDifferences(registrySnapshot.Items, corresponding.Items);
                    Console.WriteLine("Differences ({0}): ", differences.Count);

                    foreach (Difference<SnapshotRegistryItem> difference in differences)
                    {
                        Console.WriteLine("Difference ({0}):", difference.DifferenceType);
                        Console.WriteLine("  Path: {0}", (difference.Original ?? difference.New).FullPath);

                        WriteDiffToConsole(difference.Original, difference.New, "RegistryView", filesystem => filesystem.RegistryView);
                        WriteDiffToConsole(difference.Original, difference.New, "Was Readable", filesystem => filesystem.WasReadable);

                        if (difference.Original is SnapshotRegistryValue || difference.New is SnapshotRegistryValue)
                        {
                            WriteDiffToConsole(difference.Original as SnapshotRegistryValue, difference.New as SnapshotRegistryValue, "RegistryKeyType", filesystem => filesystem.RegistryKeyType);
                            WriteDiffToConsole(difference.Original as SnapshotRegistryValue, difference.New as SnapshotRegistryValue, "Value", filesystem => filesystem.Value);
                        }

                        Console.WriteLine();
                    }

                    Console.WriteLine();
                    Console.WriteLine();
                }
            }

            Console.WriteLine("Done");
            Console.ReadLine();
        }

        private static void WriteDiffToConsole<T, TV>(T originalItem, T newItem, string itemName, Func<T, TV> itemSelector)
        {
            string originalString = originalItem != null && itemSelector(originalItem) != null ? itemSelector(originalItem).ToString() : "(none)";
            string newString = newItem != null && itemSelector(newItem) != null ? itemSelector(newItem).ToString() : "(none)";

            Console.WriteLine("  {3}: {0} -> {1} {2}", originalString, newString, newString != originalString ? "(modified)" : "", itemName);
        }

        public static void SerializeSingleObject<T>(string filename, T obj)
        {
            using (FileStream fileStream = File.OpenWrite(filename))
            {
                BinaryFormatter formatter = new BinaryFormatter();
                formatter.Serialize(fileStream, obj);
            }
        }

        public static T DeSerializeSingleObject<T>(string filename)
        {
            using (FileStream fileStream = File.OpenRead(filename))
            {
                BinaryFormatter formatter = new BinaryFormatter();
                return (T)formatter.Deserialize(fileStream);
            }
        }

        public static void SerializeObject<T>(string filename, ICollection<T> obj)
        {
            const int chunkSize = 100000;

            // Serialize into chunks of ChunkSize
            using (FileStream fileStream = File.OpenWrite(filename))
            {
                BinaryFormatter formatter = new BinaryFormatter();

                foreach (IEnumerable<T> chunk in obj.Chunk(chunkSize))
                {
                    formatter.Serialize(fileStream, chunk.ToList());
                }

            }
        }

        public static ICollection<T> DeSerializeObject<T>(string filename)
        {
            LinkedList<T> result = new LinkedList<T>();

            using (FileStream fileStream = File.OpenRead(filename))
            {
                BinaryFormatter formatter = new BinaryFormatter();

                while (fileStream.Position < fileStream.Length)
                {
                    try
                    {
                        ICollection<T> chunk = (ICollection<T>)formatter.Deserialize(fileStream);

                        foreach (T item in chunk)
                            result.AddLast(item);
                    }
                    catch (IOException)
                    {
                        break;
                    }
                }
            }

            return result;
        }
    }

    public static class LinqExtensions
    {
        public static IEnumerable<IEnumerable<T>> Chunk<T>(this IEnumerable<T> source, int chunksize)
        {
            while (source.Any())
            {
                yield return source.Take(chunksize);
                source = source.Skip(chunksize);
            }
        }
    }
}
