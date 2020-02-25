using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;

namespace MonoGameWindowsStarter
{
    public enum GameState
    {
        Game = 0,
        Over = 1,
    }

    public class Ball
    {
        
        Game1 game;
        Texture2D texture;
        SoundEffect wallBounce;
        public BoundingCircle Bounds;
        public Vector2 Velocity;
        public GameState State;
        SpriteFont font;

        //Create new Ball
        public Ball(Game1 game)
        {
            this.game = game;
            State = GameState.Game;
        }

        //Initialize Ball
        public void Initialize()
        {
            //Set ball's radius to 25 pixels
            Bounds.Radius = 50;

            //Set ball's starting position to top-middle of window
            Bounds.X = game.GraphicsDevice.Viewport.Width / 2;
            Bounds.Y = 0;

            //give Ball random velocity
            Velocity = new Vector2((float)game.Random.NextDouble(), (float)game.Random.NextDouble());
            Velocity.Normalize();
        }

        //Load Ball's textures and SFX
        public void LoadContent(ContentManager content)
        {
            texture = content.Load<Texture2D>("ball");
            wallBounce = content.Load<SoundEffect>("bounce01");
            font = content.Load<SpriteFont>("defaultFont");
        }

        //Update Ball position and bounce off walls
        public void Update(GameTime gameTime)
        {
            var viewport = game.GraphicsDevice.Viewport;

            Bounds.Center += (float)gameTime.ElapsedGameTime.TotalMilliseconds * Velocity;

            if(Bounds.Center.Y - Bounds.Radius < 0)
            {
                Velocity.Y *= -1;
                float delta = Bounds.Radius - Bounds.Y;
                Bounds.Y += 2 * delta;
                wallBounce.Play();
            }
            if (Bounds.Center.Y + Bounds.Radius > 1000)
            {
                Velocity.Y *= -1;
                float delta = 1000 - Bounds.Radius - Bounds.Y;
                Bounds.Y += 2 * delta;
                wallBounce.Play();
            }
            if (Bounds.Center.X - Bounds.Radius < 0)
            {
                Velocity.X *= -1;
                float delta = Bounds.Radius - Bounds.X;
                Bounds.X += 2 * delta;
                wallBounce.Play();
            }
            if (Bounds.Center.X + Bounds.Radius > 1750)
            {
                Velocity.X *= -1;
                float delta = 1750 - Bounds.Radius - Bounds.X;
                Bounds.X += 2 * delta;
                wallBounce.Play();
            }
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(texture, Bounds, Color.White);
        }
    }
}
