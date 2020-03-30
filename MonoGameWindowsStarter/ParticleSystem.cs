using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace MonoGameWindowsStarter
{
    public delegate void ParticleSpawner(ref Particle particle);
    public delegate void ParticleUpdate(float deltaT, ref Particle particle);

    public class ParticleSystem
    {
        Particle[] particles;
        Texture2D texture;
        SpriteBatch spriteBatch;
        Random random = new Random();
        int nextIndex = 0;

        public Vector2 Emitter { get; set; }

        public int SpawnPerFrame { get; set; }

        public ParticleSpawner SpawnParticle { get; set; }

        public ParticleUpdate UpdateParticle { get; set; }

        public ParticleSystem(GraphicsDevice graphicsDevice, int size, Texture2D texture)
        {
            this.particles = new Particle[size];
            this.spriteBatch = new SpriteBatch(graphicsDevice);
            this.texture = texture;
        }

        public void Update(GameTime gameTime)
        {
            if(SpawnParticle == null || UpdateParticle == null)
            {
                return;
            }

            for(int i = 0; i < SpawnPerFrame; i++)
            {
                SpawnParticle(ref particles[nextIndex]);

                nextIndex++;
                if (nextIndex > particles.Length - 1)
                {
                    nextIndex = 0;
                }
            }

            float deltaT = (float)gameTime.ElapsedGameTime.TotalSeconds;

            for(int i = 0; i < particles.Length; i++)
            {
                if(particles[i].Life <= 0)
                {
                    continue;
                }

                UpdateParticle(deltaT, ref particles[i]);
            }
        }

        public void Draw()
        {
            spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.Additive);

            for(int i = 0; i < particles.Length; i++)
            {
                if (particles[i].Life <= 0)
                {
                    continue;
                }

                spriteBatch.Draw(texture, particles[i].Position, null, particles[i].Color, 0f, Vector2.Zero, particles[i].Scale, SpriteEffects.None, 0);
            }

            spriteBatch.End();
        }
    }
}
