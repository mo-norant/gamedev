using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using MonoGame.Extended;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Gamedev.Assets.UI
{
    public class HealthUI
    {
        private ContentManager contentManager;
        private Camera2D camera;
        private int lives;
        private Texture2D heartTexture;

        public HealthUI(ContentManager contentManager, Camera2D camera)
        {
            this.contentManager = contentManager;
            this.camera = camera;
            LoadContent();
        }

        private void LoadContent()
        {
            heartTexture = contentManager.Load<Texture2D>("Other/heart");
        }

        public void Update(GameTime gameTime, int updatetLives)
        {
            lives = updatetLives;
        }

        public void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            Vector2 p = camera.Position;
            const int SPACINGX = 60;
            //init spacing

            p += new Vector2(300, 100);

            for (int i = 0; i < lives; i++)
            {
                p += new Vector2(SPACINGX , 0);
                spriteBatch.Draw(heartTexture, p, Color.White);
            }

        }

    }
}
