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
		// public FrequencyModel Freq { get; private set; }
		public PaddleUserModel UserPaddle { get; private set; }
		public PaddleComputerModel ComputerPaddle { get; private set; }
		public ScoreModel UserScore { get; private set; }
		public ScoreModel ComputerScore { get; private set; }
		private string userPaddleNextMove = "";
		public int TimeLeft { get; private set; } // seconds
		private int TimeTotal { get; set; } // seconds
		private bool areScoresOnDelay = false;
		public List<ObstacleModel> ObstacleList { get; private set; } = new List<ObstacleModel>();
		public string AudioSrc { get; private set; }

		public GameManager(Dimensions containerDimensions, string audioSrc, string audioDataUri) {
			this.containerDimensions = containerDimensions;

			this.AudioSrc = audioSrc;
			// Uri audioUri = new Uri(audioSrc);
			Console.WriteLine(audioDataUri);
			// default time
			this.TimeLeft = 1 * 60;
			this.TimeTotal = 1 * 60;

			Ball = new BallModel(containerDimensions);
			Wall = new WallModel(containerDimensions.y);
			// Freq = new FrequencyModel();

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

			ObstacleList = new List<ObstacleModel>();
		}

		public void StartGame() {
			IsRunning = true;
			IsPaused = false;

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
				RemoveDeadObstacles();

				if (!areScoresOnDelay) {
					Ball.Move();
					ComputerPaddle.Move(Ball.position.y, containerDimensions.y);
				}
				if (userPaddleNextMove != "") {
					UserPaddle.Move(userPaddleNextMove, containerDimensions.y);
					userPaddleNextMove = "";
				}
				MoveObstacles();

				MainLoopCompleted?.Invoke(this, EventArgs.Empty);
				await Task.Delay(90); // 90 ms
			}
		}

		private void CheckCollisions() {
			Wall.handleCollision(Ball);
			UserPaddle.HandleCollision(Ball);
			ComputerPaddle.HandleCollision(Ball);
			CheckObstacleCollisions();
		}

		private async void CheckScores() {
			if (areScoresOnDelay)
				return;
			
			bool hasComputerScored = Ball.position.x - Ball.radius - 10 > containerDimensions.x;
			bool hasUserScored = Ball.position.x + Ball.radius + 10 < 0;

			if (!hasComputerScored && !hasUserScored)
				return;
			
			if (hasComputerScored)
				ComputerScore.Increment();
			if (hasUserScored)
				UserScore.Increment();
			
			areScoresOnDelay = true;
			await Task.Delay(1000); // 1 s

			Ball.MoveToCenter();

			await Task.Delay(500); // 0.5 s
			areScoresOnDelay = false;
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

				int timeElapsed = TimeTotal - TimeLeft;
				if (timeElapsed % 5 == 0) // new obstacle every 15min
					SpawnObstacle();

				await Task.Delay(1000); // 1 s
			}

			if (IsRunning)
				StopGame();
		}

		private void SpawnObstacle() {
			double width = 100;
			double height = 150;

			double minXPosition = containerDimensions.x * 0.25;
			double maxXPosition = containerDimensions.x * 0.75;
			double minYPosition = 0;
			double maxYPosition = containerDimensions.y;

			double minLeftPosition = minXPosition;
			double maxLeftPosition = maxXPosition - width;
			double minTopPosition = minYPosition;
			double maxTopPosition = maxYPosition - height;

			Random random = new Random();
			ObstacleModel newObstacle;

			do
			{
				double leftPosition = (random.NextDouble() * (maxLeftPosition - minLeftPosition) + minLeftPosition);
				double topPosition = (random.NextDouble() * (maxTopPosition - minTopPosition) + minTopPosition);
				double xDestination = random.NextDouble() < 0.5 ? minXPosition : maxXPosition;

				Dimensions leftTopCornerPosition = new Dimensions(leftPosition, topPosition);
				newObstacle = new ObstacleModel(leftTopCornerPosition, xDestination, width, height);
			} while (newObstacle.WillCollideWithOtherObstacles(ObstacleList));

			ObstacleList.Add(newObstacle);
		}

		private void MoveObstacles() {
			ObstacleList.ForEach(delegate(ObstacleModel obstacle) {
				if (!obstacle.WillCollideWithOtherObstacles(ObstacleList))
					obstacle.Move();
			});
		}

		private void CheckObstacleCollisions() {
			ObstacleList.ForEach(delegate(ObstacleModel obstacle) {
				obstacle.HandleCollision(Ball);
			});
		}

		private void RemoveDeadObstacles() {
			ObstacleList.RemoveAll(delegate(ObstacleModel obstacle) {
				bool isDead = obstacle.life <= 0;
				return isDead;
			});
		}
	}
}