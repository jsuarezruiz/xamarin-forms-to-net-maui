using Android.Views;
using Android.Widget;
using ATextAlignment = Android.Views.TextAlignment;

namespace Library.Extensions.Android
{
    public static class TextAlignmentExtensions
	{
		public static void UpdateTextAlignment(this EditText view, Microsoft.Maui.TextAlignment horizontal, Microsoft.Maui.TextAlignment vertical)
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

		internal static ATextAlignment ToTextAlignment(this Microsoft.Maui.TextAlignment alignment)
		{
			switch (alignment)
			{
				case Microsoft.Maui.TextAlignment.Center:
					return ATextAlignment.Center;
				case Microsoft.Maui.TextAlignment.End:
					return ATextAlignment.ViewEnd;
				default:
					return ATextAlignment.ViewStart;
			}
		}

		internal static GravityFlags ToHorizontalGravityFlags(this Microsoft.Maui.TextAlignment alignment)
		{
			switch (alignment)
			{
				case Microsoft.Maui.TextAlignment.Center:
					return GravityFlags.CenterHorizontal;
				case Microsoft.Maui.TextAlignment.End:
					return GravityFlags.End;
				default:
					return GravityFlags.Start;
			}
		}

		internal static GravityFlags ToVerticalGravityFlags(this Microsoft.Maui.TextAlignment alignment)
		{
			switch (alignment)
			{
				case Microsoft.Maui.TextAlignment.Start:
					return GravityFlags.Top;
				case Microsoft.Maui.TextAlignment.End:
					return GravityFlags.Bottom;
				default:
					return GravityFlags.CenterVertical;
			}
		}
	}
}