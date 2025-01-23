using System;
using System.Drawing;
using System.Windows.Forms;
using System.IO;

namespace Mouse_Magic
{
    public partial class MouseMagic : Form
    {
        public Magic magic;

        public MouseMagic()
        {
            InitializeComponent();
            Directory.CreateDirectory(AppDomain.CurrentDomain.BaseDirectory + @"\Script"); 
            magic = new Magic(Script, currentLine);
        }
        private void MouseMagic_Load(object sender, EventArgs e)
        {
            Script.Text = File.ReadAllText(AppDomain.CurrentDomain.BaseDirectory + @"\Script\MMScript.txt");
        }

        private void Run_Click(object sender, EventArgs e)
        {
            //Save Script
            File.WriteAllText(AppDomain.CurrentDomain.BaseDirectory + @"\Script\MMScript.txt", Script.Text);
            //Run the Script
            magic.Run(Script.Text, Int32.Parse(startLine.Text));
        }

        public void HighlightLine(RichTextBox richTextBox, int index, Color color)
        {
            richTextBox.SelectAll();
            richTextBox.SelectionBackColor = richTextBox.BackColor;
            var lines = richTextBox.Lines;
            if (index < 0 || index >= lines.Length)
                return;
            var start = richTextBox.GetFirstCharIndexFromLine(index);  // Get the 1st char index of the appended text
            var length = lines[index].Length;
            richTextBox.Select(start, length);                 // Select from there to the end
            richTextBox.SelectionBackColor = color;
        }

        public void WriteBmpToScript(object sender, MouseEventArgs e)
        {
            //Move the selection according to the state of the script
            //Script.SelectionStart = magic.setWriteWaitTextToScript().Item1;
        }

        private bool inserting = false;

        public void StopWriteBmpToScript(object sender, KeyPressEventArgs e)
        {
            magic.globalKeyboardMouseHook.KeyPress -= StopWriteBmpToScript;
            magic.globalKeyboardMouseHook.MouseMove -= WriteBmpToScript;

            var res = magic.setWriteWaitTextToScript();

            Point point = res.Item2;
            var bmp = magic.screen.GetBitmapAt(point.X, point.Y, point.X + 5, point.Y + 5);
            bmp.Save(AppDomain.CurrentDomain.BaseDirectory + @"\Script\" + res.Item3 + ".bmp");
            Script.SelectionStart = res.Item1 + 9;
            inserting = false;

        }

        public void StopWriteMousePosToScript(object sender, KeyPressEventArgs e)
        {
            magic.globalKeyboardMouseHook.KeyPress -= StopWriteMousePosToScript;
            magic.globalKeyboardMouseHook.MouseMove -= magic.WriteMousePosToScript;
            Script.SelectionStart = Script.SelectionStart + 9;
            inserting = false;
        }

        private void Script_TextChanged(object sender, EventArgs e)
        {
            if (inserting == false)
            {
                string line = GetLineAtCursor();
                if (line == "move ")
                {
                    inserting = true;
                    magic.globalKeyboardMouseHook.MouseMove += magic.WriteMousePosToScript;
                    magic.globalKeyboardMouseHook.KeyPress += StopWriteMousePosToScript;

                }
                else if (line.Split(' ')[0] == "wait" && line.Split(' ').Length == 3)
                {
                    inserting = true;
                    magic.globalKeyboardMouseHook.MouseMove += WriteBmpToScript;
                    magic.globalKeyboardMouseHook.KeyPress += StopWriteBmpToScript;
                }
            }
        }
        private string GetLineAtCursor()
        {
            int lineNo = Script.GetLineFromCharIndex(Script.SelectionStart) + 1;
            string[] lines = Script.Text.Replace("\r", "").Split('\n');
            return lines.Length >= lineNo ? lines[lineNo - 1] : null;
        }

        private void MouseMagic_FormClosing(object sender, FormClosingEventArgs e)
        {
            magic.Unsubscribe();
            File.WriteAllText(AppDomain.CurrentDomain.BaseDirectory + @"\Script\MMScript.txt", Script.Text);
        }

        private void currentLine_TextChanged(object sender, EventArgs e)
        {

        }

        private void startLine_TextChanged(object sender, EventArgs e)
        {

        }

        private void Script_Click(object sender, EventArgs e)
        {
            ////Clear Coloring
            //Script.Select(0, Script.Text.Length);
            //Script.SelectionColor = Color.Transparent;
        }
    }

}
