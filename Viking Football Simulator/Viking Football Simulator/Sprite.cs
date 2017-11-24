using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace VikingFootballSimulator
{
    class Sprite
    {
        //Position & movement
        protected Vector2 startPosition, position;
        protected bool startDirection, direction;
        protected Vector2 speed;
        protected float frictionAir, frictionGround;
        protected static int groundLevel = 20;
        protected float bounciness;
        int radius;

        //Animation
        Texture2D textureImage;
        SpriteEffects flip;
        int paralax = 1;

        //Constructors
        public Sprite()
        {
            position = Vector2.Zero;
            startPosition = position;
            textureImage = null;
            direction = false;
            speed = Vector2.Zero;
        }

        public Sprite(Vector2 position, Texture2D textureImage, bool direction)
        {
            this.position = position;
            this.startPosition = position;
            this.textureImage = textureImage;
            this.direction = direction;
            this.startDirection = direction;
            this.radius = Dimensions.Width / 2;
            this.speed = Vector2.Zero;
        }

        //Properties
        public int Radius //Changes the radius of the sprite (used for collitions)
        {
            get { return radius; }
            set { radius = value; }
        }

        public Rectangle Dimensions //Returns an rectangle with the sprites position and measurements
        {
            get { return new Rectangle((int)position.X, (int)position.Y, textureImage.Bounds.Width, textureImage.Bounds.Height); }
        }

        public Vector2 CenterPosition //Returns the position of center
        {
            get { return new Vector2(Dimensions.X + (Dimensions.Width / 2), Dimensions.Y + (Dimensions.Height / 2)); }
        }

        public float SpeedX //Speed in X-axies
        {
            get { return speed.X; }
        }

        public int Paralax //Control the paralax effect
        {
            get { return paralax; }
            set { paralax = value; }
        }

        //Methods
        public void Update(GameWindow gameWindow, GameTime gameTime)
        {
            ///Gravity and friction
            //Check if the sprite is touching the ground (groundLevel from bottom)
            if (position.Y == gameWindow.ClientBounds.Height - Dimensions.Height - groundLevel)
            {
                //If it has bounciness, make it bounce
                if (bounciness > 0)
                {
                    if (speed.Y <= -2f || speed.Y >= 2f) { speed.Y *= -1 * bounciness; }
                    else { speed.Y = 0; }
                }

                //Make friction so speed decreases
                speed.X *= frictionGround;
            }
            else { speed.X *= frictionAir; }

            //X-position
            position.X += speed.X;
            //Y-position
            position.Y += speed.Y;
            
            //Gravity
            speed.Y += 0.4f;

            //Prevent sprites from going out of bounds
            if (position.Y > gameWindow.ClientBounds.Height - Dimensions.Height - groundLevel)
                position.Y = gameWindow.ClientBounds.Height - Dimensions.Height - groundLevel;

            if (position.X > 1400) { position.X = 1400; }
            if (position.X < -1528) { position.X = -1528; }
        }

        //Reset the players for a new game/after a goal
        public void Reset()
        {
            position = startPosition;
            direction = startDirection;
            speed = Vector2.Zero;
        }

        //Move the player for a freekick
        public void FreeKick(FixedSprite goal)
        {
            position = goal.position;
            direction = startDirection;
            speed = Vector2.Zero;
        }

        public void Draw(SpriteBatch spriteBatch, AutomatedSprite ball, GameWindow window) //Draws the sprite on screen
        {
            //Flip the texture
            if (direction) { flip = SpriteEffects.None; }
            else { flip = SpriteEffects.FlipHorizontally; }

            //Draw the sprite
            spriteBatch.Draw(textureImage, new Rectangle(((int)position.X - (int)ball.CenterPosition.X + (window.ClientBounds.Width / 2)) / paralax, (int)position.Y, textureImage.Width, textureImage.Height),
            new Rectangle(0, 0, textureImage.Width, textureImage.Height), Color.White, 0, Vector2.Zero, flip, 0);

        }
    }
}
