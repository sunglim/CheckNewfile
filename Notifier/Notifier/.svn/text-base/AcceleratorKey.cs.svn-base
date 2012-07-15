using System;
using System.Windows.Forms;

namespace Notifier {
    public class AcceleratorKey {
        private Keys key_ = Keys.None;
        public AcceleratorKey() {
        }

        public AcceleratorKey(Keys key) {
            key_ = key;
        }

        public Keys Key {
            get {
                return key_;
            }
            set {
                key_ = value;
            }
        }

        public override Int32 GetHashCode() {
            return (Int32)key_;
        }

        public override bool Equals(Object obj) {
            if (obj.GetHashCode() == (Int32)key_)
                return true;

            return false;
        }
    }

}