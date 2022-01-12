namespace audiovisual_pong.Models
{
	public class ObstacleModel {
		public int Id { get; private set; }
		public double top { get; private set; }
		public double bottom { get; private set; }
		public double left { get; private set; }
		public double right { get; private set; }
		public double speed { get; private set; } = 5;
		private double xDestination;
		public double life { get; private set; } = 2;

		public ObstacleModel(Dimensions leftTopCornerPosition, double xDestination, double width, double height) {
			top = leftTopCornerPosition.y;
			left = leftTopCornerPosition.x;
			bottom = top + height;
			right = left + width;

			this.xDestination = xDestination;
			
			Random random = new Random();
			this.Id = (int) (random.NextDouble() * 1000000);
		}

		public void HandleCollision(BallModel ball) {
			bool isBallInside = ball.bottom >= top &&
				ball.top <= bottom &&
				ball.left <= right &&
				ball.right >= left;
			
			if (!isBallInside)
				return;

			bool towardsTopBottom = (ball.top - ball.velocity.y >= bottom || ball.bottom - ball.velocity.y <= top);
			bool towardsSide = (ball.right - ball.velocity.x <= left || ball.left - ball.velocity.x >= right);
			
			bool cornerCollision = towardsSide && towardsTopBottom;
			bool horizontalCollision = towardsSide && !towardsTopBottom;
			bool verticalCollision = towardsTopBottom && !towardsSide;

			if (cornerCollision) {
				ball.setVelocityY(ball.velocity.y * -1);
				ball.setVelocityX(ball.velocity.x * -1);
				life--;
			}
			else if (horizontalCollision) {
				ball.setVelocityX(ball.velocity.x * -1);
				life--;
			}
			else if (verticalCollision) {
				ball.setVelocityY(ball.velocity.y * -1);
				life--;
			}
		}

		public void ChangeXPosition(double deltaX) {
			right += deltaX;
			left += deltaX;
		}

		private double getNextMove() {
			if (xDestination > right) { // needs to go right
				if (xDestination < right + speed)
					return xDestination - right;
				else
					return speed;
			}
			else if (xDestination < left) { // needs to go left
				if (xDestination > left - speed)
					return xDestination - left;
				else
					return -1 * speed;
			}

			return 0;
		}

		public void Move() {
			double nextMove = getNextMove();
			ChangeXPosition(nextMove);
		}

		public bool WillCollideWithOtherObstacles(List<ObstacleModel> obstaclesList) {
			bool willCollide = false;

			double nextMove = getNextMove();
			double nextLeft = this.left + nextMove;
			double nextRight = this.right + nextMove;

			obstaclesList.ForEach(delegate(ObstacleModel obstacle) {
				if (obstacle.Id == this.Id)
					return;
				
				bool willTopSideCollide = obstacle.top <= this.top && this.top <= obstacle.bottom;
				bool willBottomSideCollide = obstacle.top <= this.bottom && this.bottom <= obstacle.bottom;
				if (!willTopSideCollide && !willBottomSideCollide)
					return;
				
				bool willLeftSideCollide = obstacle.left <= nextLeft && nextLeft <= obstacle.right;
				bool willRightSideCollide = obstacle.left <= nextRight && nextRight <= obstacle.right;

				if (willLeftSideCollide || willRightSideCollide)
					willCollide = true;
			});

			return willCollide;
		}
	}
}
