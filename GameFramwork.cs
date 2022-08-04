using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace 坦克大战
{
    enum GameState
    {
        Running,
        GameOver
    }

    internal class GameFramwork
    {
        public static Graphics g;
        private static GameState gameState = GameState.Running;

        public static void Start()
        {
            SoundManager.InitSound();
            GameObjectManager.Start();
            GameObjectManager.CreateMap();
            GameObjectManager.CreateMyTank();
            SoundManager.PlayStart();
        }

        public static void Update()
        {
            //GameObjectManager.DrawMap();
            //GameObjectManager.DrawMyTank();
            if(gameState==GameState.Running)
            {
                GameObjectManager.Update();
            }
            else if(gameState==GameState.GameOver)
            {
                GameOverUpdate();
            }
        }

        private static void GameOverUpdate()
        {
            int x = 450 / 2 - Resources.GameOver.Width / 2;
            int y = 450 / 2 - Resources.GameOver.Height / 2;
            g.DrawImage(Resources.GameOver, x, y);
        }

        public static void ChangeToGameOver()
        {
            gameState = GameState.GameOver;
        }
    }
}
