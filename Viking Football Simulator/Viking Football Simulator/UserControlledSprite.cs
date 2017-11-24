using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace VikingFootballSimulator
{
    class UserControlledSprite : Sprite
    {
        Vector2 kickStrength = new Vector2(20, -10);

        //Variables for keyboard input
        Keys movementKeyLeft, movementKeyRight, movementKeyUp;
        KeyboardState keyboardState;

        //Constructors
        public UserControlledSprite(Vector2 position, Texture2D textureImage, bool direction, Keys movementKeyLeft, Keys movementKeyRight, Keys movementKeyUp)
            : base(position, textureImage, direction)
        {
            this.movementKeyLeft = movementKeyLeft;
            this.movementKeyRight = movementKeyRight;
            this.movementKeyUp = movementKeyUp;

            frictionGround = 0.9f;
            frictionAir = 0.9f;
        }


        public void Update(KeyboardState keyboardState, GameWindow gameWindow, GameTime gameTime) //Controls the update of UserCotrolledSprites
        {
            //Get current KeyboardState
            this.keyboardState = keyboardState;

            //Movement
            if (keyboardState.IsKeyDown(movementKeyLeft))
            {
                if (speed.X >= -12f) { speed.X -= 2.0f; }
                direction = true;
            }
            if (keyboardState.IsKeyDown(movementKeyRight))
            {
                if (speed.X <= 12f) { speed.X += 2.0f; }
                direction = false;
            }

            //Checks to see if touching groung, then you can jump
            if (keyboardState.IsKeyDown(movementKeyUp) && position.Y >= gameWindow.ClientBounds.Height - Dimensions.Height - groundLevel)
            {
                speed.Y = -12.0f;
            }

            base.Update(gameWindow, gameTime);
        }

        public bool UpdateCollisions(AutomatedSprite ball) //Check for collisions
        {
            //Check if the rectangles intersect
            if (Dimensions.Intersects(ball.Dimensions))
            {
                //Check if the circles intersects
                if ((Radius + ball.Radius) >= Math.Sqrt(Math.Pow(CenterPosition.X - ball.CenterPosition.X, 2) + (Math.Pow(CenterPosition.Y - ball.CenterPosition.Y, 2))))
                {

                    //Kick the ball
                    if (direction) { ball.Kick(-kickStrength.X, kickStrength.Y); }
                    else { ball.Kick(kickStrength.X, kickStrength.Y); }


                    return true;
                }
            }
            return false;
        }
    }
}
