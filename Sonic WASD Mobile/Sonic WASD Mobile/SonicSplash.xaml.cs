using System;
using System.IO;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media.Animation;

namespace Sonic_WASD_Mobile
{
    public partial class SonicSplash : Window
    {
        public SonicSplash()
        {
            InitializeComponent();
            // ربط التحميل بحدث الظهور
            this.Loaded += (s, e) => StartSmartLoading();
        }

        private async void StartSmartLoading()
        {
            // 1. التحقق من وجود أدوات الاتصال (ADB) قبل أي خطوة
            string adbPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "tools", "adb", "adb.exe");

            bool toolsExist = await Task.Run(() => File.Exists(adbPath));

            if (!toolsExist)
            {
                MessageBox.Show("خطأ: أدوات الاتصال (ADB) غير موجودة في المسار الصحيح!",
                                "فشل البدء", MessageBoxButton.OK, MessageBoxImage.Error);
                Application.Current.Shutdown();
                return;
            }

            // 2. الانتظار (التايمر الخاص بك: 2.5 ثانية للتحميل)
            await Task.Delay(2500);

            // 3. حركة التلاشي الناعم (Fade Out) - 0.5 ثانية
            DoubleAnimation fadeOut = new DoubleAnimation(1, 0, TimeSpan.FromMilliseconds(500));

            // عند انتهاء التلاشي، نفتح الواجهة الرئيسية ونغلق الـ Splash
            fadeOut.Completed += (s, e) => {
                MainWindow main = new MainWindow();
                main.Show();
                this.Close();
            };

            this.BeginAnimation(Window.OpacityProperty, fadeOut);
        }
    }
}