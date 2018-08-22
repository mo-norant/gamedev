using System;
using System.Collections.Generic;
using System.IO;
using Gamedev.Assets.Entitities;
using Gamedev.Assets.Helpers;
using Gamedev.Assets.Players;
using Gamedev.Assets.UI;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;


namespace Gamedev.Assets.Levels
{

    //inspiratie: https://github.com/kg/PlatformerStarterKit/blob/master/Level.cs
    public class Level
    {
        private const bool isDebug = true;
        private GraphicsDevice graphicsDevice;
        private ContentManager contentManager;
        private Tile[,] tiles;
        private List<Coin> coins = new List<Coin>();
        private List<Enemy> enemies = new List<Enemy>();
        private List<Texture2D> backgrounds = new List<Texture2D>();
        private const int EntityLayer = 2;
        private Player player;
        private Score score;
        private Point startPoints;
        private int levelId;
        private Point exit = InvalidPosition;
        private Vector2 start;
        private SpriteFont debugfont, mainfont;

        public Player GetPlayer()
        {
            return player;
        }

        public bool ReachedExit
        {
            get { return reachedExit; }
        }
        bool reachedExit;

        private static readonly Point InvalidPosition = new Point(-1, -1);
        public int Width
        {
            get { return tiles.GetLength(0); }
        }
        public int Height
        {
            get { return tiles.GetLength(1); }
        }

        public Level(ContentManager contentManager, GraphicsDevice graphicsDevice, int levelId)
        {
            this.contentManager = contentManager;
            this.graphicsDevice = graphicsDevice;
            this.levelId = levelId;

            LoadContent();
            LoadBackground(levelId);
            LoadTiles(levelId);

            score = new Score(contentManager);
            


        }

        

        private void LoadBackground(int levelId)
        {
            Texture2D background = contentManager.Load<Texture2D>($"Levels/background{levelId.ToString()}");
            backgrounds.Add(background);
        }

        private void DrawBackground(SpriteBatch spriteBatch)
        {
            foreach (var sprite in backgrounds)
            {
                spriteBatch.Draw(sprite, Vector2.Zero, Color.White);
            }
        }

        private void LoadTiles(int levelId)
        {
            // Load the level and ensure all of the lines are the same length.
            int width;
            List<string> lines = new List<string>();
            using (StreamReader reader = new StreamReader("Content/Levels/" + levelId.ToString() + ".txt"))
            {
                string line = reader.ReadLine();
                width = line.Length;
                while (line != null)
                {
                    lines.Add(line);
                    if (line.Length != width)
                        throw new Exception(String.Format("The length of line {0} is different from all preceeding lines.", lines.Count));
                    line = reader.ReadLine();
                }
            }

            // Allocate the tile grid.
            tiles = new Tile[width, lines.Count];

            // Loop over every tile position,
            for (int y = 0; y < Height; ++y)
            {
                for (int x = 0; x < Width; ++x)
                {
                    // to load each tile.
                    char tileType = lines[y][x];
                    tiles[x, y] = LoadTile(tileType, x, y);
                }
            }

            // Verify that the level has a beginning and an end.
            if (player == null)
                throw new NotSupportedException("A level must have a starting point.");
            if (exit == InvalidPosition)
                throw new NotSupportedException("A level must have an exit.");

        }

        private Tile LoadTile(char tileType, int x, int y)
        {
            switch (tileType)
            {
                // Blank space
                case '.':
                    return new Tile(null, TileCollision.Passable);

                // Exit
                case 'b':
                    return LoadExitTile(x, y);

                case 'x':
                    return LoadInvisibleTile(TileCollision.Impassable);

                // Coin
                case 'C':
                    return LoadCoinTile(x, y);

                // Floating platform
                case '-':
                    return LoadTile("platform", TileCollision.Platform);

                // Player 1 start point
                case '1':
                    return LoadStartTile(x, y);
                    
                default:
                    throw new NotSupportedException(String.Format("Unsupported tile type character '{0}' at position {1}, {2}.", tileType, x, y));
            }
        }

        private Tile LoadStartTile(int x, int y)
        {
            if (player != null)
                throw new NotSupportedException("A level may only have one starting point.");

            start = RectangleExtensions.GetBottomCenter(GetBounds(x, y));
            player = new Player(this, start, contentManager, 100);

            return  LoadTile("start", TileCollision.Passable);
        }

        private Tile LoadInvisibleTile( TileCollision collision)
        {
            return new Tile(contentManager.Load<Texture2D>("Tiles/invisibleplatform"), collision);
        }

        private Tile LoadTile(string name, TileCollision collision)
        {
            return new Tile(contentManager.Load<Texture2D>("Tiles/" + name), collision);
        }

        /// <summary>
        /// Remembers the location of the level's exit.
        /// </summary>
        private Tile LoadExitTile(int x, int y)
        {
            if (exit != InvalidPosition)
                throw new NotSupportedException("A level may only have one exit.");

            exit = GetBounds(x, y).Center;

            return LoadTile("exit", TileCollision.Passable);
        }

        /// <summary>
        /// Instantiates an enemy and puts him in the level.
        /// </summary>
        private Tile LoadEnemyTile(int x, int y, string spriteSet)
        {
            Vector2 position = RectangleExtensions.GetBottomCenter(GetBounds(x, y));
            enemies.Add(new Enemy(this, position, spriteSet, contentManager));

            return new Tile(null, TileCollision.Passable);
        }

        /// <summary>
        /// Instantiates a gem and puts it in the level.
        /// </summary>
        private Tile LoadCoinTile(int x, int y)
        {
            Point position = GetBounds(x, y).Center;
            coins.Add(new Coin(this, new Vector2(position.X, position.Y), contentManager));

            return new Tile(null, TileCollision.Passable);
        }

        public TileCollision GetCollision(int x, int y)
        {
            // Prevent escaping past the level ends.
            if (x < 0 || x >= Width)
                return TileCollision.Impassable;
            // Allow jumping past the level top and falling through the bottom.
            if (y < 0 || y >= Height)
                return TileCollision.Passable;

            return tiles[x, y].Collision;
        }

        public Rectangle GetBounds(int x, int y)
        {
            return new Rectangle(x * Tile.Width, y * Tile.Height, Tile.Width, Tile.Height);
        }



        private void LoadContent()
        {
            if (isDebug)
            {
                debugfont = contentManager.Load<SpriteFont>("Fonts/BebasNeue-Regular");
            }
            mainfont = contentManager.Load<SpriteFont>("Fonts/Gameplay");
        }

        public void Update(GameTime gameTime)
        {
            // Pause while the player is dead or time is expired.
            if (!player.IsAlive)
            {
                // Still want to perform physics on the player.
                player.ApplyPhysics(gameTime);
            }
            else if (ReachedExit)
            {
                
            }
            else
            {

                player.Update(gameTime);

                UpdateCoins(gameTime);
                score.Update(gameTime);

                // Falling off the bottom of the level kills the player.
                if (player.BoundingRectangle.Top >= Height * Tile.Height)
                    OnPlayerKilled(null);

               // UpdateEnemies(gameTime);

               
                if (player.IsAlive &&
                    player.IsOnGround &&
                    player.BoundingRectangle.Contains(exit))
                {
                    OnExitReached();
                }
            }


  
        }

        private void OnExitReached()
        {
            player.OnReachedExit();
            reachedExit = true;
        }
        private void OnPlayerKilled(Enemy killedBy)
        {
           player.OnKilled(killedBy);
        }

        /// <summary>
        /// Animates each gem and checks to allows the player to collect them.
        /// </summary>
        private void UpdateCoins(GameTime gameTime)
        {
            for (int i = 0; i < coins.Count; ++i)
            {
                Coin coin = coins[i];

                coin.Update(gameTime);

                if (coin.BoundingCircle.Intersects(player.BoundingRectangle))
                {
                    coins.RemoveAt(i--);
                    OnCoinCollected(coin, player);
                }
            }
        }

        private void OnCoinCollected(Coin coin, Player collectedBy)
        {
            
        }



        public void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            // for (int i = 0; i <= EntityLayer; ++i)
            //    spriteBatch.Draw(layers[i], Vector2.Zero, Color.White);
            DrawBackground(spriteBatch);
            DrawTiles(spriteBatch);
            

            foreach (Coin coin in coins)
                coin.Draw(gameTime, spriteBatch);

            player.Draw(gameTime, spriteBatch);

            foreach (Enemy enemy in enemies)
                enemy.Draw(gameTime, spriteBatch);

            //   for (int i = EntityLayer + 1; i < layers.Length; ++i)
            //  spriteBatch.Draw(layers[i], Vector2.Zero, Color.White);
            score.Draw(gameTime, spriteBatch);
        }

        /// <summary>
        /// Draws each tile in the level.
        /// </summary>
        private void DrawTiles(SpriteBatch spriteBatch)
        {
            // For each tile position
            for (int y = 0; y < Height; ++y)
            {
                for (int x = 0; x < Width; ++x)
                {
                    // If there is a visible tile in that position
                    Texture2D texture = tiles[x, y].Texture;
                    if (texture != null)
                    {
                        // Draw it in screen space.
                        Vector2 position = new Vector2(x, y) * Tile.Size;
                        spriteBatch.Draw(texture, position, Color.White);
                        if (isDebug)
                        {
                            spriteBatch.DrawString(debugfont, $"{y.ToString()} - {x.ToString()}", position, Color.White);
                        }

                    }
                }
            }
        }

    }
}
