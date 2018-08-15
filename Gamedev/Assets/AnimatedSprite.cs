using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace Gamedev.Assets
{
    public class AnimatedSprite
    {
        public Texture2D Texture { get; set; }
        public int Rows { get; set; }
        public int Columns { get; set; }
        private int currentFrame;
        private int totalFrames;
        private bool leftMirrored = false;


        public Vector2 Position { get; set; }
        public Vector2 MoveFactor { get; set; }

        public AnimatedSprite(Texture2D texture, int rows, int columns)
        {
            Texture = texture;
            Rows = rows;
            Columns = columns;
            currentFrame = 0;
            totalFrames = Rows * Columns;
        }

        public void Update(GameTime gameTime)
        {
         


            CheckKeyboardAndUpdateMovement();
            SimulateFriction();
            UpdatePositionBasedOnMovement(gameTime);

        }

        public void Draw(SpriteBatch spriteBatch, Vector2 location)
        {
            int width = Texture.Width / Columns;
            int height = Texture.Height / Rows;
            int row = (int)((float)currentFrame / (float)Columns);
            int column = currentFrame % Columns;

            Rectangle sourceRectangle = new Rectangle(width * column, height * row, width, height);
            Rectangle destinationRectangle = new Rectangle((int)location.X, (int)location.Y, width, height);

            spriteBatch.Begin();
            if (leftMirrored)
            {
                spriteBatch.Draw(Texture, destinationRectangle, sourceRectangle, Color.White, 0.0f, new Vector2(0, 0), SpriteEffects.FlipHorizontally, 0.0f);
            }
            else
            {
                spriteBatch.Draw(Texture, destinationRectangle, sourceRectangle, Color.White);
            }
            spriteBatch.End();


        }

        private void CheckKeyboardAndUpdateMovement()
        {
            KeyboardState keyboardState = Keyboard.GetState();
            bool keyboardtouched = false;
            if (keyboardState.IsKeyDown(Keys.Left)) { MoveFactor += new Vector2(-1, 0);
                keyboardtouched = true;
                if(leftMirrored == false)
                {
                    leftMirrored = true;
                }
            }
            if (keyboardState.IsKeyDown(Keys.Right)) { MoveFactor += new Vector2(1, 0); keyboardtouched = true;
                if (leftMirrored)
                {
                    leftMirrored = false;
                }

            }
            if (keyboardState.IsKeyDown(Keys.Up)) { MoveFactor += new Vector2(0, -1); keyboardtouched = true; }

            if (keyboardtouched)
            {
                currentFrame++;
                if (currentFrame == totalFrames)
                    currentFrame = 0;
            }

        }

        private void SimulateFriction()
        {
            MoveFactor -= MoveFactor * new Vector2(.2f, .2f);
        }

        private void UpdatePositionBasedOnMovement(GameTime gameTime)
        {
            Position += MoveFactor * (float)gameTime.ElapsedGameTime.TotalMilliseconds / 15;
        }


    }
}
