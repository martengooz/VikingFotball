using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
namespace VikingFootballSimulator
{
    class AutomatedSprite : Sprite
    {
        //Constructor
        public AutomatedSprite(Vector2 position, Texture2D textureImage, bool direction) : base(position, textureImage, direction) { frictionGround = 0.97f; frictionAir = 0.98f; bounciness = 0.6f; }

        public void Kick(float X, float Y)
        {
            //Set speeds
            speed.X = X;

            speed.Y = Y;

        }

        //Place the ball at appropriate goal when freekick
        public void FreeKick()
        {
            if (Dimensions.X >= 1400) { position = new Vector2(1000, 300); }
            if (Dimensions.X <= -1464) { position = new Vector2(-1064, 300); }
            speed = Vector2.Zero;
        }
    }
}
