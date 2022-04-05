using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AsciiProgram
{
    public abstract class Controller
    {
        public abstract void Update();
        public abstract ConsoleKeyInfo GetInput();
        public abstract bool HasInput();
    }
}
