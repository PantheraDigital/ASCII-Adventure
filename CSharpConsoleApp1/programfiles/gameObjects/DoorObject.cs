using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AsciiProgram
{
    class DoorObject : GameObject
    {
        FastConsole.FastWrite fastWrite = FastConsole.FastWrite.GetInstance();
        public DoorObject(DisplayObject displayObject, string name = "none", string tags = "none")
            : base(displayObject, true, name)
        {
            if (tags != "none")
                m_tags = tags + " door";
            else
                m_tags = "door";
        }

        public override void OnCollide(MovingEntity other)
        {
            ComplexEntity obj = other as ComplexEntity;
            if (obj != null)
            {

                fastWrite.AddToBuffer(0, 0, "test", "Fire1");
                if (obj.HasComponent("Inventory") && obj.GetComponent<Inventory>("Inventory").GetObjectWithTag("key") != null)
                {
                    fastWrite.AddToBuffer(0, 0, "test", "Fire2");
                    m_solid = false;
                    m_displayObject.m_spriteChar = '.';
                }
            }
        }
    }
}
