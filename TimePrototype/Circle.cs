using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GLX;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace TimePrototype
{
    public class Circle : Sprite
    {
        public enum JumpStates
        {
            Ground,
            WallRight,
            WallLeft,
            Air
        }

        public JumpStates jumpState;
        public float storedXVelocity;
        private Sprite future;

        public Circle(Texture2D tex, Texture2D futureTex) : base(tex)
        {
            jumpState = JumpStates.Ground;
            storedXVelocity = 0;
            future = new Sprite(futureTex);
            future.color = Color.Red;
        }

        public override void Update(GameTimeWrapper gameTime)
        {
            future.position = position + Vector2.Normalize(velocity) * 100;
            future.Update(gameTime);
            base.Update(gameTime);
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            base.Draw(spriteBatch);
            future.Draw(spriteBatch);
        }
    }
}
