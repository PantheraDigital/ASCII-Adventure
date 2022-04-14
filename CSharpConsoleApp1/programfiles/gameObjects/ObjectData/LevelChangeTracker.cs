using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AsciiProgram
{
    static class LevelChangeTracker
    {
        struct LevelChanges
        {
            public static string[] mazeLevel1Changes = { "mazeLevel2" };
            public static string[] mazeLevel2Changes = { "mazeLevel1" };
        }

        static int m_currentLevelChangeIndex = 0;
        static string m_currentLevel = "null";
        static string[] m_currentLevelChanges = null;


        static void SetCurrentLevel(string levelName)
        {
            if (!levelName.Equals(m_currentLevel))
            {
                m_currentLevel = levelName;
                m_currentLevelChangeIndex = 0;
                switch (levelName)
                {
                    case "mazeLevel1":
                        m_currentLevelChanges = LevelChanges.mazeLevel1Changes;
                        break;

                    case "mazeLevel2":
                        m_currentLevelChanges = LevelChanges.mazeLevel2Changes;
                        break;

                    default:
                        m_currentLevelChanges = null;
                        break;
                }
            }
        }

        public static string GetNextLevelChange(string levelName)
        {
            SetCurrentLevel(levelName);
            if (m_currentLevelChanges == null)
                return null;
            else
            {
                if (m_currentLevelChangeIndex >= m_currentLevelChanges.Length)
                    return null;

                ++m_currentLevelChangeIndex;
                return m_currentLevelChanges[m_currentLevelChangeIndex - 1];
            }
        }

    }
}
