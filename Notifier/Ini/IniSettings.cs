using System;
using System.IO;

namespace Ini {
    public class IniSettings {
        private string _epkLocation;
        private string _epkLocation2;
        private string _epkLocation3;
        private string _filecopyLocation;
        private string checkInterval;
        private string fileType;
        private string updateMsg;
        private bool _alwaysOnTop;

        public bool SetEpkLocation(String str) {
            if (!String.IsNullOrEmpty(str)) {
                _epkLocation = str;
                return true;
            }
            return false;
        }

        public bool SetFileCopyLocation(String str) {
            if (!String.IsNullOrEmpty(str)) {
                _filecopyLocation = str;
                return true;
            }
            return false;
        }
        public bool GetAlwaysOnTop() {
            return _alwaysOnTop;
        }
        public String GetUpdateMsg() {
            return updateMsg;
        }

        public String GetEpk2Location() {
            return _epkLocation2;
        }
        public String GetEpk3Location() {
            return _epkLocation3;
        }
        public String GetEpkLocation() {
            return _epkLocation;
        }

        public String GetFileCopyLocation() {
            return _filecopyLocation;
        }

        public String GetCheckInterval() {
            return checkInterval;
        }

        public String GetFileType() {
            return fileType;
        }

        public IniSettings() {
            var ini = new IniFile(Directory.GetCurrentDirectory() + "\\setup.ini");
            _epkLocation = String.IsNullOrEmpty(ini.IniReadValue("FILE", "EPK_LOCATION")) ? @"c:\" : ini.IniReadValue("FILE", "EPK_LOCATION");
            _epkLocation2 = String.IsNullOrEmpty(ini.IniReadValue("FILE", "EPK_LOCATION2")) ? @"c:\" : ini.IniReadValue("FILE", "EPK_LOCATION2");
            _epkLocation3 = String.IsNullOrEmpty(ini.IniReadValue("FILE", "EPK_LOCATION3")) ? @"c:\" : ini.IniReadValue("FILE", "EPK_LOCATION3");
            _filecopyLocation = String.IsNullOrEmpty(ini.IniReadValue("FILE", "FILECOPY_LOCATION")) ? @"C:\temp\" : ini.IniReadValue("FILE", "FILECOPY_LOCATION");
            checkInterval = String.IsNullOrEmpty(ini.IniReadValue("FILE", "INTERVAL")) ? "5" : ini.IniReadValue("FILE", "INTERVAL");
            fileType = String.IsNullOrEmpty(ini.IniReadValue("FILE", "FILE_TYPE")) ? @"*.epk" : ini.IniReadValue("FILE", "FILE_TYPE");
            updateMsg = String.IsNullOrEmpty(ini.IniReadValue("FILE", "UPDATE_MSG")) ? String.Empty : ini.IniReadValue("FILE", "UPDATE_MSG");

            String alwaysontop = ini.IniReadValue("FILE", "ALWAYS_ON_TOP");
            _alwaysOnTop = String.IsNullOrEmpty(alwaysontop) ? false : alwaysontop.ToLower().Equals("yes") || alwaysontop.ToLower().Equals("true");
        }
    }
}