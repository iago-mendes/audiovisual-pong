namespace audiovisual_pong.Models
{
	public class GameManager
	{
		public bool IsRunning { get; private set; } = false;
		public event EventHandler? MainLoopCompleted;

		public Dimensions containerDimensions = new Dimensions(1000, 500);
		public BallModel Ball { get; private set; }
		public WallModel Wall { get; private set; }

		public GameManager() {
			Ball = new BallModel(containerDimensions);
			Wall = new WallModel(containerDimensions.y);
		}

		private void ResetGameObjects() {
			Ball = new BallModel(containerDimensions);
		}

		public void StartGame() {
			IsRunning = true;
			MainLoop();
		}

		public void StopGame() {
			IsRunning = false;
			ResetGameObjects();
		}

		public async void MainLoop()
		{
			while(IsRunning)
			{
				Wall.handleCollision(Ball); //put inside check function
				Ball.Move();

				MainLoopCompleted?.Invoke(this, EventArgs.Empty);
				await Task.Delay(100); // 100 ms
			}
		}
	}
}