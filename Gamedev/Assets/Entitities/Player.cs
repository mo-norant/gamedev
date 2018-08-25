using System;
using System.Collections.Generic;
using System.Linq;
using Gamedev.Assets.Entitities;
using Gamedev.Assets.Entitities.Enemies;
using Gamedev.Assets.Helpers;
using Gamedev.Assets.Levels;
using Gamedev.Assets.UI;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using MonoGame.Extended;
using Unity;

namespace Gamedev.Assets
{

    public enum State
    {
        STANDING, SHOOTING, RUNNING, JUMPING, TELEPORTING, DIE
    }

    public enum Direction
    {
        RIGHT, LEFT
    }

    public enum BulletKind
    {
        MAIN = 20, GRENADE = 100
    }

    public class Player : IPipelineBase
    {



        private AnimatedSprite newIdleAnimation, newRunAnimation, newJumpAnimation, newTeleportingAnimation;

        private SpriteEffects flip = SpriteEffects.None;
        private Direction direction;
        private State state;
        private GraphicsDevice graphicsDevice;
        public List<MainBullet> Bullets { get; set; }
        public ScoreUI score { get; set; }
        private ContentManager contentManager;
        public int FirePower { get; set; }
        private const int FirePowerInitWait = 800;


        private const float MoveAcceleration = 80000.0f;
        private const float MaxMoveSpeed = 2000.0f;
        private const float GroundDragFactor = 0.58f;
        private const float AirDragFactor = 0.65f;
        private const float MaxJumpTime = 1f;
        private const float JumpLaunchVelocity = -4000.0f;
        private const float GravityAcceleration = 3500.0f;
        private const float MaxFallSpeed = 1000.0f;
        private const float JumpControlPower = 0.14f;
        public int Lives { get; set; }

        public Level Level { get; set; }

        public bool IsAlive { get; set; }
        

        // Physics state
        public Vector2 Position
        {
            get { return position; }
            set { position = value; }
        }
        Vector2 position;

        private float previousBottom;

        public Vector2 Velocity
        {
            get { return velocity; }
            set { velocity = value; }
        }

        Vector2 velocity;





        /// <summary>
        /// Gets whether or not the player's feet are on the ground.
        /// </summary>
        public bool IsOnGround
        {
            get { return isOnGround; }
        }
        bool isOnGround;

        /// <summary>
        /// Current user movement input.
        /// </summary>
        private float movement;

        // Jumping state
        private bool isJumping;
        private bool wasJumping;
        private float jumpTime;
        private bool isShooting;

        private Rectangle localBounds;
        /// <summary>
        /// Gets a rectangle which bounds this player in world space.
        /// </summary>
        public Rectangle BoundingRectangle
        {
            get
            {
                int left = (int)Math.Round(Position.X - newIdleAnimation.Origin.X) + localBounds.X;
                int top = (int)Math.Round(Position.Y - newIdleAnimation.Origin.Y) + localBounds.Y;

                return new Rectangle(left, top, localBounds.Width, localBounds.Height);
            }
        }

        /// <summary>
        /// Constructors a new player.
        /// </summary>
        public Player(Level level, Vector2 position, ContentManager contentManager, int Lives, ScoreUI score, GraphicsDevice graphicsDevice)
        {
            Level = level;
            this.contentManager = contentManager;
            this.Lives = Lives;
            this.score = score;
            Bullets = new List<MainBullet>();

            FirePower = 1;

            //TESTING:

            LoadContent();

            Reset(position);
        }

        /// <summary>
        /// Loads the player sprite sheet and sounds.
        /// </summary>
        public void LoadContent()
        {
       

            newIdleAnimation = new AnimatedSprite(contentManager.Load<Texture2D>("Player/idle"), 1, 3, true);
            newRunAnimation = new AnimatedSprite(contentManager.Load<Texture2D>("Player/Run"), 1, 9, true);
            newJumpAnimation = new AnimatedSprite(contentManager.Load<Texture2D>("Player/jumping"), 1, 9, false);
            newTeleportingAnimation = new AnimatedSprite(contentManager.Load<Texture2D>("Player/teleport"), 1, 9, false);
            localBounds = new Rectangle();



            int width = (int)(newIdleAnimation.FrameWidth * 0.4);
            int left = (newIdleAnimation.FrameWidth - width) / 2;
            int height = (int)(newIdleAnimation.FrameWidth * 0.8);
            int top = newIdleAnimation.FrameHeight - height;
            localBounds = new Rectangle(left, top, width, height);

        }

        public void Reset(Vector2 position)
        {
            Position = position;
            Velocity = Vector2.Zero;
            IsAlive = true;
            state = State.STANDING;
        }

        public void Update(GameTime gameTime)
        {
            GetInput(gameTime);
            ApplyPhysics(gameTime);
            RemoveOutOfBoundBullets();
            foreach (var b in Bullets)
            {
                b.Update(gameTime);
                
            }


           

            if (IsAlive && IsOnGround)
            {
                if (Math.Abs(Velocity.X) - 0.02f > 0)
                {
                    state = State.RUNNING;
                }
                else
                {
                    state = State.STANDING;
                }
            }

            if (isFalledOf())
            {
                IsAlive = false;
                state = State.DIE;
            }

            // Clear input.
            movement = 0.0f;
            isJumping = false;
        }

        private bool isFalledOf()
        {

            if (Position.Y > 2000)
            {
                IsAlive = false;
                return true;
            }
            return false;
        }

        private void GetInput(GameTime gameTime)
        {
            KeyboardState keyboardState = Keyboard.GetState();
            if (Math.Abs(movement) < 0.5f)
                movement = 0.0f;

            if (
                keyboardState.IsKeyDown(Keys.Left) ||
                keyboardState.IsKeyDown(Keys.A))
            {
                direction = Direction.LEFT;
                state = State.RUNNING;
                movement = -1.0f;

            }


            else if (
               keyboardState.IsKeyDown(Keys.Right) ||
               keyboardState.IsKeyDown(Keys.D))
            {
                direction = Direction.RIGHT;
                state = State.RUNNING;

                movement = 1.0f;
            }




            if (keyboardState.IsKeyDown(Keys.Space))
            {
                
                state = State.SHOOTING;
                Shoot(BulletKind.MAIN, gameTime);
            }
            if (!isJumping)
            {
                state = State.JUMPING;
                isJumping =
                keyboardState.IsKeyDown(Keys.Up) ||
                keyboardState.IsKeyDown(Keys.W);
            }
            
        }

        private void RemoveOutOfBoundBullets()
        {
            //verwijder alle kogels die x absoluut van initpos verwijderd zijn per richting
            Bullets.RemoveAll(i => i.InitPosition.X + 1920 < i.Position.X);
            Bullets.RemoveAll(i => i.InitPosition.X - 1920 > i.Position.X);
        }

        public void ApplyPhysics(GameTime gameTime)
        {
            float elapsed = (float)gameTime.ElapsedGameTime.TotalSeconds;

            Vector2 previousPosition = Position;
            velocity.X += movement * MoveAcceleration * elapsed;
            velocity.Y = MathHelper.Clamp(Velocity.Y + GravityAcceleration * elapsed, -MaxFallSpeed, MaxFallSpeed);

            velocity.Y = DoJump(Velocity.Y, gameTime);

            if (IsOnGround)
                velocity.X *= GroundDragFactor;
            else
                velocity.X *= AirDragFactor;

            velocity.X = MathHelper.Clamp(Velocity.X, -MaxMoveSpeed, MaxMoveSpeed);

            Position += Velocity * elapsed;
            Position = new Vector2((float)Math.Round(Position.X), (float)Math.Round(Position.Y));

            HandleCollisions();

            if (Position.X == previousPosition.X)
                velocity.X = 0;

            if (Position.Y == previousPosition.Y)
                velocity.Y = 0;
        }

        private TimeSpan timeprev;
        private void Shoot(BulletKind bulletKind, GameTime gameTime)
        {
            if(gameTime.TotalGameTime > timeprev + TimeSpan.FromMilliseconds(FirePowerInitWait / FirePower))
            {
                if (bulletKind == BulletKind.MAIN)
                {
                    MainBullet bullet = new MainBullet(Position, (int)BulletKind.MAIN, contentManager, direction, graphicsDevice);
                    Bullets.Add(bullet);
                    
                    bullet.ShotSound.Play();

                }
                timeprev = gameTime.TotalGameTime;

            }



        }

        private float DoJump(float velocityY, GameTime gameTime)
        {
            if (isJumping)
            {
                // Begin or continue a jump
                if ((!wasJumping && IsOnGround) || jumpTime > 0.0f)
                {
                    state = State.JUMPING;
                    jumpTime += (float)gameTime.ElapsedGameTime.TotalSeconds;
                }

                // If we are in the ascent of the jump
                if (0.0f < jumpTime && jumpTime <= MaxJumpTime)
                {
                    // Fully override the vertical velocity with a power curve that gives players more control over the top of the jump
                    velocityY = JumpLaunchVelocity * (1.0f - (float)Math.Pow(jumpTime / MaxJumpTime, JumpControlPower));
                }
                else
                {
                    // Reached the apex of the jump
                    jumpTime = 0.0f;
                }
            }
            else
            {
                // Continues not jumping or cancels a jump in progress
                jumpTime = 0.0f;
            }
            wasJumping = isJumping;

            return velocityY;
        }




        private void HandleCollisions()
        {
            // Get the player's bounding rectangle and find neighboring tiles.
            Rectangle bounds = BoundingRectangle;
            int leftTile = (int)Math.Floor((float)bounds.Left / Tile.Width);
            int rightTile = (int)Math.Ceiling(((float)bounds.Right / Tile.Width)) - 1;
            int topTile = (int)Math.Floor((float)bounds.Top / Tile.Height);
            int bottomTile = (int)Math.Ceiling(((float)bounds.Bottom / Tile.Height)) - 1;

            // Reset flag to search for ground collision.
            isOnGround = false;

            // For each potentially colliding tile,
            for (int y = topTile; y <= bottomTile; ++y)
            {
                for (int x = leftTile; x <= rightTile; ++x)
                {
                    // If this tile is collidable,
                    TileCollision collision = Level.GetCollision(x, y);
                    if (collision != TileCollision.Passable)
                    {
                        // Determine collision depth (with direction) and magnitude.
                        Rectangle tileBounds = Level.GetBounds(x, y);
                        Vector2 depth = Helpers.RectangleExtensions.GetIntersectionDepth(bounds, tileBounds);
                        if (depth != Vector2.Zero)
                        {
                            float absDepthX = Math.Abs(depth.X);
                            float absDepthY = Math.Abs(depth.Y);

                            if (absDepthY < absDepthX || collision == TileCollision.Platform)
                            {
                                if (previousBottom <= tileBounds.Top)
                                    isOnGround = true;

                                if (collision == TileCollision.Impassable || IsOnGround)
                                {
                                    Position = new Vector2(Position.X, Position.Y + depth.Y);
                                    bounds = BoundingRectangle;
                                }
                            }
                            else if (collision == TileCollision.Impassable)
                            {
                                Position = new Vector2(Position.X + depth.X, Position.Y);

                                bounds = BoundingRectangle;
                            }
                        }
                    }
                }
            }

            previousBottom = bounds.Bottom;
        }

        public void OnKilled(Enemy killedBy)
        {
            IsAlive = false;
            state = State.DIE;
        }

        /// <summary>
        /// Called when this player reaches the level's exit.
        /// </summary>
        public void OnReachedExit()
        {
            state = State.TELEPORTING;
        }

        /// <summary>
        /// Draws the animated player.
        /// </summary>
        public void Draw(SpriteBatch spriteBatch)
        {
            // Flip the sprite to face the way we are moving.
            if (Velocity.X > 0)
                flip = SpriteEffects.FlipHorizontally;
            else if (Velocity.X < 0)
                flip = SpriteEffects.None;

            Console.WriteLine("Position: " + Position.ToString());

            if(state == State.RUNNING)
            {
                newRunAnimation.Draw(spriteBatch, Position, flip);
            }
            else if(state == State.JUMPING)
            {
                newJumpAnimation.Draw(spriteBatch, Position, flip);
            }
            else if(state == State.TELEPORTING)
            {
                newTeleportingAnimation.Draw(spriteBatch, Position, flip);

            }
            else
            {
                newIdleAnimation.Draw(spriteBatch, Position, flip);

            }

            foreach (var bullet in Bullets)
            {
                bullet.Draw(spriteBatch);
            }

        }

        public void LoadContent(ContentManager contentManager)
        {
            throw new NotImplementedException();
        }
    }
}
