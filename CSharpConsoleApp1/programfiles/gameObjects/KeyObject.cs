using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AsciiProgram
{
    class KeyObject : GameObject
    {

        FastConsole.FastWrite fastWrite = FastConsole.FastWrite.GetInstance();
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

            fastWrite.AddToBuffer(0, 0, this.ToString(), "fire1");
            IComponentManager obj = other as IComponentManager;
            if(obj != null && obj.HasComponent("Inventory"))
            {
                fastWrite.AddToBuffer(0, 0, this.ToString(), "fire2");
                obj.GetComponent<Inventory>("Inventory").Add(this);
            }

            //level will need to know to remove (pickup event?)
        }
    }
}
