using System.Collections.Generic;
using CriticalCommonLib.Services;
using FFXIVClientStructs.FFXIV.Client.Game;

namespace InventoryTools.Extensions
{
    /// <summary>
    /// TW port: 64-bit item totals.
    /// IInventoryMonitor.ItemCounts sums every location into a 32-bit int, which overflows
    /// once a total passes 2,147,483,647 (e.g. gil across many characters and retainers).
    /// CharacterItemCounts is keyed per character/retainer, so each entry stays small;
    /// here those entries are re-summed into a long and cached until the monitor
    /// regenerates its counts (which replaces the dictionary instance).
    /// </summary>
    public static class ItemCountExtensions
    {
        private static readonly object Lock = new();
        private static object? _source;
        private static Dictionary<(uint, InventoryItem.ItemFlags), long> _totals = new();

        public static long GetTotalItemCount(this IInventoryMonitor inventoryMonitor, uint itemId, InventoryItem.ItemFlags flags)
        {
            var source = inventoryMonitor.CharacterItemCounts;
            lock (Lock)
            {
                if (!ReferenceEquals(source, _source))
                {
                    var totals = new Dictionary<(uint, InventoryItem.ItemFlags), long>();
                    foreach (var entry in source)
                    {
                        var key = (entry.Key.Item1, entry.Key.Item2);
                        totals.TryGetValue(key, out var current);
                        totals[key] = current + entry.Value;
                    }

                    _totals = totals;
                    _source = source;
                }

                return _totals.GetValueOrDefault((itemId, flags), 0);
            }
        }
    }
}
