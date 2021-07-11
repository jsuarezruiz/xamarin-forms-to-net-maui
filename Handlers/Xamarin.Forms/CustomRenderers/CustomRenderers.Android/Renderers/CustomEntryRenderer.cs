using Android.Content;
using Android.Widget;
using CustomRenderers;
using CustomRenderers.Droid.Renderers;
using System.ComponentModel;
using Xamarin.Forms;
using Xamarin.Forms.Platform.Android;

[assembly: ExportRenderer(typeof(CustomEntry), typeof(CustomEntryRenderer))]
namespace CustomRenderers.Droid.Renderers
{
    public class CustomEntryRenderer : ViewRenderer<CustomEntry, EditText>
    {
        public CustomEntryRenderer(Context context) : base(context)
        {

        }

        protected override void OnElementChanged(ElementChangedEventArgs<CustomEntry> e)
        {
            base.OnElementChanged(e);

            if (e.OldElement == null)
            {
                SetNativeControl(new EditText(Context));
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

        void UpdateText()
        {

        }

        void UpdateTextColor()
        {

        }

        void UpdatePlaceholder()
        {

        }

        void UpdatePlaceholderColor()
        {

        }

        void UpdateCharacterSpacing()
        {

        }

        void UpdateHorizontalTextAlignment()
        {

        }

        void UpdateVerticalTextAlignment()
        {

        }
    }
}