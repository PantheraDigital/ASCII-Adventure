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
        public virtual void OnCollide(MovingEntity collider) { }
    }

    public abstract class TriggerFunctor
    {
        public abstract void Trigger(MovingEntity movingEntity);
        public abstract void EndTrigger();
    }

    public class ShowWindowTrigger : TriggerFunctor
    {
        GameWindow m_window;
        int m_layerToDraw;

        public ShowWindowTrigger(GameWindow gameWindow, int layerToDraw)
        {
            m_window = gameWindow;
            m_layerToDraw = layerToDraw;
        }

        public override void Trigger(MovingEntity movingEntity)
        {
            m_window.Draw(m_layerToDraw);
            
            Console.ReadKey(true);
            EndTrigger();
        }

        public override void EndTrigger()
        {
            if(m_window.IsActive())
            {
                m_window.Erase();
            }
        }
    }

    public class TriggerTile : Tile
    {
        TriggerFunctor m_triggerFunctor;
        string[] m_tagsToCheck;

        public TriggerTile(DisplayObject displayObject, TriggerFunctor trigger, string tagsToCheck = "none")
            : base(displayObject)
        {
            m_triggerFunctor = trigger;
            m_tagsToCheck = tagsToCheck.Split(' ');
        }

        public override void OnCollide(MovingEntity collider)
        {
            if (m_tagsToCheck.Equals("none"))
                m_triggerFunctor.Trigger(collider);
            else
            {
                bool triggered = false;
                for(int i = 0; i < m_tagsToCheck.Length; ++i)
                {
                    if (collider.GetTags().Contains(m_tagsToCheck[i]) && triggered == false)
                    {
                        m_triggerFunctor.Trigger(collider);
                        triggered = true;
                    }
                }
            }
        }

    }
}

