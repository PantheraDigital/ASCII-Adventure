using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AsciiProgram
{
    public class LevelChangeObject : GameObject
    {
        public event EventHandler<string> LevelChange;
        string m_levelName;

        public LevelChangeObject(DisplayObject displayObject, string levelName)
            :base(displayObject, false, false)
        {
            m_levelName = levelName;
        }

        public override void OnCollide(MovingEntity other)
        {
            if(other.GetTags().Contains("player"))
            {
                LevelChange?.Invoke(this, m_levelName);
            }
        }
    }
}
