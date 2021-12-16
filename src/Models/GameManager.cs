namespace audiovisual_pong.Models
{
	public class GameManager
	{
		public bool IsRunning { get; private set; } = false;
		public event EventHandler? MainLoopCompleted;

		public Dimensions containerDimensions = new Dimensions(1000, 500);
		public BallModel Ball { get; private set; }
		public WallModel Wall { get; private set; }
		public PaddleUserModel UserPaddle { get; private set; }
		public PaddleComputerModel ComputerPaddle { get; private set; }

		public GameManager() {
			Ball = new BallModel(containerDimensions);
			Wall = new WallModel(containerDimensions.y);

			double paddleWidth = 50;
			double paddleHeight = 200;
			double paddleDistanceFromBorder = 10;

			Dimensions userPaddlePosition = new Dimensions(
				containerDimensions.x - paddleDistanceFromBorder - paddleWidth,
				containerDimensions.y / 2 - paddleHeight / 2
			);
			UserPaddle = new PaddleUserModel(userPaddlePosition, paddleWidth, paddleHeight);

			Dimensions computerPaddlePosition = new Dimensions(
				paddleDistanceFromBorder,
				containerDimensions.y / 2 - paddleHeight / 2
			);
			ComputerPaddle = new PaddleComputerModel(computerPaddlePosition, paddleWidth, paddleHeight);
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
				CheckCollisions();
				Ball.Move();

				MainLoopCompleted?.Invoke(this, EventArgs.Empty);
				await Task.Delay(100); // 100 ms
			}
		}

		private void CheckCollisions() {
			Wall.handleCollision(Ball);
			UserPaddle.HandleCollision(Ball);
			ComputerPaddle.HandleCollision(Ball);
		}
	}
}