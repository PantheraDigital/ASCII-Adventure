using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AsciiProgram
{
    public class Tile
    {
        public DisplayObject m_displayObject { get; protected set; }
        public bool m_solid { get; set; }

        public Tile(DisplayObject displayObject)
        {
            m_displayObject = displayObject;
            m_solid = false;
        }

        public virtual void Update() { }
        public virtual void OnCollide() { }
    }
}

