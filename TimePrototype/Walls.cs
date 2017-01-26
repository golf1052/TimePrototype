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
    public class Walls
    {
        private GraphicsDeviceManager graphics;
        public List<Wall> walls;

        public Walls(GraphicsDeviceManager graphics)
        {
            this.graphics = graphics;
            walls = new List<Wall>();
        }

        public void Create(Vector2 size, Vector2 position)
        {
            Wall wall = new Wall(graphics);
            wall.sprite.DrawSize = size;
            wall.sprite.position = position;
            walls.Add(wall);
        }

        public void Update(GameTimeWrapper gameTime, Circle circle)
        {
            foreach (var wall in walls)
            {
                wall.Update(gameTime, circle);
            }
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            foreach (var wall in walls)
            {
                wall.Draw(spriteBatch);
            }
        }
    }
}
