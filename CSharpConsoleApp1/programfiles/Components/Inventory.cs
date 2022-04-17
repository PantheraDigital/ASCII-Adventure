using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AsciiProgram
{
    public class Inventory : Component
    {
        //FastConsole.FastWrite fastWrite = FastConsole.FastWrite.GetInstance();
        List<GameObject> m_gameObjects;
        GameWindow m_display;
        int m_sizeLimit;

        public Inventory(GameWindow gameWindow, int sizeLimit = 0)
            : base("Inventory")
        {
            m_gameObjects = new List<GameObject>();
            m_display = gameWindow;
            m_display.SetMessage("  empty  ");
            m_sizeLimit = sizeLimit;
        }

        public override void Draw()
        {
            if (m_display != null)
            {
                m_display.Draw();
            }

            /*
            Vector2 pos = m_display.GetScreenPosition();
            fastWrite.SetCursorPosition(pos);
            if(m_gameObjects.Count == 0)
            {
                fastWrite.AddToBuffer("test", "empty");
            }
            else
            {
                fastWrite.ClearLayer("test");
                foreach (GameObject obj in m_gameObjects)
                {
                    fastWrite.AddToBuffer("test", obj.m_displayObject.m_spriteChar.ToString());
                }
            }*/

        }

        public bool Add(GameObject gameObject)
        {
            if (m_sizeLimit > 0 && m_gameObjects.Count >= m_sizeLimit)
                return false;

            if (m_gameObjects.Count == 0)
                m_display.SetMessage("");

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
            foreach (GameObject obj in m_gameObjects)
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
                    m_display.RemoveFromMessage(temp.m_displayObject.m_spriteChar.ToString());
                    m_gameObjects.Remove(m_gameObjects[i]);

                    if (m_gameObjects.Count == 0)
                        m_display.SetMessage("  empty  ");

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
    }


}
