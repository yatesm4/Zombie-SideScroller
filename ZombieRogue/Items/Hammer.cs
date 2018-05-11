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
    public class Hammer : Weapon
    {
        public Hammer(ContentManager content, Vector2 position, string weapon_name, int[] skin_args) : base(content, position, weapon_name, skin_args)
        {
            LoadContent(content, weapon_name, skin_args);
        }

        public override void Update(GameTime gameTime, KeyboardState keyboardState, Character owner)
        {
            // update hammer here
        }
    }
}
