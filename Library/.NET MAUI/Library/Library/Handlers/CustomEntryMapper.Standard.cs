using Library.Controls;
using Microsoft.Maui.Handlers;

namespace Library.Handlers
{
    public partial class CustomEntryHandler : ViewHandler<ICustomEntry, object>
    {
        protected override object CreatePlatformView() => throw new NotImplementedException();

        public static void MapText(CustomEntryHandler handler, ICustomEntry entry) { }

        public static void MapTextColor(CustomEntryHandler handler, ICustomEntry entry) { }

        public static void MapPlaceholder(CustomEntryHandler handler, ICustomEntry entry) { }

        public static void MapPlaceholderColor(CustomEntryHandler handler, ICustomEntry entry) { }

        public static void MapCharacterSpacing(CustomEntryHandler handler, ICustomEntry entry) { }

        public static void MapHorizontalLayoutAlignment(CustomEntryHandler handler, ICustomEntry entry) { }

        public static void MapVerticalLayoutAlignment(CustomEntryHandler handler, ICustomEntry entry) { }
    }
}