using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Input;

using ZombieRogue.Objects;

namespace ZombieRogue.Items
{
    public class Projectile : Weapon
    {
        public Animation Spr_Projectile;

        public Vector2 InitialPosition;

        public float ProjectileSpeed = 2.5f;

        public Vector2 Trajectory;

        // returning = true?

        public Projectile(ContentManager content, Vector2 position, string weapon_name, int[] skin_args, Vector2 direction) : base(content, position, weapon_name, skin_args)
        {
            InitialPosition = position;
            Trajectory = direction;
            LoadContent(content, weapon_name, skin_args);
            Reset(position);
        }

        public override void LoadContent(ContentManager content, string weapon_name, int[] skin_args)
        {
            Spr_Projectile = new Animation(content.Load<Texture2D>($"Sprites/Weapons/{weapon_name}/Projectile/0{skin_args[0]}"), 0.1f, false);
        }

        public override void Reset(Vector2 reset_position)
        {
            Position = reset_position;
            Sprite.PlayAnimation(Spr_Projectile);
        }

        public override void Update(GameTime gameTime, KeyboardState keyboardState, Character owner)
        {
            // update projectile here
            Position += new Vector2(ProjectileSpeed, 0);
        }
    }
}
