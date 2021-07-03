using Microsoft.Maui.Controls;
using Microsoft.Maui.Graphics;

namespace Behaviors
{
    public class NumericValidationBehavior : Behavior<Entry>
    {
        protected override void OnAttachedTo(Entry entry)
        {
            entry.TextChanged += OnEntryTextChanged;
            base.OnAttachedTo(entry);
        }

        protected override void OnDetachingFrom(Entry entry)
        {
            entry.TextChanged -= OnEntryTextChanged;
            base.OnDetachingFrom(entry);
        }

        void OnEntryTextChanged(object sender, TextChangedEventArgs args)
        {
            bool isValid = double.TryParse(args.NewTextValue, out double result);
            ((Entry)sender).TextColor = isValid ? Colors.Black : Colors.Red;
        }
    }
}