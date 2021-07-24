using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RedBoxingBW2RomEditor.Exceptions
{
    public class TooMuchInputException : Exception
    {
        /// <summary>
        /// Gets the number of bytes read by the decompressed to decompress the stream.
        /// </summary>
        public long ReadBytes { get; private set; }

        /// <summary>
        /// Creates a new exception indicating that the input has more data than necessary for
        /// decompressing th stream. It may indicate that other data is present after the compressed
        /// stream.
        /// </summary>
        /// <param name="readBytes">The number of bytes read by the decompressor.</param>
        /// <param name="totLength">The indicated length of the input stream.</param>
        public TooMuchInputException(long readBytes, long totLength)
            : base(String.Format("The input contains more data than necessary. Only used 0x{0} of 0x{1} bytes", readBytes.ToString("X"), totLength.ToString("X")))
        {
            this.ReadBytes = readBytes;
        }
    }
}
