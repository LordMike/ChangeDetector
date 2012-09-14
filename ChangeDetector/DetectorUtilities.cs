using System;
using System.Collections.Generic;

namespace ChangeDetector
{
    /// <summary>
    /// General utilities for the ChangeDetector.
    /// </summary>
    public static class DetectorUtilities
    {
        /// <summary>
        /// Retrieve a list of differences between two snapshots.
        /// </summary>
        /// <typeparam name="T">The type of snapshot to compare. The type must inherit both IEquatable and IKeyItem</typeparam>
        /// <param name="originalSnapshot">The original snapshot (the old version)</param>
        /// <param name="newSnapshot">The new snapshot (the newer version)</param>
        /// <returns>List of differences between the two.</returns>
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
}
