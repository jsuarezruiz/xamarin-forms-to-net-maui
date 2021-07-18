using Android.Content.Res;
using AColor = Android.Graphics.Color;

namespace CustomHandlers.Platforms.Android
{
    internal static class ColorStateListExtensions
    {
        public static bool IsOneColor(
            this ColorStateList? csl,
            int[][] ColorStates,
            AColor color)
        {
            if (csl == null)
                return false;

            if (ColorStates.Length == 0)
                return false;

            for (int i = 0; i < ColorStates.Length; i++)
            {
                var colorState = ColorStates[i];
                if (csl.GetColorForState(colorState, color) != color)
                    return false;
            }

            return true;
        }
    }
}