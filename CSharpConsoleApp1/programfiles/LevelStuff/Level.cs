using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace AsciiProgram
{
    public class Level
    {
        List<List<Tile>> m_tiles;
        List<MovingEntity> m_movingEntities;
        Vector2 m_maxDimensions;

        Vector2 m_playerSpawn;
        Dictionary<Vector2, GameObject> m_gameObjects;


        public Level(List<List<Tile>> tiles, List<MovingEntity> movingEntities, Dictionary<Vector2, GameObject> gameObjects, Vector2 playerSpawn, MovingEntity player)
        {
            m_tiles = tiles;
            m_movingEntities = movingEntities;
            m_gameObjects = gameObjects;
            m_playerSpawn = playerSpawn;


            Vector2 temp = new Vector2(0, 0);
            temp.y = m_tiles.Count;
            for (int y = 0; y < m_tiles.Count; ++y)
            {
                if (m_tiles[y].Count - 1 > temp.x)
                    temp.x = m_tiles[y].Count;
            }

            m_maxDimensions = temp;
            player.SetPosition(m_playerSpawn);
        }

        public void Update()
        {
            foreach (Vector2 key in m_gameObjects.Keys)
            {
                m_gameObjects[key].Update();
            }

            for (int i = 0; i < m_movingEntities.Count; ++i)
            {
                m_movingEntities[i].Update();

                if (!m_movingEntities[i].GetMoveLocation().IsEqual(m_movingEntities[i].GetCurrentPosition()))
                {
                    if (ValidateMove(m_movingEntities[i].GetMoveLocation()))
                    {
                        if (m_gameObjects.ContainsKey(m_movingEntities[i].GetCurrentPosition()))
                            m_gameObjects[m_movingEntities[i].GetCurrentPosition()].EndCollide();

                        if (m_gameObjects.ContainsKey(m_movingEntities[i].GetMoveLocation()) && m_gameObjects[m_movingEntities[i].GetMoveLocation()].m_solid)
                            m_gameObjects[m_movingEntities[i].GetMoveLocation()].OnCollide(m_movingEntities[i]);
                        else
                            m_movingEntities[i].Move();

                        Vector2 coverdTilePos = m_movingEntities[i].GetCurrentPosition();
                        //m_tiles[coverdTilePos.y][coverdTilePos.x].OnCollide(m_movingEntities[i]);

                        if (m_gameObjects.ContainsKey(coverdTilePos))
                            m_gameObjects[coverdTilePos].OnCollide(m_movingEntities[i]);
                    }
                }
            }
        }


        bool ValidateMove(Vector2 positionToValidate)
        {
            if (positionToValidate.y >= 0 && positionToValidate.y < m_tiles.Count)
            {
                if (positionToValidate.x >= 0 && positionToValidate.x < m_tiles[positionToValidate.y].Count)
                    if (!m_tiles[positionToValidate.y][positionToValidate.x].m_solid)
                        return true;
            }

            return false;
        }

        public Vector2 GetMaxDimentions()
        {
            return m_maxDimensions;
        }

        public Tile GetTile(int indexX, int indexY)
        {
            if (ValidTile(indexX, indexY))
                return m_tiles[indexY][indexX];

            return new Tile(new DisplayObject('*', new Vector2(indexX, indexY)));
        }

        public bool ValidTile(int indexX, int indexY)
        {
            if (indexY >= 0 && indexY < GetNumRows())
            {
                if (indexX >= 0 && indexX < GetRowLength(indexY))
                    return true;
            }

            return false;
        }

        public int GetRowLength(int rowIndex)
        {
            if (rowIndex >= 0 && rowIndex < GetNumRows())
                return m_tiles[rowIndex].Count;

            return 0;
        }

        public int GetNumRows()
        {
            return m_tiles.Count;
        }

        public MovingEntity GetMovingEntity(int index)
        {
            if (index >= 0 && index < m_movingEntities.Count)
                return m_movingEntities[index];

            return null;
        }

        public int GetNumMovingEntities()
        {
            return m_movingEntities.Count;
        }

        public bool ValidateGameObjectKey(Vector2 keyLocation)
        {
            if (m_gameObjects.ContainsKey(keyLocation))
                return true;
            else
                return false;
        }

        public GameObject GetGameObject(Vector2 keyLocation)
        {
            if (m_gameObjects.ContainsKey(keyLocation))
                return m_gameObjects[keyLocation];
            else
                return null;
        }
    }
}
