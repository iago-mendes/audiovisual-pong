namespace audiovisual_pong.Models
{
	public class GameManager
	{
		public bool IsRunning { get; private set; } = false;
		public event EventHandler? MainLoopCompleted;

		public float[] dimensions = {1000, 500};
		public BallModel Ball { get; private set; }

		public GameManager() {
			Ball = new BallModel(dimensions);
		}

		private void ResetGameObjects() {
			Ball = new BallModel(dimensions);
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
				Ball.Move();

				MainLoopCompleted?.Invoke(this, EventArgs.Empty);
				await Task.Delay(20); // 20 ms
			}
		}
	}
}