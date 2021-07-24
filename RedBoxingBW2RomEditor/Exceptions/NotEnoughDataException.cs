using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RedBoxingBW2RomEditor.Exceptions
{
    public class NotEnoughDataException : IOException
    {
        private long currentOutSize;
        private long totalOutSize;
        /// <summary>
        /// Gets the actual number of written bytes.
        /// </summary>
        public long WrittenLength { get { return this.currentOutSize; } }
        /// <summary>
        /// Gets the number of bytes that was supposed to be written.
        /// </summary>
        public long DesiredLength { get { return this.totalOutSize; } }

        /// <summary>
        /// Creates a new NotEnoughDataException.
        /// </summary>
        /// <param name="currentOutSize">The actual number of written bytes.</param>
        /// <param name="totalOutSize">The desired number of written bytes.</param>
        public NotEnoughDataException(long currentOutSize, long totalOutSize)
            : base(String.Format("Not enough data available; 0x{0} of {1} bytes written.", currentOutSize.ToString("X"),
                (totalOutSize < 0 ? "???" : ("0x" + totalOutSize.ToString("X")))))
        {
            this.currentOutSize = currentOutSize;
            this.totalOutSize = totalOutSize;
        }
    }
}
