using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;

namespace BiMap
{
    /// <summary>
    /// A thread-safe, bidirectional map allowing mapping between two unique keys.
    /// Implementation targets .NET Standard 2.0.
    /// </summary>
    /// <typeparam name="TLeft">The type of the left key.</typeparam>
    /// <typeparam name="TRight">The type of the right key.</typeparam>
    public class BiMap<TLeft, TRight> : IBiMap<TLeft, TRight>
    {
        private readonly Dictionary<TLeft, TRight> _leftToRight;
        private readonly Dictionary<TRight, TLeft> _rightToLeft;
        private readonly ReaderWriterLockSlim _lock;
        private bool _disposed;

        /// <summary>
        /// Gets the number of elements in the BiMap.
        /// </summary>
        public int Count
        {
            get
            {
                _lock.EnterReadLock();
                try
                {
                    return _leftToRight.Count;
                }
                finally
                {
                    _lock.ExitReadLock();
                }
            }
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="BiMap{TLeft, TRight}"/> class.
        /// </summary>
        /// <param name="leftComp">Equality comparer for the left key.</param>
        /// <param name="rightComp">Equality comparer for the right key.</param>
        public BiMap(IEqualityComparer<TLeft> leftComp = null, IEqualityComparer<TRight> rightComp = null)
        {
            _leftToRight = new Dictionary<TLeft, TRight>(leftComp);
            _rightToLeft = new Dictionary<TRight, TLeft>(rightComp);
            _lock = new ReaderWriterLockSlim(LockRecursionPolicy.NoRecursion);
        }

        /// <summary>
        /// Attempts to add a pair to the map. Returns false if either key already exists.
        /// </summary>
        public bool Add(TLeft left, TRight right)
        {
            _lock.EnterWriteLock();
            try
            {
                if (_leftToRight.ContainsKey(left) || _rightToLeft.ContainsKey(right))
                {
                    return false;
                }

                _leftToRight.Add(left, right);
                _rightToLeft.Add(right, left);
                return true;
            }
            finally
            {
                _lock.ExitWriteLock();
            }
        }
        
        // Backward compatibility alias likely expected by existing code if reusing class.
        public bool AddPair(TLeft left, TRight right) => Add(left, right);

        /// <summary>
        /// Removes the mapping associated with the specified left key.
        /// </summary>
        public bool RemoveByLeft(TLeft left)
        {
            _lock.EnterWriteLock();
            try
            {
                if (_leftToRight.TryGetValue(left, out TRight right))
                {
                    _leftToRight.Remove(left);
                    _rightToLeft.Remove(right);
                    return true;
                }
                return false;
            }
            finally
            {
                _lock.ExitWriteLock();
            }
        }

        /// <summary>
        /// Removes the mapping associated with the specified right key.
        /// </summary>
        public bool RemoveByRight(TRight right)
        {
            _lock.EnterWriteLock();
            try
            {
                if (_rightToLeft.TryGetValue(right, out TLeft left))
                {
                    _rightToLeft.Remove(right);
                    _leftToRight.Remove(left);
                    return true;
                }
                return false;
            }
            finally
            {
                _lock.ExitWriteLock();
            }
        }

        /// <summary>
        /// Clears all mappings.
        /// </summary>
        public void Clear()
        {
            _lock.EnterWriteLock();
            try
            {
                _leftToRight.Clear();
                _rightToLeft.Clear();
            }
            finally
            {
                _lock.ExitWriteLock();
            }
        }

        /// <summary>
        /// Determines whether the map contains the specified left key.
        /// </summary>
        public bool ContainsLeft(TLeft left)
        {
            _lock.EnterReadLock();
            try
            {
                return _leftToRight.ContainsKey(left);
            }
            finally
            {
                _lock.ExitReadLock();
            }
        }

        /// <summary>
        /// Determines whether the map contains the specified right key.
        /// </summary>
        public bool ContainsRight(TRight right)
        {
            _lock.EnterReadLock();
            try
            {
                return _rightToLeft.ContainsKey(right);
            }
            finally
            {
                _lock.ExitReadLock();
            }
        }

        /// <summary>
        /// Gets the right value associated with the specified left key. Throws KeyNotFoundException if not found.
        /// </summary>
        public TRight GetLeftToRight(TLeft key)
        {
            _lock.EnterReadLock();
            try
            {
                if (!_leftToRight.TryGetValue(key, out TRight result))
                    throw new KeyNotFoundException($"Key: {key} not found");
                return result;
            }
            finally
            {
                _lock.ExitReadLock();
            }
        }

        /// <summary>
        /// Tries to get the right value associated with the specified left key.
        /// </summary>
        public bool TryGetLeftToRight(TLeft key, out TRight value)
        {
            _lock.EnterReadLock();
            try
            {
                return _leftToRight.TryGetValue(key, out value);
            }
            finally
            {
                _lock.ExitReadLock();
            }
        }

        /// <summary>
        /// Gets the left value associated with the specified right key. Throws KeyNotFoundException if not found.
        /// </summary>
        public TLeft GetRightToLeft(TRight key)
        {
            _lock.EnterReadLock();
            try
            {
                if (!_rightToLeft.TryGetValue(key, out TLeft result))
                    throw new KeyNotFoundException($"Key: {key} not found");
                return result;
            }
            finally
            {
                _lock.ExitReadLock();
            }
        }

        /// <summary>
        /// Tries to get the left value associated with the specified right key.
        /// </summary>
        public bool TryGetRightToLeft(TRight key, out TLeft value)
        {
            _lock.EnterReadLock();
            try
            {
                return _rightToLeft.TryGetValue(key, out value);
            }
            finally
            {
                _lock.ExitReadLock();
            }
        }

        /// <summary>
        /// Returns a thread-safe snapshot of the Left-to-Right mappings.
        /// </summary>
        public IEnumerable<KeyValuePair<TLeft, TRight>> EnumerateLeftToRight()
        {
            _lock.EnterReadLock();
            try
            {
                return new List<KeyValuePair<TLeft, TRight>>(_leftToRight);
            }
            finally
            {
                _lock.ExitReadLock();
            }
        }

        /// <summary>
        /// Returns a thread-safe snapshot of the Right-to-Left mappings.
        /// </summary>
        public IEnumerable<KeyValuePair<TRight, TLeft>> EnumerateRightToLeft()
        {
            _lock.EnterReadLock();
            try
            {
                return new List<KeyValuePair<TRight, TLeft>>(_rightToLeft);
            }
            finally
            {
                _lock.ExitReadLock();
            }
        }

        /// <summary>
        /// Disposes the ReaderWriterLockSlim.
        /// </summary>
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (!_disposed)
            {
                if (disposing)
                {
                    _lock?.Dispose();
                }
                _disposed = true;
            }
        }
    }
}
