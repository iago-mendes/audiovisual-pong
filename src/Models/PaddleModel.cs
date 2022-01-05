namespace audiovisual_pong.Models
{
	public abstract class PaddleModel {
		public double top { get; private set; }
		public double bottom { get; private set; }
		public double left { get; private set; }
		public double right { get; private set; }

		public double velocity { get; private set; } = 25;

		public PaddleModel(Dimensions leftTopCornerPosition, double width, double height) {
			top = leftTopCornerPosition.y;
			left = leftTopCornerPosition.x;

			bottom = top + height;
			right = left + width;
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
			}
			else if (horizontalCollision)
				ball.setVelocityX(ball.velocity.x * -1);
			else if (verticalCollision)
				ball.setVelocityY(ball.velocity.y * -1);
			// maybe both can be false => bug with ball movement
		}

		public void ChangeYPosition(double deltaY) {
			top += deltaY;
			bottom += deltaY;
		}
	}
}
