using System;
using System.Collections.Generic;
using System.Linq;
using System.Collections;
using System.Text;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace LEJEU.Shared
{
	/*
	public class ScreenManager
	{
		#region Variables

		GameScreen currentScreen;
		GameScreen newScreen;

		/// <summary>
		/// Creating custom Content Manager
		/// </summary>
		ContentManager content;

		/// <summary>
		/// Screen Manager Instance
		/// </summary>
		private static ScreenManager instance;

		/// <summary>
		/// Screen Stack
		/// </summary>
		/// 
		Stack<GameScreen> screenStack = new Stack<GameScreen>();

		/// <summary>
		/// Screen's width and height
		/// </summary>
		/// 
//        Vector2 dimensions;

		bool transition;

		FadeAnimation fade = new FadeAnimation();

		Texture2D fadeTexture, nullImage;

		InputManager inputManager;

		#endregion

		#region Properties
		public static ScreenManager Instance
		{
			get {
				if (instance == null)
					instance = new ScreenManager();
				return instance;
			}
		}

		public ContentManager Content
		{
			get { return content; }
		}


//		public Vector2 Dimensions
//		{
//			get { return dimensions; }
//			set { dimensions = value; }
//		}



		public Texture2D NullImage
		{
			get { return nullImage; }
		}
		#endregion

		#region Main Methods

		public void AddScreen(GameScreen screen, InputManager inputManager)
		{
			transition = true;
			newScreen = screen;
			fade.IsActive = true;
			fade.Alpha = 0.0f;
			fade.ActivateValue = 1.0f;
			this.inputManager = inputManager;
		}

		public void AddScreen(GameScreen screen, InputManager inputManager, float alpha)
		{
			transition = true;
			newScreen = screen;
			fade.IsActive = true;
			fade.ActivateValue = 1.0f;
			if (alpha != 1.0f)
				fade.Alpha = 1.0f - alpha;
			else
				fade.Alpha = alpha;

			fade.Increase = true;
			this.inputManager = inputManager;
		}

		public void Initialize() 
		{
			currentScreen = new SplashScreen();
			currentScreen.Initialize();
			fade = new FadeAnimation();
			inputManager = new InputManager();
		}

		public void LoadContent(ContentManager Content)
		{
			content = new ContentManager(Content.ServiceProvider, "Content");
			currentScreen.LoadContent(content, inputManager);

			nullImage = this.content.Load<Texture2D>("null");
			fadeTexture = this.content.Load<Texture2D>("fade");
			fade.LoadContent(content, fadeTexture, "", Vector2.Zero);
			fade.Scale = ResolutionManager.WindowRes.X;
		}
		public void Update(GameTime gameTime, InputManager input)
		{
			if (!transition)
				currentScreen.Update(gameTime, input);
			else
				Transition(gameTime);
		}
		public void Draw(SpriteBatch sb)
		{
			currentScreen.Draw(sb);
			if (transition)
				fade.Draw(sb);
		}

		#endregion

		#region Private Methods

		private void Transition(GameTime gameTime)
		{
			fade.Update(gameTime);
			if (fade.Alpha == 1.0f && fade.Timer.TotalSeconds == 1.0f)
			{
				screenStack.Push(newScreen);
				currentScreen.UnloadContent();
				currentScreen = newScreen;
				currentScreen.Initialize();
				currentScreen.LoadContent(content, inputManager);
			}
			else if (fade.Alpha == 0.0f)
			{
				transition = false;
				fade.IsActive = false;
			}
		}

		#endregion
	}
	*/
	public static class ScreenManager
	{
		public static List<GameScreen> ActiveScreens;

		public static GameScreen CurrentScreen
		{
			get {return ActiveScreens.Last(); }
		}
		
		public static void Initialize()
		{
			ActiveScreens = new List<GameScreen>();
			ActiveScreens.Add(new SplashScreen());
		}

		public static void LoadContent(ContentManager Content, InputManager input)
		{
			foreach (GameScreen screen in ActiveScreens)
			{
				screen.LoadContent(Content, input);
			}
		}

		public static void Update(GameTime gameTime, InputManager input)
		{
			foreach (GameScreen screen in ActiveScreens) 
			{
				screen.Update(gameTime, input);
			}
		}

		public static void Draw(SpriteBatch sb)
		{
			foreach (GameScreen screen in ActiveScreens) 
			{
				screen.Draw(sb);
			}
		}


		public static void AddScreen(GameScreen newScreen)
		{
			ActiveScreens.Add(newScreen);

			//initialize, loadcontent 
		}
		public static void RemoveScreen(GameScreen newScreen)
		{
			ActiveScreens.Remove(newScreen);

			//UnloadContent
		}
	}



}