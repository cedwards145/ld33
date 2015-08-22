using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;
using TiledSharp;
using XNAGameLibrary.Input;
using XNAGameLibrary.Pathfinding;
using XNAGameLibrary.Cameras;

namespace LD33
{
    /// <summary>
    /// This is the main type for your game
    /// </summary>
    public class Game1 : Microsoft.Xna.Framework.Game
    {
        public static Game1 gameRef;

        private GraphicsDeviceManager graphics;
        private SpriteBatch spriteBatch;
        Map map;
        Minion playerMinion;

        private Camera camera;

        public Game1()
        {
            graphics = new GraphicsDeviceManager(this);
            graphics.PreferredBackBufferHeight = 1080;
            graphics.PreferredBackBufferWidth = 1920;

            Content.RootDirectory = "Content";
            gameRef = this;

            camera = new Camera();
            camera.Zoom = 1f;
        }

        protected override void Initialize()
        {
            // TODO: Add your initialization logic here
            base.Initialize();

            IsMouseVisible = true;
        }

        protected override void LoadContent()
        {
            // Create a new SpriteBatch, which can be used to draw textures.
            spriteBatch = new SpriteBatch(GraphicsDevice);

            // TODO: use this.Content to load your game content here
            map = new Map(@"content\maps\map1.tmx");
        }

        protected override void Update(GameTime gameTime)
        {
            base.Update(gameTime);
            Input.Update();

            if (Input.IsKeyPressed(Keys.I))
            {
                playerMinion = new Minion();
                map.addMinion(playerMinion);
            }

            updateCamera();

            map.update();                
        }

        private void updateCamera()
        {
            Point moveValue = new Point(0, 0);
            if (Input.IsKeyDown(Keys.W))
                moveValue.Y = -1;
            if (Input.IsKeyDown(Keys.A))
                moveValue.X = -1;
            if (Input.IsKeyDown(Keys.S))
                moveValue.Y = 1;
            if (Input.IsKeyDown(Keys.D))
                moveValue.X = 1;

            Point mousePos = Input.GetMousePosition();

            camera.Move(moveValue);

            int scrollValue = Input.GetMouseScrollValue();
            camera.ZoomAmount(scrollValue);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);


            spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, SamplerState.PointClamp, null, null, null, camera.TransformationMatrix);
            map.draw(spriteBatch);
            spriteBatch.End();

            base.Draw(gameTime);
        }

    }
}
