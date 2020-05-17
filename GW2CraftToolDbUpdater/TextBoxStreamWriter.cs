using System;
using System.Text;
using System.IO;
using System.Windows.Forms;

namespace GW2CraftToolDbUpdater
{
    public class TextBoxStreamWriter : TextWriter
    {
        TextBox _output = null;

        public TextBoxStreamWriter(TextBox output)
        {
            _output = output;
        }

        public override void Write(char value)
        {
            MethodInvoker action = delegate { _output.AppendText(value.ToString()); };
            _output.BeginInvoke(action);
        }
        public override void WriteLine(char value)
        {
            MethodInvoker action = delegate { _output.AppendText(value.ToString()+Environment.NewLine); };
            _output.BeginInvoke(action);
        }

        public override Encoding Encoding
        {
            get { return System.Text.Encoding.UTF8; }
        }
    }
}
