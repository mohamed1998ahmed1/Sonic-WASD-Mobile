#include "pch.h"
#include "Sonic WASD Mobile CLR.h"

using namespace System;
using namespace System::Diagnostics;
using namespace System::IO;
using namespace System::Threading;

namespace SonicWASDMobileCLR {

    // œ«·… „”«⁄œ… ··Õ’Ê· ⁄·Ï „”«— «·‹ ADB
    static String^ GetBaseAdbPath() {
        return Path::Combine(AppDomain::CurrentDomain->BaseDirectory, "tools", "adb", "adb.exe");
    }

    // œ«·…  ‰›Ì– «·√Ê«„— «·√”«”Ì…
    static String^ ExecuteShell(String^ deviceId, String^ command) {
        try {
            Process^ p = gcnew Process();
            p->StartInfo->FileName = GetBaseAdbPath();
            p->StartInfo->Arguments = "-s " + deviceId + " shell " + command;
            p->StartInfo->RedirectStandardOutput = true;
            p->StartInfo->UseShellExecute = false;
            p->StartInfo->CreateNoWindow = true;
            p->Start();
            String^ result = p->StandardOutput->ReadToEnd();
            p->WaitForExit(1000);
            return result;
        }
        catch (...) { return ""; }
    }

    bool EngineController::PrepareController(String^ deviceId) {
        try {
            String^ serverPath = Path::Combine(AppDomain::CurrentDomain->BaseDirectory, "tools", "android-server");
            if (!File::Exists(GetBaseAdbPath()) || !File::Exists(serverPath)) return false;

            Process^ p = gcnew Process();
            p->StartInfo->FileName = GetBaseAdbPath();
            p->StartInfo->CreateNoWindow = true;
            p->StartInfo->UseShellExecute = false;

            // ≈⁄œ«œ «·« ’«·
            p->StartInfo->Arguments = "-s " + deviceId + " shell killall android-server";
            p->Start(); p->WaitForExit(500);
            p->StartInfo->Arguments = "forward --remove-all";
            p->Start(); p->WaitForExit(500);
            p->StartInfo->Arguments = "-s " + deviceId + " push \"" + serverPath + "\" /data/local/tmp/android-server";
            p->Start(); p->WaitForExit(1000);
            p->StartInfo->Arguments = "-s " + deviceId + " shell chmod 777 /data/local/tmp/android-server";
            p->Start(); p->WaitForExit(500);
            p->StartInfo->Arguments = "-s " + deviceId + " forward tcp:1234 localabstract:sonic_wasd";
            p->Start(); p->WaitForExit(500);
            p->StartInfo->Arguments = "-s " + deviceId + " shell /data/local/tmp/android-server &";
            p->Start(); p->WaitForExit(500);

            return true;
        }
        catch (...) { return false; }
    }

    void EngineController::MonitoringLoop(Object^ deviceIdObj) {
        String^ devId = (String^)deviceIdObj;
        while (_keepMonitoring) {
            try {
                // «·”ƒ«· ⁄‰ Õ«·… «·»ÿ«—Ì… ﬂ«„·…
                String^ output = ExecuteShell(devId, "dumpsys battery");

                String^ level = "0";
                String^ temp = "0.0";

                // ›· —… –ﬂÌ…: «·»ÕÀ ⁄‰ «·”ÿ— «·–Ì ÌÕ ÊÌ ⁄·Ï level Ê temperature
                array<String^>^ lines = output->Split(gcnew array<String^>{"\n", "\r"}, StringSplitOptions::RemoveEmptyEntries);

                for each (String ^ line in lines) {
                    line = line->Trim();
                    // ‰»ÕÀ ⁄‰ «·”ÿ— «·–Ì Ì»œ√ »‹ level: (»œÊ‰ „”«›«  ≈÷«›Ì…)
                    if (line->StartsWith("level:")) {
                        level = line->Replace("level:", "")->Trim();
                    }
                    // ‰»ÕÀ ⁄‰ «·”ÿ— «·–Ì ÌÕ ÊÌ ⁄·Ï temperature
                    if (line->Contains("temperature:")) {
                        String^ val = line->Replace("temperature:", "")->Trim();
                        double t = 0;
                        if (Double::TryParse(val, t)) {
                            temp = (t / 10.0).ToString("F1");
                        }
                    }
                }

                _lastData = level + "|" + temp;
            }
            catch (...) { _lastData = "0|0.0"; }
            Thread::Sleep(5000); // “Ì«œ… «·Êﬁ  ﬁ·Ì·« ·—«Õ… «·„Ê»«Ì·
        }
    }

    String^ EngineController::GetConnectedDevices() {
        try {
            Process^ p = gcnew Process();
            p->StartInfo->FileName = GetBaseAdbPath();
            p->StartInfo->Arguments = "devices";
            p->StartInfo->RedirectStandardOutput = true;
            p->StartInfo->UseShellExecute = false;
            p->StartInfo->CreateNoWindow = true;
            p->Start();
            String^ out = p->StandardOutput->ReadToEnd();
            p->WaitForExit(2000);
            return out;
        }
        catch (...) { return ""; }
    }

    void EngineController::SendKeyCommand(String^ deviceId, String^ keyCode) {
        try {
            Process^ p = gcnew Process();
            p->StartInfo->FileName = GetBaseAdbPath();
            p->StartInfo->Arguments = "-s " + deviceId + " shell input keyevent " + keyCode;
            p->StartInfo->CreateNoWindow = true;
            p->StartInfo->UseShellExecute = false;
            p->Start();
        }
        catch (...) {}
    }

    void EngineController::StartMonitoring(String^ deviceId) {
        if (_keepMonitoring) return;
        _keepMonitoring = true;
        _monitorThread = gcnew Thread(gcnew ParameterizedThreadStart(&EngineController::MonitoringLoop));
        _monitorThread->Start(deviceId);
    }

    String^ EngineController::GetQuickData() { return _lastData; }
    void EngineController::StopMonitoring() { _keepMonitoring = false; }
}