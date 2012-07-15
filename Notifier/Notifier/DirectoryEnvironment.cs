
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
namespace Notifier {
    class EnvironmentDirectory {
        public static readonly List<String> ListTransServer = new List<String>() {
                                                                @"\\156.147.69.80\",//PyungTek
                                                                @"\\156.147.69.81\",
                                                                @"\\156.147.69.82\",
                                                                @"\\165.186.175.92\",//Seocho
                                                                @"\\165.186.175.93\",
                                                                @"\\165.186.175.94\"
                                                                };
        private static readonly String L9DirPostfix = @"\pak-dvb_bb_lg1152";
        public static string L9DirectoryPrefix = @"\\156.147.69.80\tftpboot\lg1152\";
        public static string L9DirectoryPostFix = @"\pak-dvb_bb_lg1152";
        public static string MtkDirectoryPrefix = @"\\156.147.69.80\tftpboot\mtk5369\";
        public static string MtkDirectoryPostfix = @"\pak-dvb_bb_mtk5369";
        public static String EpkCopyToFolderName = @"C:\Documents and Settings\heuser\바탕 화면\Work\Epks";
        public static List<String> GetListL9FullDirectory(List<String> transServer, String userName) {
            List<String> ret = new List<String>();
            foreach (String serverName in transServer) {
                // @"\\156.147.69.80\tftpboot\lg1152\sungguk.lim\pak-dvb_bb_lg1152";
                ret.Add(new StringBuilder().AppendFormat(@"{0}tftpboot\lg1152\{1}\pak-dvb_bb_lg1152", serverName, userName).ToString());
            }
            return ret;
        }
        public static List<String> GetListMtkFullDirectory(List<String> transServer, String userName) {
            List<String> ret = new List<String>();
            foreach (String serverName in transServer) {
                // @"\\156.147.69.80\tftpboot\lg1152\sungguk.lim\pak-dvb_bb_lg1152";
                ret.Add(new StringBuilder().AppendFormat(@"{0}tftpboot\mtk5369\{1}\pak-dvb_bb_mtk5369", serverName, userName).ToString());
            }
            return ret;
        }
        public static String GetL9FullDirectory(String username) {
            StringBuilder sb = new StringBuilder();
            sb.AppendFormat("{0}{1}{2}", L9DirectoryPrefix, username, L9DirectoryPostFix);
            return sb.ToString();
        }
        public static String GetMtkFullDirectory(String username) {
            StringBuilder sb = new StringBuilder();
            sb.AppendFormat("{0}{1}{2}", MtkDirectoryPrefix, username, MtkDirectoryPostfix);
            return sb.ToString();
        }
        public static String GetImpersonatedUsername() {
            String fullCurrentName = System.Security.Principal.WindowsIdentity.GetCurrent().Name;
            return fullCurrentName.Replace(@"LGE\", string.Empty);
        }
        public static String GetL9FullDirectory() {
            StringBuilder sb = new StringBuilder();
            sb.AppendFormat("{0}{1}{2}", L9DirectoryPrefix, EnvironmentDirectory.GetImpersonatedUsername(), L9DirectoryPostFix);
            return sb.ToString();
        }
        public static String GetMtkFullDirectory() {
            StringBuilder sb = new StringBuilder();
            sb.AppendFormat("{0}{1}{2}", MtkDirectoryPrefix, EnvironmentDirectory.GetImpersonatedUsername(), MtkDirectoryPostfix);
            return sb.ToString();
        }
    }
}
