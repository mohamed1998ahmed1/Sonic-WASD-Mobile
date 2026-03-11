using SonicWASD;
using SonicWASDMobileCLR;
using System;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media;

namespace Sonic_WASD_Mobile
{
    public partial class MainWindow : Window
    {
        private bool _isMonitoring = false;
        private string _activeDeviceId = "";
        private static readonly string[] _mySeparators = new[] { "\r\n", "\r", "\n" };

        public MainWindow()
        {
            InitializeComponent();
            StatusLabel.Text = "⏳ بانتظار اختيار جهاز...";
        }

        private async void Connect_Click(object sender, RoutedEventArgs e)
        {
            if (DeviceList.SelectedItem == null)
            {
                MessageBox.Show("اختار جهاز الأول يا بطل!", "تنبيه", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            string selected = DeviceList.SelectedItem.ToString();
            string id = selected.Split('[', ']')[1];

            StatusLabel.Text = "جاري الاتصال... ⏳";
            StatusLabel.Foreground = Brushes.Yellow;

            bool isReady = await Task.Run(() => EngineController.PrepareController(id));

            if (isReady)
            {
                _activeDeviceId = id;
                StatusLabel.Text = "متصل بنجاح ⚡";
                StatusLabel.Foreground = Brushes.Lime;

                if (!_isMonitoring)
                {
                    _isMonitoring = true;
                    EngineController.StartMonitoring(id);
                    StartUpdateLoop();
                }

                // --- التعديل هنا: فتح الواجهة الجديدة وإغلاق هذه الواجهة ---
                LiveSonicWASD newWindow = new LiveSonicWASD();
                newWindow.Show();
                this.Close();
            }
            else
            {
                StatusLabel.Text = "فشل الاتصال! ❌";
                StatusLabel.Foreground = Brushes.Red;
            }
        }

        private void StartUpdateLoop()
        {
            _ = Task.Run(async () => {
                while (_isMonitoring)
                {
                    string data = EngineController.GetQuickData();
                    Dispatcher.Invoke(() => {
                        if (!string.IsNullOrEmpty(data) && data.Contains('|'))
                        {
                            string[] parts = data.Split('|');
                            BatteryStatus.Text = $"🔋 {parts[0]}%";
                            TempStatus.Text = $"🌡️ {parts[1]}°C";
                        }
                    });
                    await Task.Delay(2000);
                }
            });
        }

        private void Refresh_Click(object sender, RoutedEventArgs e)
        {
            DeviceList.Items.Clear();
            string deviceInfo = EngineController.GetConnectedDevices();
            if (!string.IsNullOrEmpty(deviceInfo))
            {
                foreach (var d in deviceInfo.Split(_mySeparators, StringSplitOptions.RemoveEmptyEntries))
                {
                    if (d.Contains("device") && !d.Contains("List"))
                        DeviceList.Items.Add($"📱 Android Device: [{d.Replace("device", "").Trim()}]");
                }
            }
        }

        private void Settings_Click(object sender, RoutedEventArgs e)
        {
            SettingsWindow settingsWin = new SettingsWindow();
            settingsWin.Owner = this;
            settingsWin.ShowDialog();
        }

        private void ExitApp_Click(object sender, RoutedEventArgs e)
        {
            _isMonitoring = false;
            EngineController.StopMonitoring();
            Application.Current.Shutdown();
        }

        private void Close_Click(object sender, RoutedEventArgs e) => Close();
        private void Minimize_Click(object sender, RoutedEventArgs e) => WindowState = WindowState.Minimized;
        private void Window_MouseDown(object sender, MouseButtonEventArgs e) => DragMove();
        private void Window_KeyDown(object sender, KeyEventArgs e)
        {
            if (string.IsNullOrEmpty(_activeDeviceId)) return;
            string key = e.Key switch { Key.W => "19", Key.S => "20", Key.A => "21", Key.D => "22", Key.Space => "62", _ => "" };
            if (key != "") Task.Run(() => EngineController.SendKeyCommand(_activeDeviceId, key));
        }
    }
}
