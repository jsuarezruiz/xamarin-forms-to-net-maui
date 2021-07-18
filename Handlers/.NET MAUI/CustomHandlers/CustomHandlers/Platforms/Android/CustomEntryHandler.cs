#nullable enable
using Android.Content.Res;
using Android.Text;
using Android.Widget;
using CustomHandlers.Platforms.Android;
using Microsoft.Maui;
using Microsoft.Maui.Handlers;

namespace CustomHandlers.Handlers
{
    public partial class CustomEntryHandler : ViewHandler<ICustomEntry, EditText>
    {
        ColorStateList? _defaultTextColors;
        ColorStateList? _defaultPlaceholderColors;

        readonly TextWatcher _watcher = new();

        protected override EditText CreateNativeView()
        {
            return new EditText(Context);
        }

        protected override void ConnectHandler(EditText nativeView)
        {
            _defaultTextColors = nativeView.TextColors;
            _defaultPlaceholderColors = nativeView.HintTextColors;

            _watcher.Handler = this;
            nativeView.AddTextChangedListener(_watcher);

            base.ConnectHandler(nativeView);
        }

        protected override void DisconnectHandler(EditText nativeView)
        {
            nativeView.RemoveTextChangedListener(_watcher);
            _watcher.Handler = null;

            base.DisconnectHandler(nativeView);
        }

        public static void MapText(CustomEntryHandler handler, ICustomEntry entry)
        {
            handler.NativeView?.UpdateText(entry);
        }

        public static void MapTextColor(CustomEntryHandler handler, ICustomEntry entry)
        {
            handler.NativeView?.UpdateTextColor(entry, handler._defaultTextColors);
        }

        public static void MapPlaceholder(CustomEntryHandler handler, ICustomEntry entry)
        {
            handler.NativeView?.UpdatePlaceholder(entry);
        }

        public static void MapPlaceholderColor(CustomEntryHandler handler, ICustomEntry entry)
        {
            handler.NativeView?.UpdatePlaceholderColor(entry, handler._defaultPlaceholderColors);
        }

        public static void MapCharacterSpacing(CustomEntryHandler handler, ICustomEntry entry)
        {
            handler.NativeView?.UpdateCharacterSpacing(entry);
        }

        public static void MapHorizontalLayoutAlignment(CustomEntryHandler handler, ICustomEntry entry)
        {
            handler.NativeView?.UpdateHorizontalLayoutAlignment(entry);
        }

        public static void MapVerticalLayoutAlignment(CustomEntryHandler handler, ICustomEntry entry)
        {
            handler.NativeView?.UpdateVerticaLayoutAlignment(entry);
        }

        class TextWatcher : Java.Lang.Object, ITextWatcher
        {
            public CustomEntryHandler? Handler { get; set; }

            void ITextWatcher.AfterTextChanged(IEditable? s)
            {
            }

            void ITextWatcher.BeforeTextChanged(Java.Lang.ICharSequence? s, int start, int count, int after)
            {
            }

            void ITextWatcher.OnTextChanged(Java.Lang.ICharSequence? s, int start, int before, int count)
            {
                // We are replacing 0 characters with 0 characters, so skip
                if (before == 0 && count == 0)
                    return;

                Handler?.VirtualView?.SendCompleted();
            }
        }
    }
}