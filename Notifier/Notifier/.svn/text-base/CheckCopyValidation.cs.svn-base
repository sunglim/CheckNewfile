using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Runtime.InteropServices;

namespace Notifier
{
    public class CheckCopyValidation
    {}

    public class ValidFileStillCopying : IValidCopyBehaviour {
        public void Handle(FileInfo input, IList<string> messages) {
            long sizeFrist = input.Length;
            System.Threading.Thread.Sleep(6000);
            long sizeSecond = new FileInfo(input.FullName).Length;

            if (sizeFrist != sizeSecond) {
                Debug.Write("A SIZEFIRST : " + sizeFrist + "\n" + "B SIZESECOND : " + sizeSecond + "\n");
                messages.Add("FileIsStillbeingCopied");
            }
        }
    }

    public class ValidCurrentlyExist : IValidCopyBehaviour {
        public void Handle(FileInfo input, IList<string> messages) {
            if (!File.Exists(input.FullName)) {
                messages.Add("FileIsNotExist");
            }
        }
    }

    public class ValidNotFileInUse : IValidCopyBehaviour {
        public void Handle(FileInfo input, IList<string> messages) {
            try {
                //Just opening the file as open/create
                FileStream fs = File.Open(input.FullName, FileMode.OpenOrCreate);
                fs.Close();
            } catch (IOException ex) {
                if((Marshal.GetHRForException(ex)  & 0xFFFF )== 32 /*ERROR_SHARING_VIOLATION*/){
                    messages.Add("FileInUseError");
                }else{
                    throw;
                }
            }
        }
    }

    public interface IValidCopyBehaviour {
        void Handle(FileInfo input, IList<string> messages);
    }
}