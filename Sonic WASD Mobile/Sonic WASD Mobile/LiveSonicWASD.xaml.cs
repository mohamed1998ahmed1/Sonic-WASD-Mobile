using SonicWASD;
using SonicWASDMobileCLR;
using System.Windows;
using System.Windows.Input;
using System.Threading.Tasks;

namespace Sonic_WASD_Mobile
{
    public partial class LiveSonicWASD : Window
    {
        private string _activeDeviceId = "";

        public LiveSonicWASD()
        {
            InitializeComponent();
        }

        private void Window_Drag(object sender, MouseButtonEventArgs e)
        {
            if (e.LeftButton == MouseButtonState.Pressed) DragMove();
        }

        // --- أزرار التحكم في النافذة ---
        private void Close_Click(object sender, RoutedEventArgs e)
        {
            // إظهار واجهة الاتصال الرئيسية وإغلاق هذه الواجهة
            MainWindow mainWindow = new MainWindow();
            mainWindow.Show();
            this.Close();
        }

        private void Minimize_Click(object sender, RoutedEventArgs e) => WindowState = WindowState.Minimized;

        private void Maximize_Click(object sender, RoutedEventArgs e)
        {
            WindowState = (WindowState == WindowState.Maximized) ? WindowState.Normal : WindowState.Maximized;
        }

        // --- الأزرار الجديدة ---
        private void ToggleFullScreen_Click(object sender, RoutedEventArgs e)
        {
            WindowState = (WindowState == WindowState.Maximized) ? WindowState.Normal : WindowState.Maximized;
        }

        private void VolumeUp_Click(object sender, RoutedEventArgs e)
        {
            if (!string.IsNullOrEmpty(_activeDeviceId))
                Task.Run(() => EngineController.SendKeyCommand(_activeDeviceId, "24"));
        }

        private void VolumeDown_Click(object sender, RoutedEventArgs e)
        {
            if (!string.IsNullOrEmpty(_activeDeviceId))
                Task.Run(() => EngineController.SendKeyCommand(_activeDeviceId, "25"));
        }

        // --- دوال أزرار التنقل (الرجوع، الهوم، التطبيقات) ---
        private void Back_Click(object sender, RoutedEventArgs e)
        {
            if (!string.IsNullOrEmpty(_activeDeviceId))
                Task.Run(() => EngineController.SendKeyCommand(_activeDeviceId, "3"));
        }

        private void Home_Click(object sender, RoutedEventArgs e)
        {
            if (!string.IsNullOrEmpty(_activeDeviceId))
                Task.Run(() => EngineController.SendKeyCommand(_activeDeviceId, "4"));
        }

        private void Recent_Click(object sender, RoutedEventArgs e)
        {
            if (!string.IsNullOrEmpty(_activeDeviceId))
                Task.Run(() => EngineController.SendKeyCommand(_activeDeviceId, "187"));
        }

        // --- أزرار الوظائف ---
        private void Connect_Click(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("جاري الاتصال...");
        }

        private void Mapping_Click(object sender, RoutedEventArgs e) { }

        private void Settings_Click(object sender, RoutedEventArgs e)
        {
            SettingsWindow settings = new SettingsWindow();
            settings.Owner = this;
            settings.ShowDialog();
        }
    }
}
