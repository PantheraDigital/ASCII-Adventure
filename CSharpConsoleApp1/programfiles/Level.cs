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


        public Level(List<List<Tile>> tiles, List<MovingEntity> movingEntities)
        {
            m_tiles = tiles;
            m_movingEntities = movingEntities;


            Vector2 temp = new Vector2(0, 0);
            temp.y = m_tiles.Count;
            for (int y = 0; y < m_tiles.Count; ++y)
            {
                if (m_tiles[y].Count - 1 > temp.x)
                    temp.x = m_tiles[y].Count;
            }

            m_maxDimensions = temp;
        }

        public void Update()
        {
            for (int y = 0; y < m_tiles.Count; ++y)
            {
                for (int x = 0; x < m_tiles[y].Count; ++x)
                {
                    m_tiles[y][x].Update();
                }
            }

            for (int i = 0; i < m_movingEntities.Count; ++i)
            {
                m_movingEntities[i].Update();

                if (!m_movingEntities[i].GetMoveLocation().IsEqual(m_movingEntities[i].GetCurrentPosition()))
                {
                    if (ValidateMove(m_movingEntities[i].GetMoveLocation()))
                    {
                        m_movingEntities[i].Move();


                        Vector2 coverdTilePos = m_movingEntities[i].GetCurrentPosition();
                        m_tiles[coverdTilePos.y][coverdTilePos.x].OnCollide(m_movingEntities[i]);
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
    }
}
