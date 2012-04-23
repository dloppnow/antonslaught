using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Media;

namespace AntOnslaught
{
    class AudioManager
    {
        Dictionary<int, SoundEffect> effectsToPlay;
        Dictionary<int, SoundEffect> effects;
        Random rand = new Random();
        private int time = 0;
        private bool hasIntroed = false;
        private bool hasStartedIntro = false;

        public enum EffectType
        {
            death_ant,
            munch,
            pop
        }

        public enum Effect
        {
            mainTheme,
            themeIntro,
            Blip,
            ant_death_1,
            ant_death_2,
            munch_1,
            munch_2,
            munch_3,
            pop_1,
            pop_2,
            pop_3
        }

        public AudioManager(ContentManager Content)
        {
            effectsToPlay = new Dictionary<int, SoundEffect>();
            effects = new Dictionary<int, SoundEffect>();
            //Load Songs
            effects.Add((int)Effect.mainTheme, Content.Load<SoundEffect>("bgm_loop"));
            effects.Add((int)Effect.themeIntro, Content.Load<SoundEffect>("bgm_intro"));
            //Load Effects
            effects.Add((int)Effect.Blip, Content.Load<SoundEffect>("blip"));
            effects.Add((int)Effect.ant_death_1, Content.Load<SoundEffect>("ant_death_1"));
            effects.Add((int)Effect.ant_death_2, Content.Load<SoundEffect>("ant_death_2"));
            effects.Add((int)Effect.munch_1, Content.Load<SoundEffect>("munch_1"));
            effects.Add((int)Effect.munch_2, Content.Load<SoundEffect>("munch_2"));
            effects.Add((int)Effect.munch_3, Content.Load<SoundEffect>("munch_3"));
            effects.Add((int)Effect.pop_1, Content.Load<SoundEffect>("pop_1"));
            effects.Add((int)Effect.pop_2, Content.Load<SoundEffect>("pop_2"));
            effects.Add((int)Effect.pop_3, Content.Load<SoundEffect>("pop_3"));
        }

        public bool queueEffect(Effect effect)
        {
            SoundEffect e = null;
            if (effects.TryGetValue((int)effect, out e))
            {
                try
                {
                    effectsToPlay.Add((int)effect, e);
                }
                catch (ArgumentException ae)
                {
                    //effectsToPlay already contains the effect we want to play. We shouldn't play it multiple times.
                }
                return true;
            }
            else
            {
                return false;
            }
        }

        public bool queueRandomEffectType(EffectType type)
        {
            bool good = false;
            if (type == EffectType.death_ant) {
                int r = rand.Next(1, 3);
                if (r == 1)
                {
                    good = queueEffect(Effect.ant_death_1);
                }
                else
                {
                    good = queueEffect(Effect.ant_death_2);
                }
            }
            else if (type == EffectType.munch)
            {
                int r = rand.Next(1, 4);
                if (r == 1)
                {
                    good = queueEffect(Effect.munch_1);
                }
                else if (r == 2)
                {
                    good = queueEffect(Effect.munch_2);
                }
                else if (r == 3)
                {
                    good = queueEffect(Effect.munch_3);
                }
            }
            else if (type == EffectType.pop)
            {
                int r = rand.Next(1, 4);
                if (r == 1)
                {
                    good = queueEffect(Effect.pop_1);
                }
                else if (r == 2)
                {
                    good = queueEffect(Effect.pop_2);
                }
                else if (r == 3)
                {
                    good = queueEffect(Effect.pop_3);
                }
            }
            return good;
        }

        public void playEffects()
        {
            foreach (KeyValuePair<int, SoundEffect> ef in effectsToPlay)
            {
                ef.Value.Play();
            }
            effectsToPlay.Clear();
        }

        public void update(GameTime gameTime)
        {
            time += gameTime.ElapsedGameTime.Milliseconds;
            if (time >= effects[(int)Effect.themeIntro].Duration.TotalMilliseconds - 1000 && !hasIntroed)
            {
                queueEffect(Effect.mainTheme);
                hasIntroed = true;
                time = 0;
            }
            else if (time >= effects[(int)Effect.mainTheme].Duration.TotalMilliseconds - 4970 && hasIntroed)
            {
                queueEffect(Effect.mainTheme);
                time = 0;
            }
            
        }

        public void startTheme()
        {
            if (!hasStartedIntro)
            {
                queueEffect(Effect.themeIntro);
                hasStartedIntro = true;
            }
            
        }
    }
}
