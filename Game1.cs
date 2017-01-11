using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using GLX;
using System;
using System.Diagnostics;

namespace TimePrototype
{
    /// <summary>
    /// This is the main type for your game.
    /// </summary>
    public class Game1 : Game
    {
        const string MainGame = "game1";
        GraphicsDeviceManager graphics;
        World world;
        GameTimeWrapper mainGameTime;
        Circle circle;
        KeyboardState keyboardState;
        KeyboardState previousKeyboardState;

        Wall floor;
        Wall wall;
        Wall wall2;

        public Game1()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
        }

        /// <summary>
        /// Allows the game to perform any initialization it needs to before starting to run.
        /// This is where it can query for any required services and load any non-graphic
        /// related content.  Calling base.Initialize will enumerate through any components
        /// and initialize them as well.
        /// </summary>
        protected override void Initialize()
        {
            previousKeyboardState = Keyboard.GetState();

            base.Initialize();
        }

        /// <summary>
        /// LoadContent will be called once per game and is the place to load
        /// all of your content.
        /// </summary>
        protected override void LoadContent()
        {
            world = new World(graphics);
            mainGameTime = new GameTimeWrapper(MainUpdate, this, 1);
            world.AddGameState(MainGame, graphics);
            world.gameStates[MainGame].AddTime(mainGameTime);
            world.gameStates[MainGame].AddDraw(MainDraw);
            world.ActivateGameState(MainGame);

            circle = new Circle(Content.Load<Texture2D>("circle"));
            circle.position = new Vector2(400);

            floor = new Wall(graphics);
            floor.sprite.drawRect = new Rectangle(0, 0, 500, 25);
            floor.sprite.position = new Vector2(100, 425);

            wall = new Wall(graphics);
            wall.sprite.drawRect = new Rectangle(0, 0, 25, 500);
            wall.sprite.position = new Vector2(500, 0);

            wall2 = new Wall(graphics);
            wall2.sprite.drawRect = new Rectangle(0, 0, 25, 200);
            wall2.sprite.position = new Vector2(400, 100);
        }

        /// <summary>
        /// UnloadContent will be called once per game and is the place to unload
        /// game-specific content.
        /// </summary>
        protected override void UnloadContent()
        {
            // TODO: Unload any non ContentManager content here
        }

        /// <summary>
        /// Allows the game to run logic such as updating the world,
        /// checking for collisions, gathering input, and playing audio.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();

            world.Update(gameTime);

            base.Update(gameTime);
        }

        public void MainUpdate(GameTimeWrapper gameTime)
        {
            world.gameStates[MainGame].UpdateCurrentCamera(gameTime);

            keyboardState = Keyboard.GetState();

            float gravity = 0.1f;

            if (circle.jumpState == Circle.JumpStates.Ground)
            {
                circle.velocity.X *= 0.87f;
            }
            else
            {
                circle.velocity.X *= 0.95f;
            }

            circle.velocity.Y += gravity;

            if (keyboardState.IsKeyDown(Keys.Left))
            {
                circle.velocity.X -= 0.5f;
            }
            else if (keyboardState.IsKeyDown(Keys.Right))
            {
                circle.velocity.X += 0.5f;
            }

            if (IsKeyDownAndsUp(Keys.Space))
            {
                if (circle.jumpState == Circle.JumpStates.Ground)
                {
                    circle.velocity.Y -= 5.0f;
                }
                else if (circle.jumpState == Circle.JumpStates.Wall)
                {
                    circle.velocity.Y -= 5.0f;
                    circle.velocity.X = circle.storedXVelocity * -1;
                }
                circle.jumpState = Circle.JumpStates.Air;
            }

            if (IsKeyDownAndsUp(Keys.LeftControl) || IsKeyDownAndsUp(Keys.LeftShift))
            {
                circle.position += Vector2.Normalize(circle.velocity) * 100;
            }

            circle.velocity.X = MathHelper.Clamp(circle.velocity.X, -5, 5);
            
            circle.Update(gameTime);
            floor.Update(gameTime, circle);
            wall.Update(gameTime, circle);
            wall2.Update(gameTime, circle);

            previousKeyboardState = keyboardState;

            base.Update(gameTime);
        }

        private bool IsKeyDownAndsUp(Keys key)
        {
            return keyboardState.IsKeyDown(key) && previousKeyboardState.IsKeyUp(key);
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);

            world.DrawWorld();

            base.Draw(gameTime);
        }

        public void MainDraw()
        {
            world.BeginDraw();
            world.Draw(circle.Draw);
            world.Draw(floor.Draw);
            world.Draw(wall.Draw);
            world.Draw(wall2.Draw);
            world.EndDraw();
        }
    }
}
