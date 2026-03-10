#pragma once

using namespace System;

namespace SonicWASDMobileCLR {
    public ref class EngineController {
    private:
        static String^ _adbPath;
        static String^ _serverPath;
        static String^ _lastData = "0|0.0";
        static bool _keepMonitoring = false;
        static System::Threading::Thread^ _monitorThread = nullptr;

        static void MonitoringLoop(Object^ deviceIdObj);

    public:
        // œÊ«· «·« ’«· «·√”«”Ì… ›ﬁÿ
        static bool PrepareController(String^ deviceId);
        static String^ GetConnectedDevices();
        static void StartMonitoring(String^ deviceId);
        static void StopMonitoring();
        static String^ GetQuickData();
        static void SendKeyCommand(String^ deviceId, String^ keyCode);
    };
}