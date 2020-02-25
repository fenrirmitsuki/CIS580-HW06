using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;

namespace MonoGameWindowsStarter
{
    enum State
    {
        South = 0,
        East = 1,
        West = 2,
        North = 3,
        Idle = 4,
    }

    public class Player
    {
        Game1 game;
        public BoundingRectangle Bounds;
        Texture2D texture;
        public SoundEffect playerBounce01;
        public SoundEffect playerBounce02;

        const int ANIM_FRAMERATE = 124;
        const float PLAYER_SPEED = 1;
        const int FRAME_WIDTH = 49;
        const int FRAME_HEIGHT = 63;
        State state;
        TimeSpan timer;
        int frame;
        public Vector2 Position;
        Vector2 origin;

        public Player(Game1 game)
        {
            this.game = game;
            timer = new TimeSpan(0);
            state = State.Idle;
            Position = new Vector2(200,200);
            origin = new Vector2(0,0);
        }

        public void Initialize()
        {

            //Bounds = new BoundingRectangle(Position - 1.8f * origin, 100, 150);
            Bounds.Width = 100;
            Bounds.Height = 150;
            Bounds.X = (game.GraphicsDevice.Viewport.Width) / 2;
            Bounds.Y = (game.GraphicsDevice.Viewport.Height) - Bounds.Height;


            Position.X = (game.GraphicsDevice.Viewport.Width) / 2;
            Position.Y = (game.GraphicsDevice.Viewport.Height) - Bounds.Height;
        }

        public void LoadContent(ContentManager content)
        {
            texture = content.Load<Texture2D>("spriteSheet");
            playerBounce01 = content.Load<SoundEffect>("bounce02");
            playerBounce02 = content.Load<SoundEffect>("bounce03");
        }

        public void Update(GameTime gameTime)
        {
            var keyboardState = Keyboard.GetState();
            float delta = (float)gameTime.ElapsedGameTime.TotalMilliseconds;

            if(keyboardState.IsKeyDown(Keys.Up))
            {
                state = State.North;
                Bounds.Y -= delta * PLAYER_SPEED;
                //position.Y -= delta * PLAYER_SPEED;
            }
            else if(keyboardState.IsKeyDown(Keys.Left))
            {
                state = State.West;
                Bounds.X -= delta * PLAYER_SPEED;
                //position.X -= delta * PLAYER_SPEED; ;
            }
            else if(keyboardState.IsKeyDown(Keys.Right))
            {
                state = State.East;
                Bounds.X += delta * PLAYER_SPEED;
                //position.X += delta * PLAYER_SPEED;
            }
            else if(keyboardState.IsKeyDown(Keys.Down))
            {
                state = State.South;
                Bounds.Y += delta * PLAYER_SPEED; ;
                //position.Y += delta * PLAYER_SPEED;
            }
            else
            {
                state = State.Idle;
            }

            if(state != State.Idle)
            {
                timer += gameTime.ElapsedGameTime;
            }

            while (timer.TotalMilliseconds > ANIM_FRAMERATE)
            {
                frame++;
                timer -= new TimeSpan(0, 0, 0, 0, ANIM_FRAMERATE);
            }
            frame %= 4;

            if (Bounds.X < 0)
            {
                Bounds.X = 0;
            }
            if (Bounds.X > game.GraphicsDevice.Viewport.Width - Bounds.Width)
            {
                Bounds.X = game.GraphicsDevice.Viewport.Width - Bounds.Width;
            }
            if (Bounds.Y > game.GraphicsDevice.Viewport.Height - Bounds.Height)
            {
                Bounds.Y = game.GraphicsDevice.Viewport.Height - Bounds.Height;
            }
            if (Bounds.Y < 2 * (game.GraphicsDevice.Viewport.Height) / 3)
            {
                Bounds.Y = 2 * (game.GraphicsDevice.Viewport.Height) / 3;
            }

        }

        public void Draw(SpriteBatch spriteBatch)
        {
            var source = new Rectangle(
                frame * FRAME_WIDTH,
                (int)state % 4 * FRAME_HEIGHT,
                FRAME_WIDTH,
                FRAME_HEIGHT
                );

            //spriteBatch.Draw(texture, Bounds, source, Color.White);
            spriteBatch.Draw(texture, Bounds, source, Color.White, 0, origin, SpriteEffects.None, 1);
        }
    }
}
