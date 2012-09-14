using System;
using System.Collections.Generic;
using System.IO;
using System.Security.Cryptography;

namespace ChangeDetector
{
    /// <summary>
    /// Utility to make filesystem snapshots.
    /// </summary>
    public static class DetectorUtilitiesFS
    {
        private static MD5CryptoServiceProvider _md5 = new MD5CryptoServiceProvider();

        /// <summary>
        /// Makes a recursive snapshot of the directory as it is.
        /// </summary>
        /// <param name="baseDirectory">The directory to start in.</param>
        /// <returns>A file system snapshot</returns>
        public static SnapshotFilesystem MakeFsSnapshot(DirectoryInfo baseDirectory)
        {
            if (!baseDirectory.Exists)
                throw new ArgumentException("Base directory doesn't exist");

            SnapshotFilesystem res = new SnapshotFilesystem();

            if (!baseDirectory.FullName.EndsWith(Path.DirectorySeparatorChar.ToString()) && !baseDirectory.FullName.EndsWith(Path.AltDirectorySeparatorChar.ToString()))
            {
                // Add ending slash to get a uniform output
                baseDirectory = new DirectoryInfo(baseDirectory.FullName + Path.DirectorySeparatorChar);
            }

            res.BasePath = baseDirectory.FullName;
            res.Items = new LinkedList<SnapshotFilesystemItem>();

            DoFsSnapshot(baseDirectory, baseDirectory, res.Items);

            return res;
        }

        /// <summary>
        /// Internal method to recursively iterate the file system.
        /// It will fill out the results parameter with items as it finds them.
        /// </summary>
        /// <param name="baseDirectory">Base directory is used to make relative paths. It should always end with a '\' (slash) to generate a uniform and valid output.</param>
        /// <param name="currentDirectory">The directory to iterate.</param>
        /// <param name="results">The results LinkedList to fill out.</param>
        private static void DoFsSnapshot(DirectoryInfo baseDirectory, DirectoryInfo currentDirectory, LinkedList<SnapshotFilesystemItem> results)
        {
            // Self
            SnapshotDirectoryInfo current = new SnapshotDirectoryInfo();
            current.RelativePath = currentDirectory.FullName.Substring(baseDirectory.FullName.Length);

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
                    current.RelativePath = fileInfo.FullName.Substring(baseDirectory.FullName.Length);

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
            catch (UnauthorizedAccessException)
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
                    DoFsSnapshot(baseDirectory, directoryInfo, results);
                }
            }
            catch (IOException)
            {
                // Couldn't read directories
                current.WasReadable = false;
            }
            catch (UnauthorizedAccessException)
            {
                // Couldn't read files
                current.WasReadable = false;
            }

            results.AddLast(current);
        }
    }
}