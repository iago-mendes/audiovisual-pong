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
		public ScoreModel UserScore { get; private set; }
		public ScoreModel ComputerScore { get; private set; }

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

			UserScore = new ScoreModel();
			ComputerScore = new ScoreModel();
		}

		private void ResetGameObjects() {
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

			UserScore.Reset();
			ComputerScore.Reset();
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
				CheckScores();
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

		private void CheckScores() {
			if (Ball.position.x + Ball.radius > containerDimensions.x) { // computer scores
				ComputerScore.Increment();
				Ball.MoveToCenter();
			}
			else if (Ball.position.x - Ball.radius < 0) { // user scores
				UserScore.Increment();
				Ball.MoveToCenter();
			}
		}

		public void MoveUserPaddleUp() {
			if (UserPaddle.top - UserPaddle.velocity > 0)
				UserPaddle.Move(-1 * UserPaddle.velocity);
			else if (UserPaddle.top > 0)
				UserPaddle.Move(-1 * UserPaddle.top);
		}

		public void MoveUserPaddleDown() {
			if (UserPaddle.bottom + UserPaddle.velocity < containerDimensions.y)
				UserPaddle.Move(UserPaddle.velocity);
			else if (UserPaddle.bottom < containerDimensions.y)
				UserPaddle.Move(containerDimensions.y - UserPaddle.bottom);
		}
	}
}