using System;
using System.Drawing;
using System.Windows.Forms;

namespace Utils
{
    public class NRichTextBox : RichTextBox
    {
        private delegate void _AppendTextDelegate(string text);
        private delegate void _AppendTextColouredDelegate(Color col, string text);
        public NRichTextBox()
        {
        }

        private bool _AutoScroll;

        public bool AutoScroll1
        {
            get { return _AutoScroll; }
            set { _AutoScroll = value; }
        }


        public void AddText(string text)
        {
            if (InvokeRequired)
            {
                Invoke(new _AppendTextDelegate(AddText), new object[] { text });
            }
            else
            {
                //AppendText(String.Format("[{0:T}] {1}\n", DateTime.Now, text));
                AppendText(String.Format("{0}", text));
                if (_AutoScroll)
                {
                    ScrollToCaret();
                }
            }
        }

        public void SetText(string text)
        {
            if (InvokeRequired)
            {
                Invoke(new _AppendTextDelegate(SetText), new object[] { text });
            }
            else
            {
                Text = text;
                ScrollToCaret();
            }
        }





        public void AddPlainText(Color col, string text)
        {
            if (InvokeRequired)
            {
                Invoke(new _AppendTextColouredDelegate(AddPlainText), new object[] { col, text });
            }
            else
            {
                /*    if(Lines.Length>=100)
                    {
                        Text = String.Empty;
                    }*/
                try
                {
                    //   AppendText(String.Format("[{0:T}] ", DateTime.Now));
                    //  SelectionColor = col;
                    AppendText(String.Format("{0}", text));
                    ScrollToCaret();
                }
                catch
                {
                    // NotYetDisposedException goes here..
                }
            }
        }


        public void AddText(Color col, string text)
        {
            if (InvokeRequired)
            {
                Invoke(new _AppendTextColouredDelegate(AddText), new object[] { col, text });
            }
            else
            {
                /*    if(Lines.Length>=100)
                    {
                        Text = String.Empty;
                    }*/
                try
                {
//                    AppendText(String.Format("[{0:T}] ", DateTime.Now));
                    SelectionColor = col;
                    AppendText(String.Format("{0}", text));
                    ScrollToCaret();
                }
                catch
                {
                    // NotYetDisposedException goes here..
                }
            }
        }


        public void AddUrl(Color col, string text)
        {
            if (InvokeRequired)
            {
                Invoke(new _AppendTextColouredDelegate(AddUrl), new object[] { col, text });
            }
            else
            {
                /*    if(Lines.Length>=100)
                    {
                        Text = String.Empty;
                    }*/
                try
                {
                    Uri u = new Uri(text);
                    text = u.ToString();
                    if (!text.Contains("<"))
                    {
                        AppendText(String.Format("[{0:T}] ", DateTime.Now));
                        SelectionColor = col;
                        AppendText(String.Format("{0}\n", text));
                        ScrollToCaret();
                    }
                }
                catch
                {
                    // NotYetDisposedException goes here..
                }
            }
        }
    }
}
