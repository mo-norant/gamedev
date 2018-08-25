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
    public class FirePowerUI
    {
        private SpriteFont scoreFont;
        private Camera2D camera;
        public int FirePower { get; set; }


        public FirePowerUI(ContentManager contentManager, Camera2D camera)
        {
            this.camera = camera;
            LoadContent(contentManager);
        }

        public void LoadContent(ContentManager contentManager)
        {
            scoreFont = contentManager.Load<SpriteFont>("Fonts/Gameplay");
        }

        public void Update(GameTime gameTime)
        {

        }

        public void Draw(SpriteBatch spriteBatch)
        {
            Vector2 p = camera.Position;


            p += new Vector2(800, 100);

            spriteBatch.DrawString(scoreFont, $"{FirePower.ToString()} X FIREPOWER", p, Color.Red, 0f, Vector2.Zero, 2f, SpriteEffects.None, 0f);

        }
    }
}
