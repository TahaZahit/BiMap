using System;
using System.Collections.Generic;
using System.Linq;
using BiMap;
using Xunit;

namespace BiMap.Tests
{
    public class CoreFunctionalityTests
    {
        [Fact]
        public void Add_ShouldAddKeyAndValue_WhenNotExists()
        {
            var map = new BiMap<int, string>();
            bool added = map.Add(1, "One");

            Assert.True(added);
            Assert.Equal("One", map.GetLeftToRight(1));
            Assert.Equal(1, map.GetRightToLeft("One"));
        }

        [Fact]
        public void Add_ShouldReturnFalse_WhenKeyExists()
        {
            var map = new BiMap<int, string>();
            map.Add(1, "One");
            
            bool added = map.Add(1, "Uno"); // Duplicate Left
            Assert.False(added);
            Assert.Equal("One", map.GetLeftToRight(1));
        }

        [Fact]
        public void Add_ShouldReturnFalse_WhenValueExists()
        {
            var map = new BiMap<int, string>();
            map.Add(1, "One");

            bool added = map.Add(2, "One"); // Duplicate Right
            Assert.False(added);
            Assert.False(map.ContainsLeft(2));
        }

        [Fact]
        public void RemoveByLeft_ShouldRemoveBothSides()
        {
            var map = new BiMap<int, string>();
            map.Add(1, "One");

            bool removed = map.RemoveByLeft(1);

            Assert.True(removed);
            Assert.False(map.ContainsLeft(1));
            Assert.False(map.ContainsRight("One"));
            Assert.Equal(0, map.Count);
        }

        [Fact]
        public void RemoveByRight_ShouldRemoveBothSides()
        {
            var map = new BiMap<int, string>();
            map.Add(1, "One");

            bool removed = map.RemoveByRight("One");

            Assert.True(removed);
            Assert.False(map.ContainsLeft(1));
            Assert.False(map.ContainsRight("One"));
            Assert.Equal(0, map.Count);
        }

        [Fact]
        public void Clear_ShouldRemoveAllItems()
        {
            var map = new BiMap<int, string>();
            map.Add(1, "One");
            map.Add(2, "Two");

            map.Clear();

            Assert.Equal(0, map.Count);
            Assert.False(map.ContainsLeft(1));
            Assert.False(map.ContainsRight("One"));
        }

        [Fact]
        public void Enumerate_ShouldReturnAllItems()
        {
            var map = new BiMap<int, string>();
            map.Add(1, "One");
            map.Add(2, "Two");

            var items = map.EnumerateLeftToRight().ToList();

            Assert.Equal(2, items.Count);
            Assert.Contains(items, kv => kv.Key == 1 && kv.Value == "One");
            Assert.Contains(items, kv => kv.Key == 2 && kv.Value == "Two");
        }
    }
}
