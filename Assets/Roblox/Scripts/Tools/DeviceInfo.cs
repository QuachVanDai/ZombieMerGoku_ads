using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace VTLTools
{
    public static class DeviceInfo
    {
        public static string DeviceModel { get; private set; }
        public static int Ram { get; private set; }
        static readonly List<string> WeakDevices = new List<string>()        {
            "Redmi 9A", "Redmi 10A", "Galaxy A10", "A17k", "Redmi 9C", "CPH2185",
            "TCL TAB 8 LE", "V2120", "Galaxy A03", "Redmi A1", "vivo 1906", "M2006C3LG",
            "V2026", "SM-A105F", "CPH1931", "220333QAG", "CPH1909", "SM-A226B",
            "M2003J15SC", "moto g(9)", "moto e(7)", "vivo 1904"
        };

        static void GetDeviceInfo()
        {
            DeviceModel = SystemInfo.deviceModel;
            Ram = SystemInfo.systemMemorySize;
            Debug.Log($"Device Model: {DeviceModel}, RAM: {Ram}");

        }

        public static bool IsWeakDevice()
        {
            GetDeviceInfo();
#if UNITY_ANDROID
            return (Ram < 4096 && (ContainsAny(DeviceModel, WeakDevices)) || Ram < 3072);
#else
            return Ram < 2048;
#endif
        }

        static bool ContainsAny(string stringToCompare, IEnumerable<string> listToCompareWith)
        {
            return listToCompareWith.Any(stringToCompare.Contains);
        }
    }
}