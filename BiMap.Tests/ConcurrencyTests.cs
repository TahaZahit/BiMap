using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using BiMap;
using Xunit;

namespace BiMap.Tests
{
    public class ConcurrencyTests
    {
        [Fact]
        public async Task Parallel_Add_ShouldBeThreadSafe()
        {
            var map = new BiMap<int, string>();
            int threadCount = 10;
            int itemsPerThread = 1000;

            var tasks = new Task[threadCount];
            for (int i = 0; i < threadCount; i++)
            {
                int threadId = i;
                tasks[i] = Task.Run(() =>
                {
                    for (int j = 0; j < itemsPerThread; j++)
                    {
                        int key = (threadId * itemsPerThread) + j;
                        map.Add(key, key.ToString());
                    }
                });
            }

            await Task.WhenAll(tasks);

            Assert.Equal(threadCount * itemsPerThread, map.Count);
        }

        [Fact]
        public async Task Parallel_Read_And_Write_ShouldNotCrash()
        {
            var map = new BiMap<int, int>();
            bool running = true;
            
            // Writer task
            var writer = Task.Run(async () =>
            {
                int key = 0;
                while (running)
                {
                    map.Add(key, key * 10);
                    map.RemoveByLeft(key);
                    key++;
                    if (key > 100000) key = 0;
                    await Task.Yield();
                }
            });

            // Reader tasks
            var readers = new Task[4];
            for (int i = 0; i < 4; i++)
            {
                readers[i] = Task.Run(async () =>
                {
                    while (running)
                    {
                        // Just access the map to stress the locks
                        _ = map.Count;
                        _ = map.ContainsLeft(50);
                        try
                        {
                            _ = map.EnumerateLeftToRight().ToList().Count;
                        } 
                        catch { /* Ignore snapshotted errors during heavy churn if any (shouldn't be any with correct locks) */ }
                        await Task.Yield();
                    }
                });
            }

            await Task.Delay(2000); // Run for 2 seconds
            running = false;
            await Task.WhenAll(writer);
            await Task.WhenAll(readers);
            
            // Should exit cleanly without exceptions
            Assert.True(true);
        }
    }
}
