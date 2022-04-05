﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AsciiProgram
{
    public class LevelChangeObject : GameObject
    {
        LevelManager m_levelManager;
        string m_levelName;

        public LevelChangeObject(DisplayObject displayObject, LevelManager levelManager, string levelName)
            :base(displayObject, false)
        {
            m_levelName = levelName;
            m_levelManager = levelManager;
        }

        public override void OnCollide(MovingEntity other)
        {
            if(other.GetTags().Contains("player"))
            {
                m_levelManager.ChangeLevel(m_levelName);
            }
        }
    }
}
