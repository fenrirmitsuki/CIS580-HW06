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
        Ball ball1;
        Ball ball2;
        Player player;
        SpriteFont font;
        int score;
        BoundingRectangle wallN;
        BoundingRectangle wallS;
        BoundingRectangle wallE;
        BoundingRectangle wallW;
        Texture2D wallText;
        ParticleSystem particleSystem1;
        ParticleSystem particleSystem2;
        ParticleSystem particleSystem3;
        Texture2D particleTexture;

        public Random Random = new Random();

        KeyboardState keyboardState;

        public Game1()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            ball1 = new Ball(this);
            ball2 = new Ball(this);
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

            ball1.Initialize();
            ball2.Initialize();
            player.Initialize();

            score = 0;

            wallN.X = -50;
            wallN.Y = -50;
            wallN.Width = 1850;
            wallN.Height = 50;

            wallS.X = -50;
            wallS.Y = 1000;
            wallS.Width = 1850;
            wallS.Height = 50;

            wallE.X = 1750;
            wallE.Y = 0;
            wallE.Width = 50;
            wallE.Height = 1000;

            wallW.X = -50;
            wallW.Y = 0;
            wallW.Width = 50;
            wallW.Height = 1000;

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

            ball1.LoadContent(Content);
            ball2.LoadContent(Content);
            player.LoadContent(Content);
            font = Content.Load<SpriteFont>("defaultFont");
            wallText = Content.Load<Texture2D>("pixel");

            // TODO: use this.Content to load your game content here
            particleTexture = Content.Load<Texture2D>("pixel");
            particleSystem1 = new ParticleSystem(this.GraphicsDevice, 1000, particleTexture);
            //particleSystem1.Emitter = new Vector2(ball1.Bounds.X, ball1.Bounds.Y);
            particleSystem1.SpawnPerFrame = 4;
            particleSystem1.SpawnParticle = (ref Particle particle) =>
            {
                particle.Position = new Vector2(ball1.Bounds.X, ball1.Bounds.Y);
                particle.Velocity = new Vector2(MathHelper.Lerp(-50, 50, (float)Random.NextDouble()), MathHelper.Lerp(0, 100, (float)Random.NextDouble()));
                particle.Acceleration = 0.1f * new Vector2(0, (float)-Random.NextDouble());
                particle.Color = Color.Red;
                particle.Scale = 3f;
                particle.Life = 4.0f;
            };
            particleSystem1.UpdateParticle = (float deltaT, ref Particle particle) =>
            {
                particle.Velocity += deltaT * particle.Acceleration;
                particle.Position += deltaT * particle.Velocity;
                particle.Scale -= deltaT;
                particle.Life -= deltaT;
            };

            particleSystem2 = new ParticleSystem(this.GraphicsDevice, 5000, particleTexture);
            particleSystem2.SpawnPerFrame = 6;
            particleSystem2.SpawnParticle = (ref Particle particle) =>
            {
                particle.Position = new Vector2(ball2.Bounds.X, ball2.Bounds.Y);
                particle.Velocity = new Vector2(MathHelper.Lerp(-25, 25, (float)Random.NextDouble()), MathHelper.Lerp(0, 100, (float)Random.NextDouble()));
                particle.Acceleration = 0.1f * new Vector2(0, (float)-Random.NextDouble());
                particle.Color = Color.Red;
                particle.Scale = 3f;
                particle.Life = 3.0f;
            };
            particleSystem2.UpdateParticle = (float deltaT, ref Particle particle) =>
            {
                particle.Velocity += (deltaT * particle.Acceleration) * .9f;
                particle.Position += deltaT * -particle.Velocity;
                particle.Scale -= deltaT;
                particle.Life -= deltaT;
            };

            particleSystem3 = new ParticleSystem(this.GraphicsDevice, 2000, particleTexture);
            particleSystem3.SpawnPerFrame = 1;
            particleSystem3.SpawnParticle = (ref Particle particle) =>
            {
                particle.Position = new Vector2(750, -50);
                particle.Velocity = new Vector2(MathHelper.Lerp(-1000, 1000, (float)Random.NextDouble()), MathHelper.Lerp(0, 100, (float)Random.NextDouble()));
                particle.Acceleration = 0.1f * new Vector2(0, (float)-Random.NextDouble());
                particle.Color = Color.Gold;
                particle.Scale = 5f;
                particle.Life = 20.0f;
            };

            particleSystem3.UpdateParticle = (float deltaT, ref Particle particle) =>
            {
                particle.Velocity += (deltaT * particle.Acceleration) * 1.3f;
                particle.Position += deltaT * particle.Velocity;
                particle.Scale -= deltaT;
                particle.Life -= deltaT;
            };
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

            ball1.Update(gameTime);
            ball2.Update(gameTime);
            player.Update(gameTime);

            if ((ball1.State == GameState.Game) && (ball2.State == GameState.Game))
            {
                score++;
            }

            // TODO: Add your update logic here
            if(player.Bounds.CollidesWith(ball1.Bounds))
            {
                if(!((ball1.Bounds.Y > player.Bounds.Y + player.Bounds.Height) || (ball1.Bounds.Y + ball1.Bounds.Radius < player.Bounds.Y)))
                {
                    ball1.Velocity *= 0;
                    ball2.Velocity *= 0;
                    ball1.State = GameState.Over;

                }
                else if(!((ball1.Bounds.X > player.Bounds.X + player.Bounds.Width) || (ball1.Bounds.X + ball1.Bounds.Radius < player.Bounds.X)))
                {
                    ball1.Velocity *= 0;
                    ball2.Velocity *= 0;
                    ball1.State = GameState.Over;
                }
            }

            if (player.Bounds.CollidesWith(ball2.Bounds))
            {
                if (!((ball2.Bounds.Y > player.Bounds.Y + player.Bounds.Height) || (ball2.Bounds.Y + ball2.Bounds.Radius < player.Bounds.Y)))
                {
                    ball2.Velocity *= 0;
                    ball1.Velocity *= 0;
                    ball2.State = GameState.Over;

                }
                else if (!((ball2.Bounds.X > player.Bounds.X + player.Bounds.Width) || (ball2.Bounds.X + ball2.Bounds.Radius < player.Bounds.X)))
                {
                    ball2.Velocity *= 0;
                    ball1.Velocity *= 0;
                    ball2.State = GameState.Over;
                }
            }

            particleSystem1.Update(gameTime);
            particleSystem2.Update(gameTime);
            particleSystem3.Update(gameTime);

            base.Update(gameTime);
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.DeepSkyBlue);

            var offset = new Vector2(750, 500);
            offset.X -= player.Bounds.X;
            offset.Y -= player.Bounds.Y;
            var tMatrix = Matrix.CreateTranslation(offset.X, offset.Y, 0);

            // TODO: Add your drawing code here
            spriteBatch.Begin(SpriteSortMode.Deferred, null, null, null, null, null, tMatrix);
            //spriteBatch.Begin();
            ball1.Draw(spriteBatch);
            ball2.Draw(spriteBatch);
            player.Draw(spriteBatch);
            particleSystem1.Draw();
            particleSystem2.Draw();
            particleSystem3.Draw();

            spriteBatch.Draw(wallText, wallN, Color.Brown);
            spriteBatch.Draw(wallText, wallS, Color.Brown);
            spriteBatch.Draw(wallText, wallE, Color.Brown);
            spriteBatch.Draw(wallText, wallW, Color.Brown);

            var textOffset1 = offset * -1;
            var textOffset2 = offset * -1;
            textOffset2.Y += 31;
            var textOffset3 = offset * -1;
            textOffset3.X += 500;

            spriteBatch.DrawString(font, "Use the arrow keys to move", textOffset1, Color.White);
            spriteBatch.DrawString(font, "Dodge the balls", textOffset2, Color.White);
            spriteBatch.DrawString(font, $"Score: {score}", textOffset3, Color.White);

            if ((ball1.State == GameState.Over) || (ball2.State == GameState.Over))
            {
                var textOffsetGameOver = offset * -1;
                textOffsetGameOver.X += 500;
                textOffsetGameOver.Y += 500;
                spriteBatch.DrawString(font, "Game Over", textOffsetGameOver, Color.White);
            }

            spriteBatch.End();

            base.Draw(gameTime);
        }
    }
}
