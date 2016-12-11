using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MatijaClassLibrary
{
    public class DuplicateTodoItemException : Exception
    {
        public DuplicateTodoItemException() : base() { }
        public DuplicateTodoItemException(string message) : base(message) { }
    }
}

