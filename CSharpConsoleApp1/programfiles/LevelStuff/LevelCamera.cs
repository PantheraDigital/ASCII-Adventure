using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using FastConsole;


namespace AsciiProgram
{

    public class LevelCamera
    {
        struct DisplayData
        {
            public DisplayObject display;
            public bool fresh;

            public DisplayData(DisplayObject display, bool fresh)
            {
                this.display = display;
                this.fresh = fresh;
            }
        }

        Dictionary<Vector2, DisplayData> m_displayList;

        Vector2 m_displayOffset;//place on screen to display
        Vector2 m_displayBounds;//how much is displayed
        Vector2 m_cameraPosition;//cam pos in level

        FastWrite m_fastWrite;
        char m_emptyChar;


        public LevelCamera(Vector2 position, Vector2 displayBounds, Vector2 displayOffset)
        {
            m_cameraPosition = position;
            m_emptyChar = ' ';

            InitalizeDisplaySize(displayBounds);

            m_displayOffset = displayOffset;
            m_displayOffset.x -= (int)(displayBounds.x / 2);
            m_displayOffset.y -= (int)(displayBounds.y / 2);

            m_fastWrite = FastWrite.GetInstance();
        } 


        public void SetEmptyChar(char emptyChar)
        {
            m_emptyChar = emptyChar;
        }

        public void SetDisplayOffset(Vector2 offset)
        {
            if (offset.x > 0 && offset.y > 0)
                m_displayOffset = offset;
        }

        public void SetCameraPosition(Vector2 position)
        {
            m_cameraPosition = position;
        }

        public void CenterCameraOn(Vector2 position)
        {
            position.x -= (int)(m_displayBounds.x / 2);
            position.y -= (int)(m_displayBounds.y / 2);
            SetCameraPosition(position);
        }

        public void InitalizeDisplaySize(Vector2 size)
        {
            m_displayList = new Dictionary<Vector2, DisplayData>();

            for (int y = 0; y < size.y; ++y)
            {
                for (int x = 0; x < size.x; ++x)
                    m_displayList.Add(new Vector2(x, y), new DisplayData(new DisplayObject(m_emptyChar, new Vector2(x, y)), true));
            }

            m_displayBounds = size;
        }

        public void UpdateDisplayList(Level currentLevel)
        {
            foreach (Vector2 key in m_displayList.Keys.ToList())
            {
                int levelSpaceY = m_cameraPosition.y + key.y;
                int levelSpaceX = m_cameraPosition.x + key.x;

                //check for game objects
                if(currentLevel.ValidateGameObjectKey(new Vector2(levelSpaceX, levelSpaceY)))
                {
                    if (!m_displayList[key].display.IsEqual(currentLevel.GetGameObject(new Vector2(levelSpaceX, levelSpaceY)).m_displayObject))
                        m_displayList[key] = new DisplayData(currentLevel.GetGameObject(new Vector2(levelSpaceX, levelSpaceY)).m_displayObject, true);
                }
                //if position has a tile (if position in camera space has a tile in it)
                else if (currentLevel.ValidTile(levelSpaceX, levelSpaceY))
                {
                    //lookup display object at screenspace location
                    //if not equal to display at location in level
                    if (!m_displayList[key].display.IsEqual(currentLevel.GetTile(levelSpaceX, levelSpaceY).m_displayObject))
                        m_displayList[key] = new DisplayData(currentLevel.GetTile(levelSpaceX, levelSpaceY).m_displayObject, true);
                }
                else
                {
                    if (m_displayList[key].display.m_spriteChar != m_emptyChar)
                    {
                        m_displayList[key] = new DisplayData(new DisplayObject(m_emptyChar, new Vector2(levelSpaceX, levelSpaceY)), true);
                    }
                }
            }

            //loop through moveEntities in level
            //if position is in screen space (x and y are between cameraPos and cameraPos + displayBounds)
            //set display at key entity.pos to entity,displayObject
            for (int i = 0; i < currentLevel.GetNumMovingEntities(); ++i)
            {
                Vector2 tempPos = currentLevel.GetMovingEntity(i).GetCurrentPosition();
                if (tempPos.x >= m_cameraPosition.x && tempPos.x < m_cameraPosition.x + m_displayBounds.x)
                {
                    if (tempPos.y >= m_cameraPosition.y && tempPos.y < m_cameraPosition.y + m_displayBounds.y)
                    {
                        //get entitys screen space location
                        Vector2 screenPos = new Vector2(tempPos.x - m_cameraPosition.x, tempPos.y - m_cameraPosition.y);
                        if (m_displayList.ContainsKey(screenPos))
                            m_displayList[screenPos] = new DisplayData(currentLevel.GetMovingEntity(i).m_displayObject, true);

                    }
                }
            }
        }

        public void Draw()
        {
            foreach (Vector2 key in m_displayList.Keys.ToList())
            {
                if (m_displayList[key].fresh)
                {
                    Vector2 temp = m_displayOffset;
                    temp.x -= m_cameraPosition.x;
                    temp.y -= m_cameraPosition.y;

                    temp.x = m_displayList[key].display.m_displayPosition.x + temp.x;
                    temp.y = m_displayList[key].display.m_displayPosition.y + temp.y;

                    m_fastWrite.AddToBuffer(temp.x, temp.y, "GameLevel", m_displayList[key].display.m_spriteChar, m_displayList[key].display.m_foregroudColor, m_displayList[key].display.m_backgroundColor);
                    m_displayList[key] = new DisplayData(m_displayList[key].display, false);
                }
            }
        }

        public void Clear()
        {
            m_fastWrite.ClearLayer("GameLevel");
        }
    }
}
