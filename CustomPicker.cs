namespace PetAdoptApp
{
    public class CustomPicker : Picker
    {
        public static readonly BindableProperty PopupBackgroundColorProperty =
            BindableProperty.Create(
                nameof(PopupBackgroundColor),
                typeof(Color),
                typeof(CustomPicker),
                Colors.White); // Белый фон по умолчанию

        public static readonly BindableProperty PopupTextColorProperty =
            BindableProperty.Create(
                nameof(PopupTextColor),
                typeof(Color),
                typeof(CustomPicker),
                Colors.Black); // Черный текст по умолчанию

        public Color PopupBackgroundColor
        {
            get => (Color)GetValue(PopupBackgroundColorProperty);
            set => SetValue(PopupBackgroundColorProperty, value);
        }
        public Color PopupTextColor
        {
            get => (Color)GetValue(PopupTextColorProperty);
            set => SetValue(PopupTextColorProperty, value);
        }
    }
}
