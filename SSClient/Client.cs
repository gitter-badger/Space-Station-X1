using System;
using SSCyg.Core;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace SSCyg.Client
{
	// Main class derived from Monogame's Game class. This is the overall control class,
	// all customization should be implemented in other classes through GameStates.
	public sealed class Client : Game
	{
		// The instance of the client
		private static Client _theClient = null;
		public static Client TheClient
		{
			get
			{
				if (_theClient == null)
					Debug.Throw(new NullReferenceException("Client singleton has not been initialized."));
				return _theClient;
			}
		}

		#region Members
		// Temporary graphics device reference, will move later
		public GraphicsDeviceManager Graphics { get; private set; }
		// Time delta for the current frame, might move later as well
		public GameTime Delta { get; private set; }
		#endregion

		public Client() :
			base()
		{
			_theClient = this;

			Graphics = new GraphicsDeviceManager(this);
			Graphics.PreferredBackBufferWidth = 1600;
			Graphics.PreferredBackBufferHeight = 900;
			Window.Position = new Point(160, 90);
			Window.Title = "Space Station X1";
			Graphics.ApplyChanges();
			this.IsMouseVisible = true;
		}

		protected override void Initialize()
		{
			Debug.BeginBlock("BaseInitialization");

			// TODO: Initialization stuff

			base.Initialize();
			Debug.EndBlock();
		}

		protected override void LoadContent()
		{
			Debug.BeginBlock("BaseLoadContent");

			// TODO: Content loading stuff

			base.LoadContent();
			Debug.EndBlock();
		}

		protected override void Update(GameTime gameTime)
		{
			Debug.BeginFrame();
			Debug.BeginBlock("BaseUpdate");

			// TODO: Update stuff

			base.Update(gameTime);
			Debug.EndBlock();
		}

		protected override void Draw(GameTime gameTime)
		{
			Debug.BeginBlock("BaseDraw");

			// TODO: Drawing stuff

			base.Draw(gameTime);
			Debug.EndBlock();
			Debug.EndFrame();
		}

		protected override void OnExiting(object sender, EventArgs args)
		{
			base.OnExiting(sender, args);
		}
	}
}