using System;
using System.Collections.Generic;
using System.Security.Cryptography;
using Alphaleonis.Win32.Vss;
using Alphaleonis.Win32.Filesystem;

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
        public static SnapshotFilesystem MakeFsSnapshot(System.IO.DirectoryInfo baseDirectory)
        {
            DirectoryInfo baseDir = new DirectoryInfo(baseDirectory.FullName);

            if (!baseDir.Exists)
                throw new ArgumentException("Base directory doesn't exist");

            SnapshotFilesystem res = new SnapshotFilesystem();

            if (!baseDir.FullName.EndsWith(Path.DirectorySeparatorChar) && !baseDir.FullName.EndsWith(Path.AltDirectorySeparatorChar))
            {
                // Add ending slash to get a uniform output
                baseDir = new DirectoryInfo(baseDir.FullName + Path.DirectorySeparatorChar);
            }

            res.BasePath = baseDir.FullName;
            res.Items = new LinkedList<SnapshotFilesystemItem>();

            // Make VSS
            // Sequence of calls: http://us.generation-nt.com/answer/volume-shadow-copy-backupcomplete-vss-e-bad-state-help-29094302.html
            IVssImplementation vssImplementation = VssUtils.LoadImplementation();
            IVssBackupComponents backupComponents = vssImplementation.CreateVssBackupComponents();

            backupComponents.InitializeForBackup(null);
            backupComponents.SetContext(VssSnapshotContext.Backup);

            backupComponents.SetBackupState(false, false, VssBackupType.Copy, false);
            backupComponents.GatherWriterMetadata();

            try
            {
                Guid snapshotSetGuid = backupComponents.StartSnapshotSet();
                Guid backupVolumeGuid = backupComponents.AddToSnapshotSet(baseDir.Root.FullName);

                backupComponents.PrepareForBackup();
                backupComponents.DoSnapshotSet();

                VssSnapshotProperties properties = backupComponents.GetSnapshotProperties(backupVolumeGuid);

                DirectoryInfo shadowCopyBase = new DirectoryInfo(Path.Combine(properties.SnapshotDeviceObject, Path.GetDirectoryNameWithoutRoot(baseDir.FullName)));

                if (!shadowCopyBase.FullName.EndsWith(Path.DirectorySeparatorChar) && !shadowCopyBase.FullName.EndsWith(Path.AltDirectorySeparatorChar))
                {
                    // Add ending slash to get a uniform output
                    shadowCopyBase = new DirectoryInfo(shadowCopyBase.FullName + Path.DirectorySeparatorChar);
                }

                // Do stuff
                DoFsSnapshot(shadowCopyBase, shadowCopyBase, res.Items);

                // Delete snapshot
                backupComponents.BackupComplete();
                backupComponents.DeleteSnapshotSet(snapshotSetGuid, false);
            }
            catch (Exception)
            {
                backupComponents.AbortBackup();
            }

            //DoFsSnapshot(baseDirectory, baseDirectory, res.Items);

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
                current.Attributes = (System.IO.FileAttributes)currentDirectory.Attributes;

                current.WasReadable = true;
            }
            catch (System.IO.IOException)
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
                    file.RelativePath = fileInfo.FullName.Substring(baseDirectory.FullName.Length);

                    try
                    {
                        file.LastAccess = fileInfo.LastAccessTimeUtc;
                        file.LastModified = fileInfo.LastWriteTimeUtc;
                        file.Attributes = (System.IO.FileAttributes)fileInfo.Attributes;

                        // Make hash
                        using (System.IO.FileStream fileStream = fileInfo.OpenRead())
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
            catch (System.IO.IOException)
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
            catch (System.IO.IOException)
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