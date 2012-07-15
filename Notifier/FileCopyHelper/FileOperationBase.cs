using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;

namespace FileCopyHelper {
    public abstract class FileOperationBase
    {
         public delegate void FileOperationProgressEventHandler(object sender, FileOperationProgressEventArgs e);

    /// <summary>
    /// Types d'opération de fichier
    /// </summary>
    public enum FileOperationType
    {
        /// <summary>
        /// Déplacement de fichier
        /// </summary>
        Move,
        /// <summary>
        /// Copie de fichier
        /// </summary>
        Copy
    }

    /// <summary>
    /// Actions possibles à effectuer lorsque l'opération de copie
    /// ou de déplacement de fichier signale sa progression
    /// </summary>
    public enum FileOperationProgressAction
    {
        /// <summary>
        /// Continuer l'opération normalement
        /// </summary>
        Continue = 0,
        /// <summary>
        /// Annuler l'opération. Le fichier de destination est supprimé.
        /// </summary>
        Cancel = 1,
        /// <summary>
        /// Arrête l'opération. Le fichier de destination est conservé.
        /// </summary>
        Stop = 2,
        /// <summary>
        /// Continue l'opération, mais arrête de signaler la progression
        /// </summary>
        Quiet = 3
    }

    /// <summary>
    /// Fournit des données pour l'évènement FileOperationProgressChanged de la
    /// classe FileOperationBase
    /// </summary>
    public class FileOperationProgressEventArgs : EventArgs
    {
        /// <summary>
        /// Crée une nouvelle instance de FileOperationProgressEventArgs
        /// </summary>
        /// <param name="operationType">Type d'opération</param>
        /// <param name="source">Fichier source</param>
        /// <param name="destination">Fichier destination</param>
        /// <param name="transferredBytes">Nombre d'octets déjà transférés</param>
        /// <param name="totalBytes">Nombre total d'octets à transférer</param>
        public FileOperationProgressEventArgs(
            FileOperationType operationType,
            string source,
            string destination,
            long transferredBytes,
            long totalBytes)
        {
            OperationType = operationType;
            Source = source;
            Destination = destination;
            TransferredBytes = transferredBytes;
            TotalBytes = totalBytes;
            Action = FileOperationProgressAction.Continue;
        }

        /// <summary>
        /// Type d'opération
        /// </summary>
        public FileOperationType OperationType { get; private set; }

        /// <summary>
        /// Fichier source
        /// </summary>
        public string Source { get; private set; }

        /// <summary>
        /// Fichier de destination
        /// </summary>
        public string Destination { get; private set; }

        /// <summary>
        /// Nombre d'octets déjà transférés
        /// </summary>
        public long TransferredBytes { get; private set; }

        /// <summary>
        /// Nombre total d'octets à transférer
        /// </summary>
        public long TotalBytes { get; private set; }

        /// <summary>
        /// Renvoie le pourcentage du tranfert déjà effectué
        /// </summary>
        public double PercentDone
        {
            get
            {
                return 100.0 * TransferredBytes / TotalBytes;
            }
        }

        /// <summary>
        /// Renvoie ou définit une valeur indiquand comment l'opération doit se poursuivre.
        /// </summary>
        public FileOperationProgressAction Action { get; set; }
    }
        /// <summary>
        /// Initialise une nouvelle instance de FileOperationBase
        /// </summary>
        /// <param name="source">Fichier source</param>
        /// <param name="destination">Fichier destination</param>
        protected FileOperationBase(string source, string destination)
        {
            Source = source;
            Destination = destination;
        }

        /// <summary>
        /// Quand cette méthode est redéfinie dans une classe dérivée,
        /// exécute l'opération correspondant à l'objet courant
        /// </summary>
        public abstract void Execute();

        /// <summary>
        /// Quand cette propriété est redéfinie dans une classe dérivée,
        /// renvoie le type d'opération correspondant à l'objet courant
        /// </summary>
        public abstract FileOperationType OperationType { get; }

        /// <summary>
        /// Renvoie ou définit une valeur indiquant si le fichier cible
        /// doit être remplacé s'il existe déjà. Si cette propriété est <c>false</c>
        /// et que le fichier cible existe, une exception est levée.
        /// </summary>
        public bool ReplaceExisting { get; set; }

        /// <summary>
        /// Renvoie le nom du fichier source
        /// </summary>
        public string Source { get; private set; }

        /// <summary>
        /// Renvoie le nom du fichier de destination
        /// </summary>
        public string Destination { get; private set; }

        /// <summary>
        /// Se produit quand l'opération signale sa progression
        /// </summary>
        public event FileOperationProgressEventHandler ProgressChanged;

        /// <summary>
        /// Déclenche l'évènement ProgressChanged
        /// </summary>
        /// <param name="e">Un FileOperationProgressEventArgs qui contient les données de l'évènement</param>
        protected void OnProgressChanged(FileOperationProgressEventArgs e)
        {
            var handler = ProgressChanged;
            if (handler != null)
                handler(this, e);
        }

        internal void CheckResult(bool result)
        {
            if (!result)
            {
                int error = Marshal.GetLastWin32Error();
                if (error != ERROR_REQUEST_ABORTED)
                {
                    int hr = Marshal.GetHRForLastWin32Error();
                    throw Marshal.GetExceptionForHR(hr);
                }
            }
        }

        internal int ProgressCallback(
            long totalFileSize,
            long totalBytesTransferred,
            long streamSize,
            long streamBytesTransferred,
            int dwStreamNumber,
            int dwCallbackReason,
            IntPtr hSourceFile,
            IntPtr hDestinationFile,
            IntPtr lpData)
        {
            var args = new FileOperationProgressEventArgs(
                OperationType,
                Source,
                Destination,
                totalBytesTransferred,
                totalFileSize);
            OnProgressChanged(args);
            return (int) args.Action;
        }

        #region Interop declarations

        internal delegate int CopyProgressRoutine(
            long totalFileSize,
            long totalBytesTransferred,
            long streamSize,
            long streamBytesTransferred,
            int dwStreamNumber,
            int dwCallbackReason,
            IntPtr hSourceFile,
            IntPtr hDestinationFile,
            IntPtr lpData);

        internal const int ERROR_REQUEST_ABORTED = 1235;

        #endregion

    }
}
