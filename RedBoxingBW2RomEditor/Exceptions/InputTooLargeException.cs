using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RedBoxingBW2RomEditor.Exceptions
{
    public class InputTooLargeException : Exception
    {
        public InputTooLargeException()
            : base("The compression ratio is not high enough to fit the input in a single compressed file.") { }
    }
}
