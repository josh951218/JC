using System;
using System.Windows.Forms;

namespace S_61.Basic
{
    public enum ViewMode : int { Normal = 0, Big = 1 }

    public class baseForm : Form
    {
        public bool IsLoad = false;
        public ViewMode ViewStyle { get; set; }
        public String SeekNo { get; set; }
        public baseForm() { }
    }
}
