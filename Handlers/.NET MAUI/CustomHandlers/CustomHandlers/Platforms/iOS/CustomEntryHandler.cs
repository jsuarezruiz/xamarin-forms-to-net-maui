using Microsoft.Maui;
using Microsoft.Maui.Handlers;
using System;
using UIKit;

namespace CustomHandlers.Handlers
{
    public partial class CustomEntryHandler : ViewHandler<ICustomEntry, UIView>
    {
        protected override UIView CreateNativeView() => throw new NotImplementedException();

        [MissingMapper]
        public static void MapText(CustomEntryHandler handler, ICustomEntry entry) { }

        [MissingMapper]
        public static void MapTextColor(CustomEntryHandler handler, ICustomEntry entry) { }

        [MissingMapper]
        public static void MapPlaceholder(CustomEntryHandler handler, ICustomEntry entry) { }

        [MissingMapper]
        public static void MapPlaceholderColor(CustomEntryHandler handler, ICustomEntry entry) { }

        [MissingMapper]
        public static void MapCharacterSpacing(CustomEntryHandler handler, ICustomEntry entry) { }

        [MissingMapper]
        public static void MapHorizontalLayoutAlignment(CustomEntryHandler handler, ICustomEntry entry) { }

        [MissingMapper]
        public static void MapVerticalLayoutAlignment(CustomEntryHandler handler, ICustomEntry entry) { }
    }
}