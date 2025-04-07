//using Android.Content;
//using Android.Graphics;
//using Android.Widget;
//using AndroidX.AppCompat.App;
//using Microsoft.Maui.Controls.Compatibility;
//using Microsoft.Maui.Controls.Compatibility.Platform.Android;
//using Microsoft.Maui.Controls.Compatibility.Platform.Android.AppCompat;
//using Microsoft.Maui.Controls.Platform;
//using Microsoft.Maui.Platform;
//using PetAdoptApp;
//using PetAdoptApp.Platforms.Android.Renderers;

//[assembly: ExportRenderer(typeof(CustomPicker), typeof(CustomPickerRenderer))]
//namespace PetAdoptApp.Platforms.Android.Renderers
//{
//    public class CustomPickerRenderer : PickerRenderer
//    {
//        public CustomPickerRenderer(Context context) : base(context) { }

//        protected override void OnElementChanged(ElementChangedEventArgs<Picker> e)
//        {
//            base.OnElementChanged(e);

//            if (Control != null && Element is CustomPicker picker)
//            {
//                Control.Click += (sender, args) =>
//                {
//                    var builder = new AlertDialog.Builder(Context);
//                    builder.SetTitle(picker.Title);

//                    var items = picker.Items.ToArray();
//                    builder.SetItems(items, (sender, args) =>
//                    {
//                        picker.SelectedIndex = args.Which;
//                    });

//                    var dialog = builder.Create();

//                    // Применяем цвета
//                    dialog.Window.SetBackgroundDrawable(new Android.Graphics.Drawables.ColorDrawable(
//                        picker.PopupBackgroundColor.ToPlatform()));

//                    // Для изменения цвета текста нужно создать кастомный адаптер
//                    dialog.Show();
//                };
//            }
//        }
//    }
//}