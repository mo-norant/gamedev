using Gamedev.Assets.Helpers;
using Gamedev.Assets.Levels;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Gamedev.Assets.Entitities.Enemies
{
    
    public enum EnemyState
    {
        RUNNING, STANDING, ATTACKING
    }
    public abstract class EnemyBase : IDebugble
    {
        protected ContentManager contentManager;
        protected AnimatedSprite mainSprite;
        protected EnemyState state;
        public Level Level { get; set; }
        public bool IsDead { get; set; }
        public Vector2 Position { get; set; }
        protected Rectangle localBounds { get; set; }
        public Rectangle BoundingRectangle
        {
            get
            {
                int left = (int)Math.Round(Position.X - mainSprite.Origin.X) + localBounds.X;
                int top = (int)Math.Round(Position.Y - mainSprite.Origin.Y) + localBounds.Y;

                return new Rectangle(left, top, localBounds.Width, localBounds.Height);
            }
        }

        public bool IsDebug { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        public GraphicsDevice GraphicsDevice { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }

        protected FaceDirection direction = FaceDirection.Left;
      

        public EnemyBase(Level level, Vector2 position, ContentManager contentManager)
        {
            Level = level;
            Position = position;
            this.contentManager = contentManager;

        }

        public abstract void LoadContent();
        public abstract void Update(GameTime gameTime);
        public abstract void Draw(SpriteBatch spriteBatch);
        public abstract void BeeingShot(int damage, BulletBase bulletBase);

        protected void DrawBorder(Texture2D pixel, Rectangle rectangleToDraw, int thicknessOfBorder, Color borderColor, SpriteBatch spriteBatch)
        {
            // Draw top line
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

        void IDebugble.DrawBorder(Texture2D pixel, Rectangle rectangleToDraw, int thicknessOfBorder, Color borderColor, SpriteBatch spriteBatch)
        {
            throw new NotImplementedException();
        }
    }
}
