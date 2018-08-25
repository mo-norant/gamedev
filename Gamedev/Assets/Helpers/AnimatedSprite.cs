using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Gamedev.Assets.Helpers
{
    public class AnimatedSprite
    {
        public Texture2D Texture { get; set; }
        public int Rows { get; set; }
        public int Columns { get; set; }
        private int currentFrame;
        private int totalFrames;
        private int frameCount;
        public int FrameWidth { get { return Texture.Width / totalFrames; }}
        public int FrameHeight { get { return Texture.Height; } }
        public bool IsLoop { get; set; }
        public Vector2 Origin
        {
            get { return new Vector2(FrameWidth/ 2.0f, FrameHeight); }
        }

        public AnimatedSprite(Texture2D texture, int rows, int columns, bool isLoop)
        {
            Texture = texture;
            Rows = rows;
            Columns = columns;
            currentFrame = 0;
            totalFrames = Rows * Columns;
            IsLoop = isLoop;
        }

        

        public void Draw(SpriteBatch spriteBatch, Vector2 location, SpriteEffects spriteEffects)
        {
            if(frameCount == 5)
            {
                currentFrame++;
                if (currentFrame == totalFrames)
                {
                    currentFrame = 0;
                    
                }
                frameCount = 0;
            }
          
            
            frameCount++;

            int width = Texture.Width / Columns;
            int height = Texture.Height / Rows;
            int row = (int)((float)currentFrame / (float)Columns);
            int column = currentFrame % Columns;

            Rectangle sourceRectangle = new Rectangle(width * column, height * row, width, height);
            Rectangle destinationRectangle = new Rectangle((int)location.X, (int)location.Y, width, height);
            Rectangle source = new Rectangle(width * column, height * row, width, height);

            spriteBatch.Draw(Texture, location, source, Color.White, 0.0f, Origin, 1.0f, spriteEffects, 0.0f);

        }
    }
}
