using Xamarin.Forms;

namespace Library.Triggers
{
    public class NumericValidationTriggerAction : TriggerAction<Entry>
    {
        protected override void Invoke(Entry entry)
        {
            bool isValid = double.TryParse(entry.Text, out double result);
            entry.TextColor = isValid ? Color.Default : Color.Red;
        }
    }
}