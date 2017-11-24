using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace VikingFootballSimulator
{
    class FixedSprite : Sprite
    {
        //Variables
        bool whichGoal;

        //Constructor
        public FixedSprite(Vector2 position, Texture2D textureImage, bool direction)
            : base(position, textureImage, direction) { }
        //Constructor
        public FixedSprite(Vector2 position, Texture2D textureImage, bool direction, bool whichGoal)
            : base(position, textureImage, direction)
        {
            this.whichGoal = whichGoal;
        }

        //Methods
        public bool UpdateCollitions(AutomatedSprite ball) //Check for collitions
        {
            //Check if the rectangles intersect
            if (Dimensions.Intersects(ball.Dimensions))
            {
                if (whichGoal && ball.SpeedX > 0) { return true; }
                if (!whichGoal && ball.SpeedX < 0) { return true; }
            }
            return false;
        }
    }
}
