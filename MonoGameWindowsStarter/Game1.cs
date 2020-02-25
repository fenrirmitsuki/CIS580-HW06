using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;

namespace MonoGameWindowsStarter
{
    /// <summary>
    /// This is the main type for your game.
    /// </summary>
    public class Game1 : Game
    {
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;
        Ball ball;
        Player player;
        SpriteFont font;
        int score;

        public Random Random = new Random();

        KeyboardState keyboardState;

        public Game1()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            ball = new Ball(this);
            player = new Player(this);
        }

        /// <summary>
        /// Allows the game to perform any initialization it needs to before starting to run.
        /// This is where it can query for any required services and load any non-graphic
        /// related content.  Calling base.Initialize will enumerate through any components
        /// and initialize them as well.
        /// </summary>
        protected override void Initialize()
        {
            graphics.PreferredBackBufferWidth = 1500;
            graphics.PreferredBackBufferHeight = 900;
            graphics.ApplyChanges();

            ball.Initialize();
            player.Initialize();

            score = 0;

            base.Initialize();
        }

        /// <summary>
        /// LoadContent will be called once per game and is the place to load
        /// all of your content.
        /// </summary>
        protected override void LoadContent()
        {
            // Create a new SpriteBatch, which can be used to draw textures.
            spriteBatch = new SpriteBatch(GraphicsDevice);

            ball.LoadContent(Content);
            player.LoadContent(Content);
            font = Content.Load<SpriteFont>("defaultFont");

            // TODO: use this.Content to load your game content here
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
            keyboardState = Keyboard.GetState();

            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();

            ball.Update(gameTime);
            player.Update(gameTime);

            // TODO: Add your update logic here
            if(player.Bounds.CollidesWith(ball.Bounds))
            {
                if(!((ball.Bounds.Y > player.Bounds.Y + player.Bounds.Height) || (ball.Bounds.Y + ball.Bounds.Radius < player.Bounds.Y)))
                {
                    ball.Velocity.Y *= -1;

                    if(ball.State == GameState.Game)
                    {
                        score++;
                    }
                    
                    int sfx = Random.Next(2);
                    if(sfx == 1)
                    {
                        player.playerBounce01.Play();
                    }
                    else
                    {
                        player.playerBounce02.Play();
                    }

                }
                else if(!((ball.Bounds.X > player.Bounds.X + player.Bounds.Width) || (ball.Bounds.X + ball.Bounds.Radius < player.Bounds.X)))
                {
                    ball.Velocity.X *= -1;
                    if (ball.State == GameState.Game)
                    {
                        score++;
                    }

                    int sfx = Random.Next(1,2);
                    if (sfx == 1)
                    {
                        player.playerBounce01.Play();
                    }
                    else
                    {
                        player.playerBounce02.Play();
                    }
                }
            }

            base.Update(gameTime);
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.DeepSkyBlue);

            var offset = new Vector2(10, 10) - player.Position;
            var tMatrix = Matrix.CreateTranslation(offset.X, offset.Y, 0);

            // TODO: Add your drawing code here
            //spriteBatch.Begin(SpriteSortMode.Deferred, null, null, null, null, null, tMatrix);
            spriteBatch.Begin();
            ball.Draw(spriteBatch);
            player.Draw(spriteBatch);

            spriteBatch.DrawString(font, "Use the arrow keys to move", new Vector2(1,1), Color.White);
            spriteBatch.DrawString(font, "Bounce the ball to earn points", new Vector2(1,19), Color.White);
            spriteBatch.DrawString(font, "Don't let the ball past you", new Vector2(1,38), Color.White);
            spriteBatch.DrawString(font, $"Score: {score}", new Vector2(500, 1), Color.White);
            spriteBatch.End();

            base.Draw(gameTime);
        }
    }
}
