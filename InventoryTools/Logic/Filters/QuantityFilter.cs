using AllaganLib.GameSheets.Sheets.Rows;
using AllaganLib.Shared.Extensions;
using CriticalCommonLib.Models;
using CriticalCommonLib.Services;

using InventoryTools.Extensions;
using InventoryTools.Logic.Filters.Abstract;
using InventoryTools.Services;
using Microsoft.Extensions.Logging;

namespace InventoryTools.Logic.Filters
{
    public class QuantityFilter : StringFilter
    {
        private readonly IInventoryMonitor _inventoryMonitor;

        public QuantityFilter(ILogger<QuantityFilter> logger, ImGuiService imGuiService, IInventoryMonitor inventoryMonitor) : base(logger, imGuiService)
        {
            _inventoryMonitor = inventoryMonitor;
            ShowOperatorTooltip = true;
        }
        public override string Key { get; set; } = "Qty";
        public override string Name { get; set; } = "Quantity";
        public override string HelpText { get; set; } = "The quantity of the item.";

        public override FilterCategory FilterCategory { get; set; } = FilterCategory.Basic;

        public override bool? FilterItem(FilterConfiguration configuration,InventoryItem item)
        {
            var currentValue = CurrentValue(configuration);
            if (!string.IsNullOrEmpty(currentValue))
            {
                if (!item.Quantity.PassesFilter(currentValue.ToLower()))
                {
                    return false;
                }
            }

            return true;
        }

        public override bool? FilterItem(FilterConfiguration configuration, ItemRow item)
        {
            var currentValue = CurrentValue(configuration);
            if (!string.IsNullOrEmpty(currentValue))
            {
                // TW port: summed as 64-bit so totals above 2,147,483,647 (e.g. gil) don't overflow.
                long qty = _inventoryMonitor.GetTotalItemCount(item.RowId, FFXIVClientStructs.FFXIV.Client.Game.InventoryItem.ItemFlags.None)
                           + _inventoryMonitor.GetTotalItemCount(item.RowId, FFXIVClientStructs.FFXIV.Client.Game.InventoryItem.ItemFlags.HighQuality);
                if (!((decimal)qty).PassesFilter(currentValue.ToLower()))
                {
                    return false;
                }
            }

            return true;
        }
    }
}