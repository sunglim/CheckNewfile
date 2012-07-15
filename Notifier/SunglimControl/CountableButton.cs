using System.Windows.Forms;

namespace SunglimControl {
    public class CountableButton : Button{
        private int buttonCnt;

        public CountableButton(){
            buttonCnt = 0;
        }

        public int PlusCount(int num){
            buttonCnt += num;
            return buttonCnt;
        }

        public void ResetCount(){
            buttonCnt = 0;
        }

        public int ButtonCount{
            get{
                return buttonCnt;
            }
        }

    }
}
