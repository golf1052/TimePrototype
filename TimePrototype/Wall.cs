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
    public class Wall
    {
        public Sprite sprite;
        

        public Wall(GraphicsDeviceManager graphics)
        {
            sprite = new Sprite(graphics);
        }

        public void Update(GameTimeWrapper gameTime, Circle circle)
        {
            if (circle.rectangle.Intersects(sprite.rectangle))
            {
                SortedDictionary<float, Action> moveFuncs = new SortedDictionary<float, Action>();
                if (circle.rectangle.Right < sprite.rectangle.Center.X)
                {
                    circle.jumpState = Circle.JumpStates.WallRight;
                    float distance = Math.Abs(circle.position.X - (sprite.rectangle.Left - circle.tex.Height / 2f));
                    moveFuncs.Add(distance, () =>
                    {
                        if (circle.velocity.X != 0)
                        {
                            circle.storedXVelocity = circle.velocity.X;
                        }
                        circle.velocity.X = 0;
                        circle.velocity.Y /= 1.5f;
                        circle.position.X = sprite.rectangle.Left - circle.tex.Height / 2f;
                    });
                }
                else if (circle.rectangle.Left > sprite.rectangle.Center.X)
                {
                    circle.jumpState = Circle.JumpStates.WallLeft;
                    float distance = Math.Abs(circle.position.X - (sprite.rectangle.Right + circle.tex.Height / 2f));
                    moveFuncs.Add(distance, () =>
                    {
                        if (circle.velocity.X != 0)
                        {
                            circle.storedXVelocity = circle.velocity.X;
                        }
                        circle.velocity.X = 0;
                        circle.velocity.Y /= 4;
                        circle.position.X = sprite.rectangle.Right + circle.tex.Height / 2f;
                    });
                }

                if (circle.rectangle.Bottom < sprite.rectangle.Center.Y)
                {
                    circle.jumpState = Circle.JumpStates.Ground;
                    float distance = Math.Abs(circle.position.Y - (sprite.rectangle.Top - circle.tex.Height / 2f));
                    moveFuncs.Add(distance, () =>
                    {
                        circle.velocity.Y = 0;
                        circle.position.Y = sprite.rectangle.Top - circle.tex.Height / 2f;
                    });
                }
                else if (circle.rectangle.Top < sprite.rectangle.Center.Y)
                {
                    circle.jumpState = Circle.JumpStates.Ground;
                    float distance = Math.Abs(circle.position.Y - (sprite.rectangle.Bottom + circle.tex.Height / 2f));
                    moveFuncs.Add(distance, () =>
                    {
                        circle.velocity.Y = 0;
                        circle.position.Y = sprite.rectangle.Bottom + circle.tex.Height / 2f;
                    });
                }

                if (moveFuncs.Count > 0)
                {
                    moveFuncs.First().Value.Invoke();
                }
            }
            sprite.Update(gameTime);
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            sprite.DrawRect(spriteBatch);
        }
    }
}
