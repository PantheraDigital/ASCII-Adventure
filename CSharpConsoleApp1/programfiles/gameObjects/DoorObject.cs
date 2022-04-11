using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AsciiProgram
{
    class DoorObject : GameObject
    {
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
            if (obj != null && m_solid)
            {
                if (obj.HasComponent("Inventory") && obj.GetComponent<Inventory>("Inventory").GetObjectWithTag("key") != null)
                {
                    m_solid = false;
                    m_displayObject.m_spriteChar = '.';
                }
            }
        }
    }
}
