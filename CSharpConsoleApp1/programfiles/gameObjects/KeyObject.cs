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
            :base(displayObject, false, name)
        {
            if (tags == "none")
                m_tags = "key";
            else
                m_tags += " key";
        }

        public override void OnCollide(MovingEntity other)
        {
            ComplexEntity obj = other as ComplexEntity;
            if (obj != null)
            {
                if (obj.HasComponent("Inventory"))
                {
                    obj.GetComponent<Inventory>("Inventory").Add(this);
                }
            }

            //level will need to know to remove (pickup event?)
        }
    }
}
