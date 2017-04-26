using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace tlsh.exceptions
{
    class InsufficientComplexityException : Exception
    {
        public InsufficientComplexityException(string message):base(message)
        {            
        }
    }
}
