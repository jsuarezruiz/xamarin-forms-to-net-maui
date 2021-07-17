namespace CustomRenderers.Droid.Extensions
{
    public static class CharacterSpacingExtensions
	{
		public const float EmCoefficient = 0.0624f;

		public static float ToEm(this double pt)
		{
			return (float)pt * EmCoefficient; //Coefficient for converting Pt to Em
		}
	}
}