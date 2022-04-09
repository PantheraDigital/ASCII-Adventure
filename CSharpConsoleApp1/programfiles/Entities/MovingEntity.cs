using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AsciiProgram
{
    public abstract class Component
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

        public virtual void Draw() { }
    }

    public class Inventory : Component
    {

        FastConsole.FastWrite fastWrite = FastConsole.FastWrite.GetInstance();
        List<GameObject> m_gameObjects;
        GameWindow m_display;

        public Inventory(GameWindow gameWindow)
            :base("Inventory")
        {
            m_gameObjects = new List<GameObject>();
            m_display = gameWindow;
            m_display.SetMessage("");
        }

        public override void Draw()
        {
            if (m_display != null)
            {
                m_display.Draw();
            }

            fastWrite.SetCursorPosition(m_display.GetScreenPosition());
            foreach (GameObject obj in m_gameObjects)
                fastWrite.AddToBuffer("test", obj.m_displayObject.m_spriteChar);
        }

        public bool Add(GameObject gameObject)
        {
            if (gameObject != null)
            {
                m_gameObjects.Add(gameObject);
                m_display.AddToMessage(gameObject.m_displayObject.m_spriteChar.ToString());
                return true;
            }

            return false;
        }

        public List<GameObject> GetObjects()
        {
            return m_gameObjects;
        }

        public bool HasObjectWithTag(string tag)
        {
            foreach(GameObject obj in m_gameObjects)
            {
                if (obj.m_tags.Contains(tag))
                    return true;
            }

            return false;
        }

        public GameObject GetObjectWithTag(string tag)
        {
            for (int i = 0; i < m_gameObjects.Count; ++i)
            {
                if (m_gameObjects[i].m_tags.Contains(tag))
                {
                    GameObject temp = m_gameObjects[i];
                    m_gameObjects.RemoveAt(i);
                    return temp;
                }
            }

            return null;
        }

        public bool HasObject(string name)
        {
            foreach (GameObject obj in m_gameObjects)
            {
                if (obj.m_name.Equals(name))
                    return true;
            }

            return false;
        }

        public GameObject GetObjectByName(string name)
        {
            for (int i = 0; i < m_gameObjects.Count; ++i)
            {
                if (m_gameObjects[i].m_name.Equals(name))
                {
                    GameObject temp = m_gameObjects[i];
                    m_gameObjects.Remove(m_gameObjects[i]);
                    return temp;
                }
            }

            return null;
        }

        public bool Remove(GameObject gameObject)
        {
            if (gameObject != null && m_gameObjects.Contains(gameObject))
            {
                m_gameObjects.Remove(gameObject);
                m_display.RemoveFromMessage(gameObject.m_displayObject.m_spriteChar.ToString());
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
        void DrawComponents();
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
            if (m_components.ContainsKey(type))
                return true;
            else
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
        public void DrawComponents()
        {
            foreach(string key in m_components.Keys)
            {
                m_components[key].Draw();
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

