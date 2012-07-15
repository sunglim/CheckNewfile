using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Diagnostics;

namespace Notifier {
    public class EpkList {
        public delegate void FileOperationProgressEventHandler(int value);

        public enum EpkStatus {
            Idle = 0,
            /// <summary>
            /// Annuler l'opération. Le fichier de destination est supprimé.
            /// </summary>
            Check = 1,
            /// <summary>
            /// Arrête l'opération. Le fichier de destination est conservé.
            /// </summary>
            Copy = 2,
            /// <summary>
            /// Continue l'opération, mais arrête de signaler la progression
            /// </summary>
            Done = 3
        }

        private FileInfo[] _ancentFileList;
        private FileInfo[] _oldFileList;
        private FileInfo[] _newlyAddedFileList;

        private FileInfo _lastAddedFileInfo;

        public void SetFileLocation(String str) {
        }

        public EpkList(FileInfo[] oldFileList, FileInfo[] newFileList) {
            this._oldFileList = oldFileList;
            this._newlyAddedFileList = newFileList;
        }

        public bool IsNewFileAdded(FileOperationProgressEventHandler _call) {
            IList<string> messages = new List<string>();
            var behaviours = new List<IValidCopyBehaviour>{
                new ValidCurrentlyExist(),
                new ValidFileStillCopying()
                //new ValidNotFileInUse()
            };

            if (_oldFileList != null && _newlyAddedFileList != null) {
                Boolean isHasOldExceptNewList = (from file in _newlyAddedFileList
                                                 select file).Except(_oldFileList, new EpkEqualityComparer()).Any();
                FileInfo tmpinfo;
                if (isHasOldExceptNewList) {
                    foreach (var a in _oldFileList) {
                        Debug.Write("OLD : " + a.Name + "\n");
                    }
                    foreach (var a in _newlyAddedFileList) {
                        Debug.Write("NEW : " + a.Name + "\n");
                    }

                    tmpinfo = _newlyAddedFileList.Except(_oldFileList, new EpkEqualityComparer()).First();

                    foreach(var behaviour in behaviours){
                        behaviour.Handle(tmpinfo,messages);
                    }

                    if (messages.Count > 0) {
                        Debug.WriteLine(messages);
                        RollbackFileLists();
                        return false;
                    }

                    //Debug.Write("A SIZEFIRST : " + sizeFrist + "\n" + "B SIZESECOND : " + sizeSecond + "\n");
                } else {
                    Debug.Write("No new file\n");
                }

                    _lastAddedFileInfo = _newlyAddedFileList.Except(_oldFileList, new EpkEqualityComparer()).ToList().First();
            }
            return true;
        }

        public String GetNewlyAddedFile() {
            return (_lastAddedFileInfo != null) ? _lastAddedFileInfo.Name : String.Empty;
        }
        public void SetOldFileList(FileInfo[] oldFileList) {
            this._oldFileList = oldFileList;
        }
        public void SetNewFileList(FileInfo[] newFileList) {
            this._newlyAddedFileList = newFileList;
        }
        public void PushNewFileList(FileInfo[] newFileList) {
            this._ancentFileList = this._oldFileList;
            this._oldFileList = this._newlyAddedFileList;
            this._newlyAddedFileList = newFileList;
        }
        private void RollbackFileLists() {
            this._newlyAddedFileList = this._oldFileList;
            this._oldFileList = this._ancentFileList;
        }

        #region EpkEqualityComparer
        private class EpkEqualityComparer : IEqualityComparer<FileInfo> {
            public bool Equals(FileInfo fi1, FileInfo fi2) {
                if (fi1.CreationTime != fi2.CreationTime) {
                    Debug.Write("1?? : " + fi1.Name + "\n" + fi2.Name + "\n");
                }
                if ((fi1.Name != fi2.Name)) {
                    Debug.Write("2?? : " + fi1.Name + "\n" + fi2.Name + "\n");
                }
                if (fi1.Length != fi2.Length) {
                    Debug.Write("3?? : " + fi1.Name + "\n" + fi2.Name + "\n");
                }
                return (fi1.Name == fi2.Name) && (fi1.Length == fi2.Length) && (fi1.CreationTime == fi2.CreationTime);
            }
            public int GetHashCode(FileInfo fi) {
                string s = String.Format("{0}{1}{2}", fi.Name, fi.Length, fi.CreationTime);
                return s.GetHashCode();
            }
        }
        #endregion
    }
}
