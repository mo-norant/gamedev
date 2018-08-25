using Gamedev.Assets.Entitities.Bullets;
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

    
    public class TrumpEnemy : EnemyBase
    {

        public bool IsDebug { get; set; }
        public int Health { get; set; }
        private AnimatedSprite standingSprite, runningSprite;
        private GraphicsDevice graphicsDevice;
        private float waitTime;
        private const float MoveSpeed = 128.0f;
        private const float MaxWaitTime = 0.5f;
        private Player player;
        public List<BulletBase> bullets { get; set; }
        private Rectangle closeSpace;

        public TrumpEnemy(GraphicsDevice graphicsDevice, Level level, Vector2 position, ContentManager contentManager, int healthinit, Player player, bool isDebug):base(level, position, contentManager)
        {
            IsDebug = isDebug;
            this.player = player;
            this.graphicsDevice = graphicsDevice;
            Health = healthinit;
            LoadContent();
            bullets = new List<BulletBase>();
        }

      

        public override void LoadContent()
        {
            standingSprite = new AnimatedSprite(contentManager.Load<Texture2D>("Enemies/Trumps/standing"), 1, 1, true);
            runningSprite = new AnimatedSprite(contentManager.Load<Texture2D>("Enemies/Trumps/running"), 1, 6, true);
            mainSprite = runningSprite;


             int width = (int)(runningSprite.FrameWidth * 0.35);
             int left = (runningSprite.FrameWidth - width) / 2;
              int height = (int)(runningSprite.FrameWidth * 0.7);
             int top = runningSprite.FrameHeight - height;
             localBounds = new Rectangle(left, top, width, height);

           

        }

        private void CheckCollisionWithEachBullet()
        {
            foreach (var b in bullets)
            {
                b.CheckCollisionWithPlayer(player);
            }
        }

        private void RemoveNotEffectiveBullets()
        {
            bullets.RemoveAll(i => i.IsNotEffective);
        }

        int wait;
        public override void Update(GameTime gameTime)
        {
            float elapsed = (float)gameTime.ElapsedGameTime.TotalSeconds;

            float posX = Position.X + localBounds.Width / 2 * (int)direction;
            int tileX = (int)Math.Floor(posX / Tile.Width) - (int)direction;
            int tileY = (int)Math.Floor(Position.Y / Tile.Height);

            if (waitTime > 0)
            {
                waitTime = Math.Max(0.0f, waitTime - (float)gameTime.ElapsedGameTime.TotalSeconds);
                if (waitTime <= 0.0f)
                {
                    direction = (FaceDirection)(-(int)direction);
                }
            }
            else
            {
                if (Level.GetCollision(tileX + (int)direction, tileY - 1) == TileCollision.Impassable ||
                    Level.GetCollision(tileX + (int)direction, tileY) == TileCollision.Passable)
                {
                    waitTime = MaxWaitTime;
                }
                else
                {
                    Vector2 velocity = new Vector2((int)direction * MoveSpeed * elapsed, 0.0f);
                    Position = Position + velocity;
                }
            }

            if (player != null )
            {
                if (PlayerIsClose(player))
                {
                    if(wait > 100)
                    {
                        Attack();
                        wait = 0;
                    }
                }
                wait++;
            }


            UpdateBullets(gameTime);
            closeSpace = new Rectangle((int)Position.X - runningSprite.FrameWidth - 500, (int)Position.Y - runningSprite.FrameHeight - 500, runningSprite.FrameWidth + 1000, runningSprite.FrameHeight + 1000);
            CheckCollisionWithEachBullet();

        }

        private void Attack()
        {
            state = EnemyState.ATTACKING;

            TrumpBullet b = new TrumpBullet(Position, 100, contentManager, player, graphicsDevice, IsDebug);
            bullets.Add(b);

        }

        public override void BeeingShot(int damage, BulletBase bulletBase)
        {
            Health -= damage;
            if (Health < 0)
            {
                IsDead = true;

            }

            bulletBase.IsNotEffective = true;

            if(direction == FaceDirection.Left)
            {
                Position += new Vector2(20, 0);
            }
            else
            {
                Position -= new Vector2(20, 0);

            }

        }

        public bool PlayerIsClose(Player player)
        {


            if (player.BoundingRectangle.Intersects(closeSpace))
            {
                return true;
            }


            return false;
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            if (IsDebug)
            {

                Texture2D pixel;

                pixel = new Texture2D(graphicsDevice, 1, 1, false, SurfaceFormat.Color);
                pixel.SetData(new[] { Color.White });
                //Draw close rectangle
                DrawBorder(pixel, closeSpace, 5, Color.Red, spriteBatch);
                //Draw remove health rectangle
                DrawBorder(pixel, BoundingRectangle, 5, Color.Blue, spriteBatch);

            }
            SpriteEffects flip = direction > 0 ? SpriteEffects.FlipHorizontally : SpriteEffects.None;
            
            if (!Level.Player.IsAlive ||
              Level.ReachedExit ||
              waitTime > 0)
            {
                state = EnemyState.STANDING;
                standingSprite.Draw(spriteBatch, Position, flip);

            }
            else
            {
                state = EnemyState.RUNNING;
                runningSprite.Draw(spriteBatch, Position, flip);

            }

            DrawBullets(spriteBatch);



        }

        private void DrawBullets(SpriteBatch spriteBatch)
        {
            foreach (var b in bullets)
            {
                b.Draw(spriteBatch);
            }
        }

        private void UpdateBullets(GameTime gameTime)
        {
            foreach (var b in bullets)
            {
                b.Update(gameTime);
            }
            RemoveNotEffectiveBullets();
        }

     
    }
}
