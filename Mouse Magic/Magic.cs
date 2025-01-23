using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.Windows.Forms;
using Gma.System.MouseKeyHook;
using System.Runtime.InteropServices;
using System.IO;
using System.ComponentModel;
using System.Dynamic;
using System.Threading;

namespace Mouse_Magic
{
    public class Magic
    {
        public BackgroundWorker bgWorker = new BackgroundWorker();
        public IKeyboardMouseEvents globalKeyboardMouseHook = Hook.GlobalEvents();
        public MouseControl mouse = new MouseControl();
        public ScreenFunctions screen = new ScreenFunctions();
        public RichTextBox Script;
        public TextBox currentLine;

        public Magic(RichTextBox ScriptIn, TextBox currentLineIn) {
            Script = ScriptIn;
            currentLine = currentLineIn;

            bgWorker.WorkerReportsProgress = true;
            bgWorker.WorkerSupportsCancellation = true;
            bgWorker.DoWork += new DoWorkEventHandler(bgWorker_DoWork);
            bgWorker.ProgressChanged += new ProgressChangedEventHandler(bgWorker_ProgressChanged);
        }

        public void Run(string script, int startLine) {
            if (bgWorker.IsBusy != true)
            {
                dynamic d = new ExpandoObject();
                d.Script = script;
                d.startLine = startLine;
                bgWorker.RunWorkerAsync(d);
                globalKeyboardMouseHook.KeyPress += StopBackgroundworker;

                //Clear Coloring
                currentScriptLength = 0;
                Script.Select(0, Script.Text.Length);
                Script.SelectionColor = Color.Transparent;
            }
        }

        // This event handler is where the time-consuming work is done.
        private void bgWorker_DoWork(object sender, DoWorkEventArgs e)
        {
            BackgroundWorker worker = sender as BackgroundWorker;
            dynamic d = e.Argument as dynamic;
            string[] lines = d.Script.Split(new[] { "\n" }, StringSplitOptions.None);

            for (int i = (int)d.startLine; i < lines.Length; i++)
            {
                if (worker.CancellationPending == true)
                {
                    break;
                }

                dynamic obj = new ExpandoObject();
                obj.line = lines[i];
                worker.ReportProgress(i, obj);

                string[] words = lines[i].Split(' ');
                if (words[0] == "wait" && words.Length == 4)
                {
                    waitfor(words[1], Int32.Parse(words[2]), Int32.Parse(words[3]));
                }
                else if (words[0] == "wait" && words[4] == "=>")
                {
                    if (words[5] == "moveclick" && words.Length == 8)
                    {
                        waitfor(words[1], Int32.Parse(words[2]), Int32.Parse(words[3]),
                            () => { Cursor.Position = new Point(Int32.Parse(words[6]), Int32.Parse(words[7])); mouse.LeftClick(); });
                    }
                }
                else if (words[0] == "move" && words.Length == 3)
                {
                    Cursor.Position = new Point(Int32.Parse(words[1]), Int32.Parse(words[2]));
                }
                else if (words[0] == "click" && words.Length == 1)
                {
                    mouse.LeftClick();
                }
                else if (words[0] == "delay" && words.Length == 2)
                {
                    Thread.Sleep(Int32.Parse(words[1]));
                }
                else if (words[0] == "end" && words.Length == 1)
                {
                    break;
                }
                else if (words[0] == "goto" && words.Length == 2)
                {
                    i = Int32.Parse(words[1]) - 1;
                }
                else if (words[0] == "prnt" && words.Length == 5)
                {
                    screen.TakeSceenshot(Int32.Parse(words[1]), Int32.Parse(words[2]),
                        Int32.Parse(words[3]), Int32.Parse(words[4]));
                }
                else if (words[0] == "write" && words.Length == 2)
                {
                    SendKeys.SendWait(words[1]);
                }
            }

            void waitfor(string fn, int x, int y, Action callback = null)
            {
                Bitmap bmp = (Bitmap)Image.FromFile(AppDomain.CurrentDomain.BaseDirectory + @"\Script\" + fn + ".bmp");
                bool foundBmp = false;
                while (foundBmp == false && worker.CancellationPending == false)
                {
                    Thread.Sleep(100);
                    Point point = new Point(x, y);
                    Cursor.Position = point;
                    Bitmap bmp2 = screen.GetBitmapAt(point.X, point.Y, point.X + 5, point.Y + 5);
                    foundBmp = screen.CompareMemCmp(bmp, bmp2);
                    callback?.Invoke();
                    Cursor.Position = point;
                }
            }
        }

        // This event handler updates the progress.
        private int currentScriptLength = 0;
        private void bgWorker_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            dynamic obj = e.UserState;
            currentLine.Text = e.ProgressPercentage + ": " + (string)obj.line;
            currentScriptLength += obj.line.Length + 1;
            Script.Select(0, currentScriptLength);
            Script.SelectionColor = Color.Green;
        }

        public void StopBackgroundworker(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == '#')
            {
                bgWorker.CancelAsync();
                globalKeyboardMouseHook.KeyPress -= StopBackgroundworker;
            }
        }

        public void WriteMousePosToScript(object sender, MouseEventArgs e)
        {
            string[] lines = Script.Text.Split(
                new[] { "\r\n", "\r", "\n" },
                StringSplitOptions.None
            );
            int cursorPos = Script.SelectionStart;
            int lineNo = Script.GetLineFromCharIndex(cursorPos);
            lines[lineNo] = "move " + string.Format("{0:0000} {1:0000}", e.X, e.Y);
            Script.Text = string.Join("\r\n", lines);
            Script.SelectionStart = cursorPos;
        }

        public Tuple<int, Point, string> setWriteWaitTextToScript()
        {
            string[] lines = Script.Text.Split(
                new[] { "\r\n", "\r", "\n" },
                StringSplitOptions.None
            );
            int cursorPos = Script.SelectionStart;
            int lineNo = Script.GetLineFromCharIndex(cursorPos);
            string fn = lines[lineNo].Split(' ')[1];
            var point = Cursor.Position;
            lines[lineNo] = "wait " + fn + " " + string.Format("{0:0000} {1:0000}", point.X, point.Y);
            Script.Text = string.Join("\r\n", lines);

            return Tuple.Create(cursorPos, point, fn);
        }

        public void Unsubscribe()
        {
            globalKeyboardMouseHook.Dispose();
        }
    }
}
