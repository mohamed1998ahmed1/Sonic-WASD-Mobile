# 🚀 Sonic WASD Mobile
**The Next-Gen Key-Mapping Engine for High-Performance Mobile Gaming on PC.**

## 📋 Overview
**Sonic WASD Mobile** is a high-speed control engine designed to bridge the gap between mobile touch interfaces and PC peripherals. Built for competitive titles like **COD Mobile** and **PUBG**, it focuses on zero-latency input injection and precision mouse control.

## 🛠️ Project Status & Architecture
The following table provides a detailed breakdown of the system components, their roles, and current development status:

| م | الملف / القطعة | اللغة / النوع | الدور الوظيفي | الحالة | النسبة | ملاحظة فنية |
م,الملف / القطعة,اللغة,الدور الوظيفي,الحالة,النسبة,الملاحظة الفنية

1,App.xaml (.cs),XAML/C#,مدير التشغيل (Startup),✅ جاهز,100%,مضبوط على SonicSplash.
2,SonicSplash (.xaml/.cs),XAML/C#,شاشة التحميل,✅ جاهز,100%,ربط DispatcherTimer احترافي.
3,MainWindow (.xaml/.cs),XAML/C#,واجهة القيادة,✅ جاهز,95%,جاهز لاستقبال بيانات الـ Engine.
4,LiveSonicWASD (.xaml/.cs),XAML/C#,واجهة البث,✅ جاهز,95%,تم التأكد من الـ HwndHost (عرض scrcpy).
5,Sonic WASD CLR.h,C++/CLI,الجسر التقني (Bridge),✅ جاهز,100%,قلب النظام والربط.
6,EngineController.cpp,C++,إدارة الـ ADB والاتصال,✅ جاهز,95%,يحتاج ضبط مسارات ADB.
7,LiveStreamController.cpp,C++,إدارة عرض scrcpy,⚠️ تطوير,70%,التركيز على تضمين النافذة (Embedding).
8,Winsock2 / Ws2_32,Library,نقل بيانات الشبكة,✅ جاهز,100%,استقبال حزم الـ Socket.
9,MonoGame.WpfInterop,NuGet,محرك الرسوم والعرض,✅ جاهز,100%,العرض السريع داخل الـ WPF.

## ✨ Key Features
* **Sonic Input Engine:** Ultra-low latency mapping for instantaneous response.
* **360° Precision FPS:** Advanced mouse-lock system for seamless camera movement.
* **Transparent Overlay:** A lightweight C# / WPF-based UI that stays on top of your game.
* **Universal Compatibility:** Designed to work with **ADB**, **scrcpy**, and other mirroring tools.

## 🛠️ Built With
* **C# / .NET** - Core logic and UI.
* **C++** - Low-level input injection and performance optimization.

## 🚀 Getting Started
*This project is currently in active development.*

1. Clone the repository:
   `git clone https://github.com/mohamed1998ahmed1/Sonic-WASD-Mobile.git`
2. Open the solution in **Visual Studio 2022**.
3. Build and Run.

## 🤝 Contributing
Contributions are welcome! If you have ideas for improving the input latency or adding new game profiles, feel free to open an issue or a PR.

---
*Built with passion by [mohamed1998ahmed1]*
