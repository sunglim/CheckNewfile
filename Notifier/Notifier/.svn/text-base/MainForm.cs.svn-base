using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Text;
using System.Threading;
using System.Windows.Forms;
using System.IO;
using FileCopyHelper;
using Ini;

using SunglimControl;

namespace Notifier {
    public partial class MainForm : Form {

        #region Variables

        private readonly IniSettings _iniSettings;

        private EpkList _epkList;
        private EpkCopyFileQueue _epkFileQueue;
        private String[] epkFolderUri = new String[3];
        private PeriodicCopyJob[] _periodicCheck = new PeriodicCopyJob[3];
        private Thread[] checkThread = new Thread[3];
        #endregion
        
        private string copyFileque;

        #region Methods

        private void callbackFromPeriodicCheck(String msg,int index) {
            writeToDebugPanel(msg);
            if (msg.StartsWith("!!")) { // find 

               copyFileque = msg.Substring(2);
               // _epkFileQueue.Enqueue(index);//enqueue file name

                mCopier.RunWorkerAsync();
            }
        }

        private void writeToDebugPanel(String msg) {
            txtResultPrint.Invoke((Action)delegate {
                txtResultPrint.AppendText(DateTime.Now.ToString("H:mm:ss") + " " + msg + Environment.NewLine);
                txtResultPrint.ScrollToCaret();
                if (txtBoxResultFile.TextLength > 100000) {
                    txtBoxResultFile.Clear();
                }
            });
        }

        #endregion

        private void UpdateSettingsUi() {
            txtEpkDir.Text = _iniSettings.GetEpkLocation();
            txtEpkDir2.Text = _iniSettings.GetEpk2Location();
            txtEpkDir3.Text = _iniSettings.GetEpk3Location();
            txtEpkCopyToDir.Text = _iniSettings.GetFileCopyLocation();
            txtInterval.Text = _iniSettings.GetCheckInterval();
            this.TopMost = _iniSettings.GetAlwaysOnTop();
        }

        public MainForm() {
            InitializeComponent();

            InitializeBackgroundWorker();

            this._iniSettings = new IniSettings();
            _epkFileQueue = new EpkCopyFileQueue();

            UpdateSettingsUi();

            LoadAcceleratorKeyMapping();

        }

        private void InitializeBackgroundWorker() {
            mCopier.DoWork += startCopyJob;
            mCopier.RunWorkerCompleted += new RunWorkerCompletedEventHandler(restartTimer);
        }

        private void restartTimer(object sender, RunWorkerCompletedEventArgs e) {
        }

        private void callbackReturn(int value) {
            Debug.Write("Return Vaue : " + value + "\n");
            writeToDebugPanel("Start Copy : " + value);
        }

        private void ChangeUI(bool doStart) {
            checkNowButton.Enabled = true;
            mainStartButton.Text = doStart ? "Cancel" : "시작";
            progressCopy.Value = 0;
            mainStartButton.Text = "progressing";
            mainStartButton.Enabled = false;

            writeToDebugPanel(" start & save initial Directory info");
        }


        private void startCopyJob(object sender, DoWorkEventArgs e) {
            string newlyAddedFile = copyFileque.Substring(copyFileque.LastIndexOf('\\') + 1);
            CopyFile(copyFileque, txtEpkCopyToDir.Text + "\\" + newlyAddedFile); 

            StringBuilder sb = new StringBuilder();
            sb.AppendFormat("swu {0}{1}", _iniSettings.GetUpdateMsg(), newlyAddedFile);

            txtBoxResultFile.Invoke((Action)delegate {
                txtBoxResultFile.Text = sb.ToString();
            });

            String processNameWithOption = (txtEpkCopyToDir.Text.EndsWith("\\")) ? "/select, " + txtEpkCopyToDir.Text + newlyAddedFile
                                                                                 : "/select, " + txtEpkCopyToDir.Text + @"\" + newlyAddedFile;

            Process.Start("explorer.exe", processNameWithOption);

            copySwuMsgButton.ResetCount();

            progressCopy.Invoke((Action)delegate {
                progressCopy.Value = 0;
            });
            copySwuMsgButton.Invoke((Action)delegate {
                copySwuMsgButton.Text = "Ready.." + Environment.NewLine + "CTRL + C";
                copySwuMsgButton.BackColor = System.Drawing.SystemColors.Highlight;
                copySwuMsgButton.Enabled = true;
            });
        }

        private void CopyFile(string source, string destination) {
            FileCopyOperation copy = new FileCopyOperation(source, destination);
            copy.ReplaceExisting = true;
            copy.ProgressChanged += copy_ProgressChanged;

            copy.Execute();
        }

        private void copy_ProgressChanged(object sender, FileOperationBase.FileOperationProgressEventArgs e) {
            if (progressCopy.Value != (int)e.PercentDone) {
                progressCopy.Invoke((Action)delegate {
                    progressCopy.Value = (int)e.PercentDone;
                });

                labelCopyPercent.Invoke((Action)delegate {
                    labelCopyPercent.Text = progressCopy.Value + " %";
                });
            }
        }
 
        #region KeyAccelerator

        private enum Accelerators {
            Unspecified = 0,
            CopySwu,
            Logout
        };
        private Hashtable _accelHash = new Hashtable();
        private void LoadAcceleratorKeyMapping() {
            _accelHash.Add(new AcceleratorKey(Keys.Control | Keys.C), Accelerators.CopySwu);
        }

        protected override bool ProcessCmdKey(ref Message msg, Keys keyData) {
            bool bHandled = false;

            Accelerators accel = Accelerators.Unspecified;
            if (_accelHash.ContainsKey(new AcceleratorKey(keyData))) {
                accel = (Accelerators)_accelHash[keyData];

                switch (accel) {
                    case Accelerators.CopySwu:
                        if (copySwuMsgButton.Enabled) {
                            copySwuAction();
                        }
                        bHandled = true;
                        break;

                    default:
                        break;

                } // switch
            } // if
            return bHandled;
        }

        #endregion

        #region EventFunction

        private void mainStartButton_Click(object sender, EventArgs e) {
                epkFolderUri[0] = txtEpkDir.Text.Trim();
                epkFolderUri[1] = txtEpkDir2.Text.Trim();
                epkFolderUri[2] = txtEpkDir3.Text.Trim();

            int i=0;
            foreach (var epkFoler in epkFolderUri) {
                if(!String.IsNullOrEmpty(epkFoler)){
                    _periodicCheck[i] = new PeriodicCopyJob(epkFoler, Int32.Parse(txtInterval.Text.Trim()), new PeriodicCopyJob.PeriodicCheckEventHandler(callbackFromPeriodicCheck),i);
                    checkThread[i] = new Thread(new ThreadStart(_periodicCheck[i].startMain));
                    checkThread[i].Start();
                }
            }

            writeToDebugPanel("startMain()");
            ChangeUI(this.mainStartButton.Text.Equals("시작"));
        }

        private void txtInterval_KeyPress(object sender, KeyPressEventArgs e) {
            // allow only number key
            if (!char.IsControl(e.KeyChar)
                    && !char.IsDigit(e.KeyChar)) {
                e.Handled = true;
            }
        }

        private void txtInterval_Leave(object sender, EventArgs e) {
            writeToDebugPanel("Changed Interval to " +  (1000) * Int32.Parse(txtInterval.Text.Trim()));
        }

        private void MainForm_Resize(object sender, EventArgs e) {
            txtResultPrint.Height = this.Height - 290/*origin textbox size*/;
        }

        #region ButtonEvent

        private void copySwuAction() {
            copySwuMsgButton.PlusCount(1);
            copySwuMsgButton.Text = String.Format(" ( {0} ) ", copySwuMsgButton.ButtonCount);
            copySwuMsgButton.BackColor = System.Drawing.SystemColors.Control;

            Clipboard.SetText(txtBoxResultFile.Text.Trim());
            MessageBox.Show("Message Copied to clipboard" + Environment.NewLine + "\"" + txtBoxResultFile.Text.Trim() + "\"");
            writeToDebugPanel("Message Copied to clipboard" + Environment.NewLine + "\"" + txtBoxResultFile.Text.Trim() + "\"");
        }

        private void checkNowButton_Click(object sender, EventArgs e) {
            writeToDebugPanel("CheckNow!");
        }

        private void copySwuMsgButton_Click(object sender, EventArgs e) {
            copySwuAction();
        }

        private void openEpkFolderButton_Click(object sender, EventArgs e) {
            Process.Start("explorer.exe", txtEpkDir.Text.Trim());
        }

        private void openCopytoFolderButton_Click(object sender, EventArgs e) {
            Process.Start("explorer.exe", txtEpkCopyToDir.Text.Trim());
        }

        private void openEpkDlgButton_Click(object sender, EventArgs e) {
            openFolderDialog(sender);
        }

        private void openFolderDialog(object sender) {
            // TODO : need a EXtract Method;
            TextBox passedTextBox = (TextBox)this.Controls.Find(this.txtEpkDir.Name + ((Button)sender).Name.Replace("openEpkDlgButton", string.Empty), true)[0];

            var dlg1 = new Ionic.Utils.FolderBrowserDialogEx();
            dlg1.Description = "Select a folder which will be checked :";
            dlg1.ShowNewFolderButton = true;
            dlg1.ShowEditBox = true;
            //dlg1.NewStyle = false;
            dlg1.SelectedPath = txtEpkDir.Text;
            dlg1.ShowFullPathInEditBox = true;
            dlg1.RootFolder = System.Environment.SpecialFolder.MyComputer;

            // Show the FolderBrowserDialog.
            DialogResult result = dlg1.ShowDialog();
            if (result == DialogResult.OK) {
                passedTextBox.Text = dlg1.SelectedPath;
                writeToDebugPanel(DateTime.Now.ToString("H:mm:ss") + " file directory changed to " +
                                                 txtEpkDir.Text);
            }
        }

        private void openEpkDlgButton2_Click(object sender, EventArgs e) {
            openFolderDialog(sender);
        }

        private void openEpkDlgButton3_Click(object sender, EventArgs e) {
            openFolderDialog(sender);
        }

        private void openCopytoDlgButton_Click(object sender, EventArgs e) {
            var dlg1 = new Ionic.Utils.FolderBrowserDialogEx
                           {
                               Description = "Select a folder the file be located:",
                               ShowNewFolderButton = true,
                               ShowEditBox = true,
                               SelectedPath = txtEpkDir.Text,
                               ShowFullPathInEditBox = true,
                               RootFolder = System.Environment.SpecialFolder.MyComputer
                           };
            //dlg1.NewStyle = false;

            // Show the FolderBrowserDialog.
            DialogResult result = dlg1.ShowDialog();
            if (result == DialogResult.OK) {
                txtEpkCopyToDir.Text = dlg1.SelectedPath;
                writeToDebugPanel(DateTime.Now.ToString("H:mm:ss") + " destination directory changed to " +
                                          txtEpkDir.Text);
            }
        }

        private void openEpkFolderButton2_Click(object sender, EventArgs e) {
            Process.Start("explorer.exe", txtEpkDir2.Text.Trim());
        }

        private void openEpkFolderButton3_Click(object sender, EventArgs e) {
            Process.Start("explorer.exe", txtEpkDir3.Text.Trim());
        }

        #endregion



        #endregion



    }
}
