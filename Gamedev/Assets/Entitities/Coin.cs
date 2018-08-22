using Gamedev.Assets.Helpers;
using Gamedev.Assets.Levels;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System;


namespace Gamedev.Assets.Players
{
    public class Coin
    {
        private Texture2D texture;
        private Vector2 origin;
        private ContentManager contentManager;
        public const int PointValue = 30;
        public readonly Color Color = Color.Yellow;

        // The coin is animated from a base position along the Y axis.
        private Vector2 basePosition;
        private float bounce;

        public Level Level
        {
            get { return level; }
        }
        Level level;

        /// <summary>
        /// Gets the current position of this coin in world space.
        /// </summary>
        public Vector2 Position
        {
            get
            {
                return basePosition + new Vector2(0.0f, bounce);
            }
        }

        /// <summary>
        /// Gets a circle which bounds this coin in world space.
        /// </summary>
        public Circle BoundingCircle
        {
            get
            {
                return new Circle(Position, Tile.Width / 3.0f);
            }
        }

        /// <summary>
        /// Constructs a new coin.
        /// </summary>
        public Coin(Level level, Vector2 position, ContentManager contentManager)
        {
            this.level = level;
            this.basePosition = position;
            this.contentManager = contentManager;
            LoadContent();
        }

        /// <summary>
        /// Loads the coin texture and collected sound.
        /// </summary>
        public void LoadContent()
        {
            texture = contentManager.Load<Texture2D>("Other/coin");
            origin = new Vector2(texture.Width / 2.0f, texture.Height / 2.0f);
        }

        /// <summary>
        /// Bounces up and down in the air to entice players to collect them.
        /// </summary>
        public void Update(GameTime gameTime)
        {
            // Bounce control constants
            const float BounceHeight = 0.18f;
            const float BounceRate = 3.0f;
            const float BounceSync = -0.75f;

            // Bounce along a sine curve over time.
            // Include the X coordinate so that neighboring coins bounce in a nice wave pattern.            
            double t = gameTime.TotalGameTime.TotalSeconds * BounceRate + Position.X * BounceSync;
            bounce = (float)Math.Sin(t) * BounceHeight * texture.Height;
        }



        /// <summary>
        /// Draws a coin in the appropriate color.
        /// </summary>
        public void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(texture, Position, null, Color, 0.0f, origin, 1.0f, SpriteEffects.None, 0.0f);
        }
    }
}
