using System.Text.RegularExpressions;

namespace AymedCaseMauiClient.Behaviors
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

        private void OnEntryTextChanged(object sender, TextChangedEventArgs args)
        {
            if (sender is Entry entry)
            {
                // Sadece 0-9 arası rakamları kabul et
                if (!string.IsNullOrEmpty(args.NewTextValue))
                {
                    bool isValid = Regex.IsMatch(args.NewTextValue, "^[0-9]$");

                    if (!isValid)
                    {
                        // Geçersiz karakter girildiğinde eski değere geri dön
                        entry.Text = args.OldTextValue;
                    }
                }
            }
        }
    }
}
