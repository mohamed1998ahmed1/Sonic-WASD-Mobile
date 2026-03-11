using System.Windows;
using System.Windows.Input;

namespace SonicWASD
{
    public partial class SettingsWindow : Window
    {
        public SettingsWindow() { InitializeComponent(); }

        private void CloseSettings_Click(object sender, RoutedEventArgs e) { this.Close(); }
        private void Save_Click(object sender, RoutedEventArgs e) { this.Close(); }

        protected override void OnMouseLeftButtonDown(MouseButtonEventArgs e)
        {
            base.OnMouseLeftButtonDown(e);
            if (e.ButtonState == MouseButtonState.Pressed) this.DragMove();
        }
    }
}
