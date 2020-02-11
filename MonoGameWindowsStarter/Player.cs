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
        North = 2,
        West = 3,
        Idle = 4
    }

    public class Player
    {
        Game1 game;
        public BoundingRectangle Bounds;
        Texture2D texture;
        public SoundEffect playerBounce01;
        public SoundEffect playerBounce02;

        const int ANIM_FRAMRATE = 124;
        const float PLAYER_SPEED = 100;
        const int FRAME_WIDTH = 100;
        const int FRAME_HEIGHT = 100;
        State state;
        TimeSpan timer;
        int frame;

        public Player(Game1 game)
        {
            this.game = game;
            timer = new TimeSpan(0);
            state = State.Idle;
        }

        public void Initialize()
        {
            Bounds.Width = 100;
            Bounds.Height = 150;
            Bounds.X = (game.GraphicsDevice.Viewport.Width) / 2;
            Bounds.Y = (game.GraphicsDevice.Viewport.Height) - Bounds.Height;
        }

        public void LoadContent(ContentManager content)
        {
            texture = content.Load<Texture2D>("playerSheet");
            playerBounce01 = content.Load<SoundEffect>("bounce02");
            playerBounce02 = content.Load<SoundEffect>("bounce03");
        }

        public void Update(GameTime gameTime)
        {
            var keyboardState = Keyboard.GetState();

            if(keyboardState.IsKeyDown(Keys.Left))
            {
                state = State.West;
                Bounds.X -= (float)gameTime.ElapsedGameTime.TotalMilliseconds;
            }
            else if(keyboardState.IsKeyDown(Keys.Right))
            {
                state = State.East;
                Bounds.X += (float)gameTime.ElapsedGameTime.TotalMilliseconds;
            }
            else
            {
                state = State.Idle;
            }

            if(state != State.Idle)
            {
                timer += gameTime.ElapsedGameTime;
            }

            if(Bounds.X < 0)
            {
                Bounds.X = 0;
            }
            if(Bounds.X > game.GraphicsDevice.Viewport.Width - Bounds.Width)
            {
                Bounds.X = game.GraphicsDevice.Viewport.Width - Bounds.Width;
            }

            while(timer.TotalMilliseconds > ANIM_FRAMRATE)
            {
                frame++;
                timer -= new TimeSpan(0,0,0,0, ANIM_FRAMRATE);
            }
            frame %= 4;
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            var source = new Rectangle(
                frame * FRAME_WIDTH,
                (int)state % 4 * FRAME_HEIGHT,
                FRAME_WIDTH,
                FRAME_HEIGHT
                );
            spriteBatch.Draw(texture, Bounds, Color.White);
        }
    }
}
