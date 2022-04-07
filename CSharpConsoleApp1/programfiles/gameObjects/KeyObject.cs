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
            :base(displayObject, false)
        {
            if (name != "none")
                m_name = name;

            if (tags != "none")
                m_tags = tags;
        }

        public override void OnCollide(MovingEntity other)
        {
            //check if has inventory
            //add self to inventory

            //level will need to know to remove (pickup event?)
        }
    }
}
