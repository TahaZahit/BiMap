using System;
using System.Collections.Generic;

namespace BiMap
{
    /// <summary>
    /// Represents a thread-safe generic bidirectional map.
    /// </summary>
    /// <typeparam name="TLeft">The type of the left key.</typeparam>
    /// <typeparam name="TRight">The type of the right key.</typeparam>
    public interface IBiMap<TLeft, TRight> : IDisposable
    {
        /// <summary>
        /// Gets the number of pairs in the map.
        /// </summary>
        int Count { get; }

        /// <summary>
        /// Adds a pair to the map. Returns false if either key already exists.
        /// </summary>
        bool Add(TLeft left, TRight right);

        /// <summary>
        /// Removes a pair by the left key.
        /// </summary>
        bool RemoveByLeft(TLeft left);

        /// <summary>
        /// Removes a pair by the right key.
        /// </summary>
        bool RemoveByRight(TRight right);

        /// <summary>
        /// Clears the map.
        /// </summary>
        void Clear();

        /// <summary>
        /// Checks if the left key exists.
        /// </summary>
        bool ContainsLeft(TLeft left);

        /// <summary>
        /// Checks if the right key exists.
        /// </summary>
        bool ContainsRight(TRight right);

        /// <summary>
        /// Gets the right value associated with the left key.
        /// </summary>
        TRight GetLeftToRight(TLeft left);

        /// <summary>
        /// Tries to get the right value associated with the left key.
        /// </summary>
        bool TryGetLeftToRight(TLeft left, out TRight right);

        /// <summary>
        /// Gets the left value associated with the right key.
        /// </summary>
        TLeft GetRightToLeft(TRight right);

        /// <summary>
        /// Tries to get the left value associated with the right key.
        /// </summary>
        bool TryGetRightToLeft(TRight right, out TLeft left);

        /// <summary>
        /// Returns a thread-safe snapshot of the Left-to-Right mappings.
        /// </summary>
        IEnumerable<KeyValuePair<TLeft, TRight>> EnumerateLeftToRight();

        /// <summary>
        /// Returns a thread-safe snapshot of the Right-to-Left mappings.
        /// </summary>
        IEnumerable<KeyValuePair<TRight, TLeft>> EnumerateRightToLeft();
    }
}
