namespace audiovisual_pong.Models
{
	public abstract class PaddleModel {
		public double top { get; private set; }
		public double bottom { get; private set; }
		public double left { get; private set; }
		public double right { get; private set; }

		public PaddleModel(Dimensions leftTopCornerPosition, double width, double height) {
			top = leftTopCornerPosition.y;
			left = leftTopCornerPosition.x;

			bottom = top + height;
			right = left + width;
		}

		public void HandleCollision(BallModel ball) {
			bool isBallInside = ball.position.y+ball.radius >= top &&
				ball.position.y-ball.radius <= bottom &&
				ball.position.x-ball.radius <= right &&
				ball.position.x+ball.radius >= left;
			
			if (!isBallInside)
				return;
			
			bool isOutHorizontal = ball.position.x >= right || ball.position.x <= left;
			bool isOutVertical = ball.position.y >= bottom || ball.position.y <= top;

			if (isOutHorizontal && isOutVertical) {
				ball.setVelocityY(ball.velocity.y * -1);
				ball.setVelocityX(ball.velocity.x * -1);
			}
			else if (isOutHorizontal)
				ball.setVelocityX(ball.velocity.x * -1);
			else if (isOutVertical)
				ball.setVelocityY(ball.velocity.y * -1);
			// maybe both can be false => bug with ball movement
		}

		public void Move(double deltaY) {
			top += deltaY;
			bottom += deltaY;
		}
	}
}
