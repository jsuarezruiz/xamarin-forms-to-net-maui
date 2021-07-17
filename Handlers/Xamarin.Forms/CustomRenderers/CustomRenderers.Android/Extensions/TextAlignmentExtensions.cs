using Android.Views;
using Android.Widget;
using ATextAlignment = Android.Views.TextAlignment;

namespace CustomRenderers.Droid.Extensions
{
    public static class TextAlignmentExtensions
	{
		public static void UpdateTextAlignment(this EditText view, Xamarin.Forms.TextAlignment horizontal, Xamarin.Forms.TextAlignment vertical)
		{
			if (view.Context != null)
			{
				view.Gravity = vertical.ToVerticalGravityFlags() | horizontal.ToHorizontalGravityFlags();
			}
			else
			{
				view.TextAlignment = horizontal.ToTextAlignment();
				view.Gravity = vertical.ToVerticalGravityFlags();
			}
		}

		internal static ATextAlignment ToTextAlignment(this Xamarin.Forms.TextAlignment alignment)
		{
			switch (alignment)
			{
				case Xamarin.Forms.TextAlignment.Center:
					return ATextAlignment.Center;
				case Xamarin.Forms.TextAlignment.End:
					return ATextAlignment.ViewEnd;
				default:
					return ATextAlignment.ViewStart;
			}
		}

		internal static GravityFlags ToHorizontalGravityFlags(this Xamarin.Forms.TextAlignment alignment)
		{
			switch (alignment)
			{
				case Xamarin.Forms.TextAlignment.Center:
					return GravityFlags.CenterHorizontal;
				case Xamarin.Forms.TextAlignment.End:
					return GravityFlags.End;
				default:
					return GravityFlags.Start;
			}
		}

		internal static GravityFlags ToVerticalGravityFlags(this Xamarin.Forms.TextAlignment alignment)
		{
			switch (alignment)
			{
				case Xamarin.Forms.TextAlignment.Start:
					return GravityFlags.Top;
				case  Xamarin.Forms.TextAlignment.End:
					return GravityFlags.Bottom;
				default:
					return GravityFlags.CenterVertical;
			}
		}
	}
}