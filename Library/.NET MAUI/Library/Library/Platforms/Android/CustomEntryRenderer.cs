using Android.Content;
using Android.Content.Res;
using Android.Text;
using Android.Widget;
using Java.Lang;
using Library.Extensions.Android;
using Library.Controls;
using Library.Renderers.Android;
using Microsoft.Maui.Controls.Compatibility;
using Microsoft.Maui.Controls.Compatibility.Platform.Android;
using Microsoft.Maui.Controls.Platform;
using System.ComponentModel;
using AResource = Android.Resource;

[assembly: ExportRenderer(typeof(CustomEntry), typeof(CustomEntryRenderer))]
namespace Library.Renderers.Android
{
    public class CustomEntryRenderer : ViewRenderer<CustomEntry, EditText>, ITextWatcher
    {
        readonly int[][] ColorStates =
        {
            new[] { AResource.Attribute.StateEnabled },
            new[] { -AResource.Attribute.StateEnabled }
        };

        ColorStateList _defaultTextColors;
        ColorStateList _defaultPlaceholderColors;

        public CustomEntryRenderer(Context context) : base(context)
        {

        }

        protected override void OnElementChanged(ElementChangedEventArgs<CustomEntry> e)
        {
            base.OnElementChanged(e);

            if (e.OldElement == null)
            {
                EditText editText = new EditText(Context);

                _defaultTextColors = editText.TextColors;
                _defaultPlaceholderColors = editText.HintTextColors;

                SetNativeControl(editText);
            }

            UpdateText();
            UpdateTextColor();
            UpdatePlaceholder();
            UpdatePlaceholderColor();
            UpdateCharacterSpacing();
            UpdateHorizontalTextAlignment();
            UpdateVerticalTextAlignment();
        }

        protected override void OnElementPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName == CustomEntry.TextProperty.PropertyName)
                UpdateText();
            else if (e.PropertyName == CustomEntry.TextColorProperty.PropertyName)
                UpdateTextColor();
            if (e.PropertyName == CustomEntry.PlaceholderProperty.PropertyName)
                UpdatePlaceholder();
            else if (e.PropertyName == Entry.PlaceholderColorProperty.PropertyName)
                UpdatePlaceholderColor();
            else if (e.PropertyName == Entry.CharacterSpacingProperty.PropertyName)
                UpdateCharacterSpacing();
            else if (e.PropertyName == Entry.HorizontalTextAlignmentProperty.PropertyName)
                UpdateHorizontalTextAlignment();
            else if (e.PropertyName == Entry.VerticalTextAlignmentProperty.PropertyName)
                UpdateVerticalTextAlignment();

            base.OnElementPropertyChanged(sender, e);
        }

        public void AfterTextChanged(IEditable s)
        {

        }

        public void BeforeTextChanged(ICharSequence s, int start, int count, int after)
        {

        }

        public void OnTextChanged(ICharSequence s, int start, int before, int count)
        {
            if (before == 0 && count == 0)
                return;

            Element.SendCompleted();
        }

        void UpdateText()
        {
            Control.Text = Element.Text;
        }

        void UpdateTextColor()
        {
            var textColor = Element.TextColor;

            if (textColor == null)
            {
                if (_defaultTextColors != null)
                    Control.SetTextColor(_defaultTextColors);
            }
            else
            {
                var androidColor = textColor.ToAndroid();

                if (!Control.TextColors.IsOneColor(ColorStates, androidColor))
                {
                    var acolor = androidColor.ToArgb();
                    Control.SetTextColor(new ColorStateList(ColorStates, new[] { acolor, acolor }));
                }
            }
        }

        void UpdatePlaceholder()
        {
            if (Control.Hint == Element.Placeholder)
                return;

            Control.Hint = Element.Placeholder;
        }

        void UpdatePlaceholderColor()
        {
            var placeholderTextColor = Element.PlaceholderColor;

            if (placeholderTextColor == null)
            {
                Control.SetHintTextColor(_defaultPlaceholderColors);
            }
            else
            {
                var androidColor = placeholderTextColor.ToAndroid();

                if (!Control.HintTextColors.IsOneColor(ColorExtensions.States, androidColor))
                {
                    var acolor = androidColor.ToArgb();
                    Control.SetHintTextColor(new ColorStateList(ColorExtensions.States, new[] { acolor, acolor }));
                }
            }
        }

        void UpdateCharacterSpacing()
        {
            Control.LetterSpacing = Element.CharacterSpacing.ToEm();
        }

        void UpdateHorizontalTextAlignment()
        {
            Control.UpdateTextAlignment(Element.HorizontalTextAlignment, Element.VerticalTextAlignment);
        }

        void UpdateVerticalTextAlignment()
        {
            Control.UpdateTextAlignment(Element.HorizontalTextAlignment, Element.VerticalTextAlignment);
        }
    }
}