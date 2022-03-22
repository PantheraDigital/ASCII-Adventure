using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AsciiProgram
{

    public class MovingEntity : Entity//make interface? IMove? 
    {
        Vector2 m_movePosition;
        bool m_moved;


        public MovingEntity(DisplayObject displayObject, Controller controller)
            : base(displayObject, controller)
        {
            m_movePosition = m_displayObject.m_displayPosition;
            m_moved = false;
        }

        public override void Update()
        {
            if (m_controller == null || m_displayObject == null)
                return;


            m_controller.Update();

            if (m_moved)
                m_moved = false;
            else
                m_movePosition = m_displayObject.m_displayPosition;


            if (m_controller.HasInput())
            {
                switch (m_controller.GetInput().Key)
                {
                    case ConsoleKey.W:
                        m_movePosition.y = m_displayObject.m_displayPosition.y - 1;
                        break;
                    case ConsoleKey.S:
                        m_movePosition.y = m_displayObject.m_displayPosition.y + 1;
                        break;
                    case ConsoleKey.A:
                        m_movePosition.x = m_displayObject.m_displayPosition.x - 1;
                        break;
                    case ConsoleKey.D:
                        m_movePosition.x = m_displayObject.m_displayPosition.x + 1;
                        break;
                    default:
                        break;
                }
            }

        }

        public Vector2 GetMoveLocation()
        {
            return m_movePosition;
        }

        public void Move()
        {
            m_displayObject.m_displayPosition = m_movePosition;
            m_moved = true;
        }

    }
}

