using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nayuki749.Speech_to_Text
{
    public class SpeechCanceledEventArgs : EventArgs
    {

        public string Message;

        public SpeechCanceledEventArgs()
        {

        }
    }
}
