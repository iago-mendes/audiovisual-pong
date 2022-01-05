
namespace audiovisual_pong.Models
{
	public class GameManager
	{
		public bool IsRunning { get; private set; } = false;
		public bool IsPaused { get; private set; } = false;
		public event EventHandler? MainLoopCompleted;
		public Dimensions containerDimensions;
		public BallModel Ball { get; private set; }
		public WallModel Wall { get; private set; }
		public PaddleUserModel UserPaddle { get; private set; }
		public PaddleComputerModel ComputerPaddle { get; private set; }
		public ScoreModel UserScore { get; private set; }
		public ScoreModel ComputerScore { get; private set; }
		private string userPaddleNextMove = "";
		public int TimeLeft { get; private set; } // seconds
		private int TimeTotal { get; set; } // seconds

		public GameManager(Dimensions containerDimensions, int time) {
			this.containerDimensions = containerDimensions;
			this.TimeLeft = time;
			this.TimeTotal = time;

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
			this.TimeLeft = this.TimeTotal;

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
			TimeLoop();
		}

		public void StopGame() {
			IsPaused = false;
			IsRunning = false;
			ResetGameObjects();
			MainLoopCompleted?.Invoke(this, EventArgs.Empty);
		}

		public void TogglePauseGame() {
			this.IsPaused = !this.IsPaused;
		}

		public async void MainLoop()
		{
			while(IsRunning)
			{
				if (IsPaused) {
					await Task.Delay(90); // 90 ms
					continue;
				}

				CheckScores();
				CheckCollisions();
				Ball.Move();
				ComputerPaddle.Move(Ball.position.y, containerDimensions.y);

				if (userPaddleNextMove != "") {
					UserPaddle.Move(userPaddleNextMove, containerDimensions.y);
					userPaddleNextMove = "";
				}

				MainLoopCompleted?.Invoke(this, EventArgs.Empty);
				await Task.Delay(90); // 90 ms
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
			userPaddleNextMove="up";
		}

		public void MoveUserPaddleDown() {
			userPaddleNextMove="down";
		}

		private async void TimeLoop() {
			while (IsRunning && TimeLeft > 0) {
				if (IsPaused) {
					await Task.Delay(1000); // 1 s
					continue;
				}
				
				TimeLeft--;
				await Task.Delay(1000); // 1 s
			}

			if (IsRunning)
				StopGame();
		}
	}
}