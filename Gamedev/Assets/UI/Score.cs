﻿using Microsoft.Xna.Framework;
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
    public class Score
    {
        private ContentManager contentManager;
        private SpriteFont scoreFont;
        private Camera2D camera;
        public int _Score { get; set; }


        public Score(ContentManager contentManager, Camera2D camera)
        {
            this.contentManager = contentManager;
            this.camera = camera;
            LoadContent();
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
            Vector2 p = camera.Position;

            //Spacing

            p += new Vector2(50, 100);
            
            spriteBatch.DrawString(scoreFont, $"Score: {_Score.ToString()}", p, Color.Red, 0f, Vector2.Zero, 2f, SpriteEffects.None, 0f);

        }
    }
}
