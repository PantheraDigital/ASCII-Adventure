using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AsciiProgram
{
    class DoorObject : GameObject
    {
        GameWindow m_closedMessage;
        MovingEntity m_collider;

        public DoorObject(DisplayObject displayObject, string name = "none", string tags = "none")
            : base(displayObject, true, false, name)
        {
            if (tags != "none")
                m_tags = tags + " door";
            else
                m_tags = "door";

            m_collider = null;

            m_closedMessage = new GameWindow("LockedDoor", new Vector2(10, 10), new Vector2(20, 8), '.', ConsoleColor.Gray, ConsoleColor.Black);
            m_closedMessage.SetMessage("This door is locked. You will need a (k)ey.");
            m_closedMessage.SetBorderChar('!', ConsoleColor.Red, ConsoleColor.Black);
        }

        public override void OnCollide(MovingEntity other)
        {
            ComplexEntity obj = other as ComplexEntity;
            if (obj != null && m_solid && obj.HasTag("player"))
            {
                if (obj.HasComponent("Inventory") && obj.GetComponent<Inventory>("Inventory").GetObjectWithTag("key") != null)
                {
                    m_solid = false;
                    m_displayObject.m_spriteChar = '.';
                    m_collider = null;
                }
                else
                {
                    m_collider = other;
                    m_closedMessage.Draw();
                }
            }
        }

        public override void Update()
        {
            if(m_closedMessage.IsActive() && m_collider != null)
            {
                if(Math.Abs(m_collider.GetCurrentPosition().x - m_displayObject.m_displayPosition.x) >= 2 ||
                    Math.Abs(m_collider.GetCurrentPosition().y - m_displayObject.m_displayPosition.y) >= 2)
                {
                    m_closedMessage.Erase();
                    m_collider = null;
                }
            }
        }
    }
}
