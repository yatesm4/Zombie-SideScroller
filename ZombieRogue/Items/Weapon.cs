using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

using ZombieRogue.Objects;

namespace ZombieRogue.Items
{
    public abstract class Weapon
    {
        public Animation Spr_Idle;
        public Animation Spr_Projectile;
        public Animation Spr_Swing;
        public Animation Spr_Swing_Throw;

        public AnimationPlayer Sprite;

        public Vector2 Position;

        public bool IsFlipped = false;

        public Weapon(ContentManager content, Vector2 position, string weapon_name, int[] skin_args)
        {
            LoadContent(content, weapon_name, skin_args);
            Reset(position);
        }

        public virtual void LoadContent(ContentManager content, string weapon_name, int[] skin_args)
        {
            Spr_Idle = new Animation(content.Load<Texture2D>($"Sprites/Weapons/{weapon_name}/Idle/0{skin_args[0]}"), 0.075f, true);
            Spr_Projectile = new Animation(content.Load<Texture2D>($"Sprites/Weapons/{weapon_name}/Projectile/0{skin_args[1]}"), 0.075f, true);
            Spr_Swing = new Animation(content.Load<Texture2D>($"Sprites/Weapons/{weapon_name}/Swing/0{skin_args[2]}"), 0.15f, true);
            Spr_Swing_Throw = new Animation(content.Load<Texture2D>($"Sprites/Weapons/{weapon_name}/Swing_Throw/0{skin_args[3]}"), 0.15f, true);
        }

        public virtual void Reset(Vector2 reset_position)
        {
            Position = reset_position;
            Sprite.PlayAnimation(Spr_Idle);
        }

        public abstract void Update(GameTime gameTime, KeyboardState keyboardState, Character owner);

        public void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            Sprite.Draw(gameTime, spriteBatch, Position, IsFlipped ? SpriteEffects.FlipHorizontally : SpriteEffects.None);
        }
    }
}
