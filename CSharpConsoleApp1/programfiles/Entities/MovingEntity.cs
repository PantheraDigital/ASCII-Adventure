using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AsciiProgram
{
    public class Component
    {
        string m_type;

        public Component(string type)
        {
            m_type = type;
        }

        public string GetComponentType()
        {
            return m_type;
        }
    }

    public class Inventory : Component
    {
        List<GameObject> m_gameObjects;

        public Inventory()
            :base("Inventory")
        {
            m_gameObjects = new List<GameObject>();
        }

        public bool Add(GameObject gameObject)
        {
            if (gameObject != null)
            {
                m_gameObjects.Add(gameObject);
                return true;
            }

            return false;
        }

        public List<GameObject> GetObjects()
        {
            return m_gameObjects;
        }

        public bool Remove(GameObject gameObject)
        {
            if (gameObject != null && m_gameObjects.Contains(gameObject))
            {
                m_gameObjects.Remove(gameObject);
                return true;
            }
            else
                return false;
        }
    }

    public interface IComponentManager
    {
        bool HasComponent(string type);
        T GetComponent<T>(string type);//return component in its type
        bool AddComponent(Component component);
    }

    public class ComplexEntity : MovingEntity, IComponentManager
    {
        Dictionary<string, Component> m_components;

        public ComplexEntity(DisplayObject displayObject, Controller controller, string tags = "none")
            :base(displayObject, controller, tags)
        {
            m_components = new Dictionary<string, Component>();
        }

        public bool HasComponent(string type)
        {
            return false;
        }

        public T GetComponent<T>(string type)
        {
            if (m_components.ContainsKey(type))
                return (T)Convert.ChangeType(m_components[type], typeof(T));
            else
                return default(T);
        }

        public bool AddComponent(Component component)
        {
            if (m_components.ContainsKey(component.GetComponentType()))
                return false;
            else
            {
                m_components.Add(component.GetComponentType(), component);
                return true;
            }
        }
    }


    public class MovingEntity : Entity 
    {
        Vector2 m_movePosition;
        string m_tags;


        public MovingEntity(DisplayObject displayObject, Controller controller, string tags = "none")
            : base(displayObject, controller)
        {
            m_movePosition = m_displayObject.m_displayPosition;
            m_tags = tags;
        }

        public override void Update()
        {
            if (m_controller == null || m_displayObject == null)
                return;


            m_controller.Update();
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
        }

        public string GetTags()
        {
            return m_tags;
        }
    }
}

