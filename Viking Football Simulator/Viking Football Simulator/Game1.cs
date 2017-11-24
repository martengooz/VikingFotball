using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace VikingFootballSimulator
{
    /// <summary>
    /// This is the main type for your game
    /// </summary>
    public class Game1 : Microsoft.Xna.Framework.Game
    {
        //Default Variables
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;

        //Textures
        Texture2D player1Texture, player2Texture;

        Texture2D ballTexture, goalTexture, grassTexture, backgroundTexture;

        Texture2D player1IconTexture, player2IconTexture, menuBackground;

        SpriteFont font;

        //Sprites
        UserControlledSprite player1, player2;

        AutomatedSprite ball;

        FixedSprite goal1, goal2, grass, background;

        //Game variables
        int player1Points = 0;
        int player2Points = 0;

        bool gameRunning = false; //Is the game running
        bool pauseMenu = false; //Is the menu paused
        bool selectItem = true; //Selected item in the menu (true = continue game, false = exit)

        bool player1Win = false; //True if the screen for player 1 win should be displayed
        bool player2Win = false; //Same for player 2

        //Rendering
        Color backgroundColor = Color.CornflowerBlue;

        public Game1()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";

        }

        /// <summary>
        /// Allows the game to perform any initialization it needs to before starting to run.
        /// This is where it can query for any required services and load any non-graphic
        /// related content.  Calling base.Initialize will enumerate through any components
        /// and initialize them as well.
        /// </summary>
        protected override void Initialize()
        {
            base.Initialize();
        }

        /// <summary>
        /// LoadContent will be called once per game and is the place to load
        /// all of your content.
        /// </summary>
        protected override void LoadContent()
        {
            // Create a new SpriteBatch, which can be used to draw textures.
            spriteBatch = new SpriteBatch(GraphicsDevice);

            //Font
            font = this.Content.Load<SpriteFont>("Font");

            //Textures
            player1Texture = this.Content.Load<Texture2D>(@"img\player1");
            player2Texture = this.Content.Load<Texture2D>(@"img\player2");

            ballTexture = this.Content.Load<Texture2D>(@"img\ball");

            goalTexture = this.Content.Load<Texture2D>(@"img/goal");
            grassTexture = this.Content.Load<Texture2D>(@"img/grass");
            backgroundTexture = this.Content.Load<Texture2D>(@"img/background");

            player1IconTexture = this.Content.Load<Texture2D>(@"img\head1");
            player2IconTexture = this.Content.Load<Texture2D>(@"img\head2");
            menuBackground = this.Content.Load<Texture2D>(@"img\menu");

            //Sprites
            player1 = new UserControlledSprite(new Vector2(-328, 300), player1Texture, false, Keys.A, Keys.D, Keys.W);
            player2 = new UserControlledSprite(new Vector2(200, 300), player2Texture, true, Keys.Left, Keys.Right, Keys.Up);

            ball = new AutomatedSprite(new Vector2(-32, 300), ballTexture, false);

            goal1 = new FixedSprite(new Vector2(-1264, 220), goalTexture, false, false);
            goal2 = new FixedSprite(new Vector2(1200, 220), goalTexture, true, true);
            grass = new FixedSprite(new Vector2(-2000, 440), grassTexture, true);
            background = new FixedSprite(new Vector2(-1800, -40), backgroundTexture, true);
            background.Paralax = 4; //Activate paralax on the background background
        }

        /// <summary>
        /// UnloadContent will be called once per game and is the place to unload
        /// all content.
        /// </summary>
        protected override void UnloadContent()
        {
            // TODO: Unload any non ContentManager content here
        }

        /// <summary>
        /// Allows the game to run logic such as updating the world,
        /// checking for collisions, gathering input, and playing audio.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Update(GameTime gameTime)
        {
            // Allows the game to exit
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed)
                this.Exit();

            // TODO: Add your update logic here

            //Get keys
            KeyboardState keyboardState = Keyboard.GetState();

            if (gameRunning) //If the game is running, run the game logic
            {
                //Sprites update
                player1.Update(Keyboard.GetState(), Window, gameTime);
                player2.Update(Keyboard.GetState(), Window, gameTime);

                ball.Update(Window, gameTime);

                //Check for collitions between player and  ball
                if (player1.UpdateCollisions(ball) || player2.UpdateCollisions(ball)) { }

                //Check if ball goes out of bounds for a freekick
                if (ball.Dimensions.X >= 1400 || ball.Dimensions.X <= -1464)
                {
                    player1.FreeKick(goal1);
                    player2.FreeKick(goal2);

                    ball.FreeKick();
                }

                //Check for collitions between goal and ball
                if (goal1.UpdateCollitions(ball))
                {
                    player2Points++;

                    player1.Reset();
                    player2.Reset();
                    ball.Reset();
                }

                if (goal2.UpdateCollitions(ball))
                {
                    player1Points++;

                    player1.Reset();
                    player2.Reset();
                    ball.Reset();
                }

                //Calls for winning screen
                if (player1Points >= 10) { player1Win = true; gameRunning = false; player1Points = 0; player2Points = 0; }
                if (player2Points >= 10) { player2Win = true; gameRunning = false; player1Points = 0; player2Points = 0; }

                //Pause
                if (keyboardState.IsKeyDown(Keys.Escape)) { pauseMenu = true; gameRunning = false; }
            }
            else //Menu logic
            {
                //Select items
                if (keyboardState.IsKeyDown(Keys.Up)) { selectItem = true; }
                if (keyboardState.IsKeyDown(Keys.Down)) { selectItem = false; }

                //Select item
                if (keyboardState.IsKeyDown(Keys.Enter))
                {
                    if (selectItem) { gameRunning = true; }
                    if (!selectItem) { this.Exit(); }
                }

                //Close win screen if enter or esc is pressed
                if ((player1Win || player2Win) && (keyboardState.IsKeyDown(Keys.Enter) || keyboardState.IsKeyDown(Keys.Escape)))
                {
                    player1Win = false;
                    player2Win = false;
                }
            }
            base.Update(gameTime);
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(backgroundColor);

            // TODO: Add your drawing code here
            spriteBatch.Begin();

            if (gameRunning) //Draw game
            {

                //Draw UI
                spriteBatch.DrawString(font, Convert.ToString(player1Points) + " - " + Convert.ToString(player2Points), new Vector2(355, 10), Color.White);
                spriteBatch.Draw(player1IconTexture, new Rectangle(290, 10, 50, 50), Color.White);
                spriteBatch.Draw(player2IconTexture, new Rectangle(450, 10, 50, 50), Color.White);

                //Draw background
                background.Draw(spriteBatch, ball, Window);

                //Draw Sprites 
                player1.Draw(spriteBatch, ball, Window);
                player2.Draw(spriteBatch, ball, Window);

                ball.Draw(spriteBatch, ball, Window);

                //Foreground
                goal1.Draw(spriteBatch, ball, Window);
                goal2.Draw(spriteBatch, ball, Window);
                grass.Draw(spriteBatch, ball, Window);
            }
            else //Draw menu 
            {
                spriteBatch.Draw(menuBackground, Vector2.Zero, Color.White);

                if (player1Win || player2Win)
                {
                    spriteBatch.DrawString(font, player1Win ? "Redbeard wins!" : "Greybeard wins!", new Vector2(260, 290), Color.White);
                    spriteBatch.DrawString(font, "Press 'Enter' to restart", new Vector2(200, 350), selectItem ? Color.White : Color.DarkGray);
                }
                else
                {
                    spriteBatch.DrawString(font, pauseMenu ? "Continue" : "Start game", new Vector2(310, 250), selectItem ? Color.White : Color.DarkGray);
                    spriteBatch.DrawString(font, "Exit", new Vector2(310, 290), !selectItem ? Color.White : Color.DarkGray);
                }
            }
            spriteBatch.End();
            base.Draw(gameTime);
        }
    }
}
