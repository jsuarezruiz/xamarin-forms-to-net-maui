using Microsoft.Maui;
using Microsoft.Maui.Graphics;

namespace CustomHandlers.Handlers
{
    public interface ICustomEntry : IView
    {
        public string Text { get; }
        public Color TextColor { get; }
        public string Placeholder { get; }
        public Color PlaceholderColor { get; }
        public double CharacterSpacing { get; }
        public TextAlignment HorizontalTextAlignment { get; }
        public TextAlignment VerticalTextAlignment { get; }

        void SendCompleted();
    }
}
