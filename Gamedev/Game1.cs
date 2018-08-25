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
        private ScoreUI score;
        private HealthUI healthUI;
        private FirePowerUI firePowerUI;
        private IntroSeq introseq = new IntroSeq();
        private bool isDebug;
        public Game1()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            graphics.PreferredBackBufferHeight = 1080;
            graphics.PreferredBackBufferWidth = 1920;
            graphics.GraphicsProfile = GraphicsProfile.HiDef;
            //graphics.ToggleFullScreen();
            
        }

        protected override void Initialize()
        {
            base.Initialize();

            introseq.LoadContent(Content);

            var viewportAdapter = new BoxingViewportAdapter(Window, GraphicsDevice, 1920, 1080);
            camera = new Camera2D(viewportAdapter);
            score = new ScoreUI(Content, camera);
            healthUI = new HealthUI(Content, camera);
            firePowerUI = new FirePowerUI(Content, camera);
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

            if (introseq.PlayVideo && isDebug)
            {
                introseq.Update(gameTime);
            }
            else
            {
                level1.Update(gameTime);
                healthUI.Lives = level1.GetPlayer().Lives;
                healthUI.Update(gameTime);
                firePowerUI.Update(gameTime);
            }
            

            base.Update(gameTime);
        }

   
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);
            var transformMatrix = camera.GetViewMatrix();
            spriteBatch.Begin(transformMatrix: transformMatrix);
            if (introseq.PlayVideo && isDebug )
            {
                introseq.Draw(spriteBatch);
            }
            else
            {
                FollowPlayer(level1, camera);
                level1.Draw(spriteBatch);
                score.Draw( spriteBatch);
                healthUI.Draw( spriteBatch);
                firePowerUI.Draw(spriteBatch);
            }



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
