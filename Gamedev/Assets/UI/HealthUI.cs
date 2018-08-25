using Gamedev.Assets.Helpers;
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
    public class HealthUI : IPipelineBase
    {
        private Camera2D camera;
        private Texture2D heartTexture;
        public int Lives { get; set; }

        public HealthUI(ContentManager contentManager, Camera2D camera)
        {
            this.camera = camera;
            LoadContent(contentManager);
        }

        public void LoadContent(ContentManager contentManager)
        {
            heartTexture = contentManager.Load<Texture2D>("Other/heart");
        }

        public void Update(GameTime gameTime)
        {
            
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            Vector2 p = camera.Position;
            const int SPACINGX = 60;

            p += new Vector2(300, 100);

            for (int i = 0; i < Lives; i++)
            {
                p += new Vector2(SPACINGX , 0);
                spriteBatch.Draw(heartTexture, p, Color.White);
            }

        }

    }
}
