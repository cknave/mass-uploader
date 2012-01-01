using System;
using System.Text;
using System.IO;
using System.Windows.Forms;
using System.Drawing;

namespace Utils
{
    public class TextBoxStreamWriter : TextWriter
    {
        NRichTextBox _output = null;

        public TextBoxStreamWriter(NRichTextBox output)
        {
            _output = output;
        }

        public override void Write(char value)
        {
            base.Write(value);
            _output.AddText(Color.Green, value.ToString());
        }

        public override Encoding Encoding
        {
            get { return System.Text.Encoding.UTF8; }
        }
    }
}