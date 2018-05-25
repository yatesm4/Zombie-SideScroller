using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using ZombieRogue.Objects;

namespace ZombieRogue.FX
{
    public class SpecialEffect
    {
        public Vector2 Position = Vector2.Zero;
        public float EffectSpeed = 0.075f;
        public float Duration = 2.5f;
        public bool HasEnded = false;
        public float Scale = 1;

        public Animation Spr_Effect;
        public AnimationPlayer Sprite;

        public bool IsGrowing = false;
        public float Growth = 0.0f;

        public SpecialEffect(ContentManager content, Vector2 position, string effect, float effectSpeed = 0.0f)
        {
            Position = position;
            if (effectSpeed > 0)
                EffectSpeed = effectSpeed;
            LoadContent(content, effect);
        }

        public virtual void LoadContent(ContentManager content, string effect_name)
        {
            Spr_Effect = new Animation(content.Load<Texture2D>($"Sprites/FX/{effect_name}"), EffectSpeed, true);
            Sprite.PlayAnimation(Spr_Effect);
            Sprite.Scale = 0.4f;
        }

        public virtual void Update(GameTime gameTime, KeyboardState keyboardState)
        {
            if (HasEnded.Equals(false))
            {
                Console.WriteLine($"Effect timer: {Duration}");
                if (Duration > 0)
                {
                    Duration -= 0.1f;
                }
                else
                {
                    HasEnded = true;
                }
            }
        }

        public virtual void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            if (HasEnded.Equals(false))
                Sprite.Draw(gameTime, spriteBatch, Position, SpriteEffects.None);
        }
    }
}
