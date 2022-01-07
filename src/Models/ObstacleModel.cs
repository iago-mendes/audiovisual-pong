namespace audiovisual_pong.Models
{
	public abstract class ObstacleModel {
		public double width { get; private set; } = 100;
		public double height { get; private set; } = 150;
		public double top { get; private set; }
		public double bottom { get; private set; }
		public double left { get; private set; }
		public double right { get; private set; }
		public double speed { get; private set; } = 25;
		private double xDestination;
		public double life { get; private set; } = 2;

		public ObstacleModel(Dimensions leftTopCornerPosition, double xDestination) {
			top = leftTopCornerPosition.y;
			left = leftTopCornerPosition.x;
			bottom = top + height;
			right = left + width;

			this.xDestination = xDestination;
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

		public void Move() {
			if (xDestination > right) { // needs to go right
				if (xDestination < right + speed)
					ChangeXPosition(xDestination - right);
				else
					ChangeXPosition(speed);
			}
			else if (xDestination < left) { // needs to go left
				if (xDestination > left - speed)
					ChangeXPosition(xDestination - left);
				else
					ChangeXPosition(-1 * speed);
			}
		}
	}
}
