using System;
using System.Runtime.InteropServices;


namespace FileCopyHelper {
 public class FileCopyOperation : FileOperationBase
    {

        /// <summary>
        /// Initialise une nouvelle instance de FileCopyOperation
        /// </summary>
        /// <param name="source">Fichier source</param>
        /// <param name="destination">Fichier destination</param>
        public FileCopyOperation(string source, string destination)
            : base(source, destination)
        {
        }

        /// <summary>
        /// Exécute la copie
        /// </summary>
        public override void Execute()
        {
            bool cancel = false;
            int flags = 0;
            if (!ReplaceExisting)
                flags |= COPY_FILE_FAIL_IF_EXISTS;

            bool r = CopyFileEx(
                Source,
                Destination,
                ProgressCallback,
                IntPtr.Zero,
                ref cancel,
                flags);
            
            CheckResult(r);
        }

        /// <summary>
        /// Renvoie FileOperationType.Copy
        /// </summary>
        public override FileOperationType OperationType
        {
            get { return FileOperationType.Copy; }
        }

        #region Interop declarations

        [DllImport("kernel32", SetLastError = true)]
        private static extern bool CopyFileEx(
            string lpExistingFileName,
            string lpNewFileName,
            CopyProgressRoutine lpProgressRoutine,
            IntPtr lpData,
            ref bool pbCancel,
            int dwCopyFlags);

        private const int COPY_FILE_FAIL_IF_EXISTS = 0x0001;
        private const int COPY_FILE_RESTARTABLE = 0x0002;
        private const int COPY_FILE_OPEN_SOURCE_FOR_WRITE = 0x0004;
        private const int COPY_FILE_ALLOW_DECRYPTED_DESTINATION = 0x0008;
        private const int COPY_FILE_COPY_SYMLINK = 0x0800;
        private const int COPY_FILE_NO_BUFFERING = 0x1000;

        #endregion
    }
}
