using System;
using System.Text.RegularExpressions;
using Xamarin.Forms;

namespace Mahzan.Mobile.Behaviors
{
    public class DecimalValidatorBehavior: Behavior<Entry>
    {
        const string decimalRegex = @"^(\d*\.)?\d+$";

        static readonly BindablePropertyKey IsValidPropertyKey = BindableProperty.CreateReadOnly("IsValid", typeof(bool?), typeof(DecimalValidatorBehavior), null);

        public static readonly BindableProperty IsValidProperty = IsValidPropertyKey.BindableProperty;
        public bool? IsValid
        {
            get { return (bool?)base.GetValue(IsValidProperty); }
            private set { base.SetValue(IsValidPropertyKey, value); }
        }

        protected override void OnAttachedTo(Entry bindable)
        {
            bindable.TextChanged += HandleTextChanged;
        }

        void HandleTextChanged(object sender, TextChangedEventArgs e)
        {
            IsValid = (Regex.IsMatch(e.NewTextValue, decimalRegex, RegexOptions.IgnoreCase, TimeSpan.FromMilliseconds(250)));
            ((Entry)sender).TextColor = IsValid.Value ? Color.Default : Color.Red;
        }

        protected override void OnDetachingFrom(Entry bindable)
        {
            bindable.TextChanged -= HandleTextChanged;

        }
    }
}