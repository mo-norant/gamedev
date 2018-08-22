using Gamedev.Assets;
using Gamedev.Assets.Levels;
using Gamedev.Assets.UI;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using MonoGame.Extended;
using MonoGame.Extended.ViewportAdapters;

namespace Gamedev
{

    public class Game1 : Game
    {
        private GraphicsDeviceManager graphics;
        private SpriteBatch spriteBatch;
        private Level level1;
        private Camera2D camera;
        private Score score;
        private HealthUI healthUI;

        public Game1()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            graphics.PreferredBackBufferHeight = 1080;
            graphics.PreferredBackBufferWidth = 1920;
            graphics.GraphicsProfile = GraphicsProfile.HiDef;

            
        }

        protected override void Initialize()
        {
            base.Initialize();
            var viewportAdapter = new BoxingViewportAdapter(Window, GraphicsDevice, 1920, 1080);
            camera = new Camera2D(viewportAdapter);
            score = new Score(Content, camera);
            healthUI = new HealthUI(Content, camera);
            level1 = new Level(Content, GraphicsDevice, 0, score);

        }


        protected override void LoadContent()
        {

            spriteBatch = new SpriteBatch(GraphicsDevice);


        }


        protected override void Update(GameTime gameTime)
        {
            if (Keyboard.GetState().IsKeyDown(Keys.Escape))
            {
                Exit();

            }
            
            
            level1.Update(gameTime);
            healthUI.Update(gameTime, level1.GetPlayer().Lives);
            base.Update(gameTime);
        }

   
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);
            var transformMatrix = camera.GetViewMatrix();
            spriteBatch.Begin(transformMatrix: transformMatrix);
            FollowPlayer(level1, camera);
            level1.Draw(gameTime, spriteBatch);
            score.Draw(gameTime, spriteBatch);
            healthUI.Draw(gameTime, spriteBatch);
            spriteBatch.End();
            base.Draw(gameTime);
        }

        private void FollowPlayer(Level level, Camera2D camera)
        {
            Vector2 playerPosition = level1.GetPlayer().Position;
            
            if(playerPosition.X >  1080 && playerPosition.X < 11800)
            {
                
                camera.Position = new Vector2(playerPosition.X - 1080, 0);

            }
           
            
        }
    }
}
