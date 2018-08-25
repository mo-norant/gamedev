using Gamedev.Assets.Helpers;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Gamedev.Assets.Entitities
{
    public abstract  class BulletBase : IDebugble
    {

        public BulletBase(GraphicsDevice graphicsDevice)
        {
            this.GraphicsDevice = graphicsDevice;
            
        }


        public AnimatedSprite animatedSprite { get; set; }
        private Rectangle localBounds;
        public Direction direction;
        public Texture2D BulletTexture { get; set; }
        //al schade uitgehaald
        public bool IsNotEffective { get; set; }
        public Vector2 Position { get; set; }
        public Vector2 InitPosition { get; set; }
        public Rectangle BoundingRectangle
        {
            get
            {
                int left = (int)Math.Round(Position.X - animatedSprite.Origin.X) + localBounds.X;
                int top = (int)Math.Round(Position.Y - animatedSprite.Origin.Y) + localBounds.Y;

                return new Rectangle(left, top, localBounds.Width, localBounds.Height);
            }
        }

        public abstract void Draw(SpriteBatch spriteBatch);
        public abstract void Update(GameTime gameTime);
        public abstract void LoadContent(ContentManager contentManager);
        public abstract void CheckCollisionWithPlayer(Player player);
        protected void DrawBorder(Texture2D pixel, Rectangle rectangleToDraw, int thicknessOfBorder, Color borderColor, SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(pixel, new Rectangle(rectangleToDraw.X, rectangleToDraw.Y, rectangleToDraw.Width, thicknessOfBorder), borderColor);
            spriteBatch.Draw(pixel, new Rectangle(rectangleToDraw.X, rectangleToDraw.Y, thicknessOfBorder, rectangleToDraw.Height), borderColor);
            spriteBatch.Draw(pixel, new Rectangle((rectangleToDraw.X + rectangleToDraw.Width - thicknessOfBorder),
                                            rectangleToDraw.Y,
                                            thicknessOfBorder,
                                            rectangleToDraw.Height), borderColor);
            spriteBatch.Draw(pixel, new Rectangle(rectangleToDraw.X,
                                            rectangleToDraw.Y + rectangleToDraw.Height - thicknessOfBorder,
                                            rectangleToDraw.Width,
                                            thicknessOfBorder), borderColor);
        }

        
        void IDebugble.DrawBorder(Texture2D pixel, Rectangle rectangleToDraw, int thicknessOfBorder, Color borderColor, SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(pixel, new Rectangle(rectangleToDraw.X, rectangleToDraw.Y, rectangleToDraw.Width, thicknessOfBorder), borderColor);

            // Draw left line
            spriteBatch.Draw(pixel, new Rectangle(rectangleToDraw.X, rectangleToDraw.Y, thicknessOfBorder, rectangleToDraw.Height), borderColor);

            // Draw right line
            spriteBatch.Draw(pixel, new Rectangle((rectangleToDraw.X + rectangleToDraw.Width - thicknessOfBorder),
                                            rectangleToDraw.Y,
                                            thicknessOfBorder,
                                            rectangleToDraw.Height), borderColor);
            // Draw bottom line
            spriteBatch.Draw(pixel, new Rectangle(rectangleToDraw.X,
                                            rectangleToDraw.Y + rectangleToDraw.Height - thicknessOfBorder,
                                            rectangleToDraw.Width,
                                            thicknessOfBorder), borderColor);
        }

        public int Damage { get; set; }
        public bool IsDebug { get; set ; }
        public GraphicsDevice GraphicsDevice { get ; set; }

        private ContentManager contentManager;
    }
}
