using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AsciiProgram
{
    public class PlayerController : Controller
    {
        ConsoleKeyInfo m_input;
        bool m_hasInput;

        public override void Update()
        {
            if (Console.KeyAvailable == true)
            {
                m_input = Console.ReadKey(true);
                m_hasInput = true;
            }
            else
                m_hasInput = false;
        }

        public override bool HasInput()
        {
            return m_hasInput;
        }

        public override ConsoleKeyInfo GetInput()
        {
            return m_input;
        }
    }
}
