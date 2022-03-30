using Library.Controls;
using Microsoft.Maui.Handlers;

namespace Library.Handlers
{
    public partial class CustomEntryHandler 
    {
        public static PropertyMapper<ICustomEntry, CustomEntryHandler> CustomEntryMapper = new PropertyMapper<ICustomEntry, CustomEntryHandler>(ViewHandler.ViewMapper)
        {
            [nameof(ICustomEntry.Text)] = MapText,
            [nameof(ICustomEntry.TextColor)] = MapTextColor,
            [nameof(ICustomEntry.Placeholder)] = MapPlaceholder,
            [nameof(ICustomEntry.PlaceholderColor)] = MapPlaceholderColor,
            [nameof(ICustomEntry.CharacterSpacing)] = MapCharacterSpacing,
            [nameof(ICustomEntry.HorizontalLayoutAlignment)] = MapHorizontalLayoutAlignment,
            [nameof(ICustomEntry.VerticalLayoutAlignment)] = MapVerticalLayoutAlignment
        };

        public CustomEntryHandler() : base(CustomEntryMapper)
        {

        }

        public CustomEntryHandler(PropertyMapper mapper = null) : base(mapper ?? CustomEntryMapper)
        {

        }
    }
}