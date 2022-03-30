#nullable enable
using Android.Content.Res;
using Android.Widget;
using Library.Controls;
using Microsoft.Maui.Platform;
using AResource = Android.Resource;

namespace Library.Extensions.Android
{
    public static class CustomEntryExtensions
    {
        static readonly int[][] ColorStates =
        {
            new[] { AResource.Attribute.StateEnabled },
            new[] { -AResource.Attribute.StateEnabled }
        };

        public static void UpdateText(this EditText editText, ICustomEntry entry)
        {
            editText.Text = entry.Text;
        }

        public static void UpdateTextColor(this EditText editText, ICustomEntry entry, ColorStateList? defaultTextColors)
        {
            var textColor = entry.TextColor;

            if (textColor == null)
            {
                if (defaultTextColors != null)
                    editText.SetTextColor(defaultTextColors);
            }
            else
            {
                var androidColor = textColor.ToPlatform();

                if (!editText.TextColors.IsOneColor(ColorStates, androidColor))
                {
                    var acolor = androidColor.ToArgb();
                    editText.SetTextColor(new ColorStateList(ColorStates, new[] { acolor, acolor }));
                }
            }
        }

        public static void UpdatePlaceholder(this EditText editText, ICustomEntry entry)
        {
            if (editText.Hint == entry.Placeholder)
                return;

            editText.Hint = entry.Placeholder;
        }

        public static void UpdatePlaceholderColor(this EditText editText, ICustomEntry entry, ColorStateList? defaultPlaceholderColors)
        {
            var placeholderTextColor = entry.PlaceholderColor;

            if (placeholderTextColor == null)
            {
                editText.SetHintTextColor(defaultPlaceholderColors);
            }
            else
            {
                var androidColor = placeholderTextColor.ToPlatform();

                if (!editText.HintTextColors.IsOneColor(ColorStates, androidColor))
                {
                    var acolor = androidColor.ToArgb();
                    editText.SetHintTextColor(new ColorStateList(ColorStates, new[] { acolor, acolor }));
                }
            }
        }

        public static void UpdateCharacterSpacing(this EditText editText, ICustomEntry entry)
        {
            editText.LetterSpacing = entry.CharacterSpacing.ToEm();
        }

        public static void UpdateHorizontalLayoutAlignment(this EditText editText, ICustomEntry entry)
        {
            editText.UpdateTextAlignment(entry.HorizontalTextAlignment, entry.VerticalTextAlignment);
        }

        public static void UpdateVerticaLayoutAlignment(this EditText editText, ICustomEntry entry)
        {
            editText.UpdateTextAlignment(entry.HorizontalTextAlignment, entry.VerticalTextAlignment);
        }
    }
}
