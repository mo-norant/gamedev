using Gamedev.Assets.Entitities.Specials;
using Gamedev.Assets.Helpers;
using Gamedev.Assets.Levels;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System;


namespace Gamedev.Assets.Players
{
    public class Coin : SpecialItemBase, IPipelineBase
    {
        public const int PointValue = 30;
        public readonly Color Color = Color.Yellow;

        private Vector2 position;
        private float bounce;
        private int randomCoinValue;
        private Level level;
        public Vector2 Position
        {
            get
            {
                return position + new Vector2(0.0f, bounce);
            }
        }

        public int GetCoinValue()
        {
            return randomCoinValue;
        }

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
        public Coin(Level level, Vector2 position, ContentManager contentManager) : base(contentManager)
        {
            this.level = level;
            this.position = position;
            randomCoinValue = RandomCoinValue();
            LoadContent(contentManager);
        }

        private int RandomCoinValue()
        {
            Random r = new Random();
            return r.Next(1, 20)*1000;
        }
       
        public override void LoadContent(ContentManager contentManager)
        {
            texture = contentManager.Load<Texture2D>("Other/coin");
            origin = new Vector2(texture.Width / 2.0f, texture.Height / 2.0f);
        }

        public override void Update(GameTime gameTime)
        {
            const float BounceHeight = 0.18f;
            const float BounceRate = 3.0f;
            const float BounceSync = -0.75f;
            double t = gameTime.TotalGameTime.TotalSeconds * BounceRate + Position.X * BounceSync;
            bounce = (float)Math.Sin(t) * BounceHeight * texture.Height;
        }


        public override void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(texture, Position, null, Color, 0.0f, origin, 1.0f, SpriteEffects.None, 0.0f);
        }
       
       
    }
}
