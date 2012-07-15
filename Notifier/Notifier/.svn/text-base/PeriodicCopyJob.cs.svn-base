using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Timers;
using FileCopyHelper;

namespace Notifier {
    class PeriodicCopyJob {
        private int index;
        private readonly string _checkDirectoryPath;
        private Timer _mainCheckTimer;
        private readonly int _timerInterval;
        private EpkList _checkEpkList;
        private readonly string _searchPattern;
        private PeriodicCheckEventHandler callBackHandler;

        public delegate void PeriodicCheckEventHandler(string msg,int index);

        public PeriodicCopyJob(String directoryPath, int checkInterval, PeriodicCheckEventHandler callback,int index) {
            this._checkDirectoryPath = directoryPath;
            this._timerInterval = checkInterval;
            this._searchPattern = "*.epk";
            this.callBackHandler = callback;
            this.index = index;
        }

        public PeriodicCopyJob(String directoryPath, int checkInterval, string searchPattern, PeriodicCheckEventHandler callback,int index) {
            this._checkDirectoryPath = directoryPath;
            this._timerInterval = checkInterval;
            this._searchPattern = searchPattern;
            this.callBackHandler = callback;
            this.index = index;
        }

        public void callbackMsg(string str) {
            callBackHandler(str,this.index);
        }

        public void startMain() {
            callbackMsg("startMain debug : " + _checkDirectoryPath);
            initializeTimer();
            InitCheck();
            _mainCheckTimer.Enabled = true;
        }

        private void initializeTimer() {
            _mainCheckTimer = new System.Timers.Timer();
            _mainCheckTimer.Elapsed += new ElapsedEventHandler(checkFileExist);
            _mainCheckTimer.Interval = (1000) * _timerInterval;
        }

        private void checkFileExist(Object myObject, EventArgs myEventArgs) {
            callbackMsg("checkFileExist : " + _checkDirectoryPath);

            try {
                DirectoryInfo di = new DirectoryInfo(_checkDirectoryPath);
                FileInfo[] newFileInfos = di.GetFiles(_searchPattern);
                _checkEpkList.PushNewFileList(newFileInfos);
                if (_checkEpkList.IsNewFileAdded(callbackReturn)) {
                    newEpkFoundJob();
                }
            } catch (Exception ea) {
                // writeToDebugPanel("ERROR : timer1_Tick() " + ea.ToString());
            }
        }

        private void newEpkFoundJob() {
            callbackMsg("!!" + _checkDirectoryPath + "\\" + _checkEpkList.GetNewlyAddedFile());
            callbackMsg(" NEW FILE FOUND  ");
            //_mainCheckTimer.Stop();
        }

        private void InitCheck() {
            callbackMsg("initCheck : " + _checkDirectoryPath);
            try {
                if (String.IsNullOrEmpty(_checkDirectoryPath)) {
                    return;
                } else {
                    DirectoryInfo di = new DirectoryInfo(_checkDirectoryPath);
                    FileInfo[] oldFiles = di.GetFiles(_searchPattern);

                    _checkEpkList = new EpkList(oldFiles, oldFiles);
                }
            } catch (Exception ea) {
            }
        }
        private void callbackReturn(int value) {
            Debug.Write("Return Vaue : " + value + "\n");
            callbackMsg("Return Vaue : " + value + "\n");
        }

        public void RestartTimer() {
            _mainCheckTimer.Start();
        }

    }
}
