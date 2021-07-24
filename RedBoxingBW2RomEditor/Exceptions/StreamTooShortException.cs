using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RedBoxingBW2RomEditor.Exceptions
{
    public class StreamTooShortException : EndOfStreamException
    {
        public StreamTooShortException()
            : base("The end of the stream was reached before the given amount of data was read.")
        { }
    }
}
