using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AsciiProgram
{
    class KeyObject : GameObject
    {
        public KeyObject(DisplayObject displayObject, string name = "none", string tags = "none")
            :base(displayObject, false, true, name)
        {
            m_pickUp = true;
            if (tags == "none")
                m_tags = "key";
            else
                m_tags += " key";
        }

    }
}
