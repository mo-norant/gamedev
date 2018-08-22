using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Gamedev.Assets.UI
{
    public class Score
    {
        private ContentManager contentManager;
        private Vector2 position;
        private SpriteFont scoreFont;
        public int _Score { get; set; }


        public Score(ContentManager contentManager)
        {
            this.contentManager = contentManager;
            LoadContent();
            position = new Vector2(100, 100);

        }

        private void LoadContent()
        {
            scoreFont = contentManager.Load<SpriteFont>("Fonts/Gameplay");
        }

        public void Update(GameTime gameTime)
        {
            
        }

        public void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            spriteBatch.DrawString(scoreFont, $"Score: {_Score.ToString()}", position, Color.Red, 0f, Vector2.Zero, 2f, SpriteEffects.None, 0f);

        }
    }
}
