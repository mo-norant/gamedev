using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Gamedev.Assets.Helpers;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace Gamedev.Assets.Entitities.Bullets
{
    public class TrumpBullet : BulletBase
    {
        bool isRight;
        Player player;
        double angle;
        Vector2 spritePosition, spriteSpeed;
        Vector2 speed = new Vector2(7, 0);
        SpriteEffects spriteEffect;
        int framecounts;
        Rectangle closeSpace;
        public TrumpBullet(Vector2 position, int damage, ContentManager contentManager, Player player, GraphicsDevice graphicsDevice, bool isDebug) : base(graphicsDevice)
        {
            Position = position;
            InitPosition = position;
            Damage = damage;
            this.player = player;
            IsDebug = isDebug;
            //verhoogkogels

            Position += new Vector2(0, -75);

            isRight = Position.X > player.Position.X;
            InitPosition = player.Position;
            LoadContent(contentManager);

            closeSpace = new Rectangle((int)Position.X - BulletTexture.Width, (int)Position.Y - BulletTexture.Height, BulletTexture.Width, BulletTexture.Height);
            animatedSprite = new AnimatedSprite(BulletTexture, 1, 1 , true);


        }



        public override void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(BulletTexture, Position, Color.White);
            if (IsDebug)
            {

                Texture2D pixel;

                pixel = new Texture2D(GraphicsDevice, 1, 1, false, SurfaceFormat.Color);
                pixel.SetData(new[] { Color.White });
                DrawBorder(pixel, closeSpace, 5, Color.Purple, spriteBatch);


            }
        }

        public override void CheckCollisionWithPlayer(Player player)
        {
            if (player.BoundingRectangle.Intersects(BoundingRectangle) && IsNotEffective == false)
            {
                player.Lives--;
                IsNotEffective = true;
            }
        }

        public override void LoadContent(ContentManager contentManager)
        {
            BulletTexture = contentManager.Load<Texture2D>("Bullets/trumpbullet");
        }

        private void FireShot(GameTime gameTime)
        {
            closeSpace = new Rectangle((int)Position.X, (int)Position.Y, BulletTexture.Width, BulletTexture.Height);

            var t = player.Position;

            t.X -= 50;

            var movementChange = t - Position;


            if (movementChange != Vector2.Zero)
                movementChange.Normalize();


            Position += movementChange;

            if (Position.X < player.Position.X)
            {
                Position += speed;
            }
            else
            {
                Position -= speed;

            }
        }
        public override void Update(GameTime gameTime)
        {
            framecounts++;
            if (framecounts > 125)
            {
                IsNotEffective = true;
            }

            FireShot(gameTime);

        }
    }
}
