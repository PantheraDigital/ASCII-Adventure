﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AsciiProgram
{
    public class LevelManager
    {
        FastConsole.FastWrite fastWrite = FastConsole.FastWrite.GetInstance();
        Level m_currentLevel;
        LevelCamera m_levelCam;
        ComplexEntity m_player;


        public LevelManager()
        {

        }

        public void Initialize(string levelName, ComplexEntity player)
        {
            m_player = player;
            m_currentLevel = Program.SetUpLevel(levelName, player);

            int levelCenterX = (int)(Console.WindowWidth / 2);// - (int)(m_currentLevel.GetMaxDimentions().x / 2);
            int levelCenterY = (int)(Console.WindowHeight / 2);// - (int)(m_currentLevel.GetMaxDimentions().y / 2);
            m_levelCam = new LevelCamera(new Vector2(0, 0), new Vector2(21, 21), new Vector2(levelCenterX, levelCenterY));
        }

        public void Run()
        {
            bool quit = false;
            while (quit == false)
            {
                m_currentLevel.Update();

                m_levelCam.CenterCameraOn(m_player.GetCurrentPosition());
                m_levelCam.UpdateDisplayList(m_currentLevel);

                m_levelCam.Draw();
                m_player.DrawComponents();

                fastWrite.DisplayBuffer();

                if (m_player.GetController().HasInput())
                {
                    if (m_player.GetController().GetInput().Key == ConsoleKey.Q)
                        quit = true;
                }

                /*
                if (stopwatch.IsRunning && stopwatch.Elapsed.TotalSeconds >= timeStep)
                {
                    //Console.SetCursorPosition(6, 6);
                    //Console.Write("No Input");

                    ++timesUpdated;
                    Console.SetCursorPosition(6, 5);
                    Console.Write("Update num: " + timesUpdated);
                    stopwatch.Restart();
                }

                if (controller.HasInput())
                {
                    if (controller.m_input.Key == ConsoleKey.Q)
                        quit = true;

                    //stopwatch.Restart();
                    Console.SetCursorPosition(6, 6);
                    Console.Write("Input: " + controller.m_input.KeyChar);
                }*/
            }
        }

        public void ChangeLevel(object sender, string newLevel)
        {
            m_currentLevel = Program.SetUpLevel(newLevel, m_player);
        }
    }
}
