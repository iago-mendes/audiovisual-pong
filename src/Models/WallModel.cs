namespace audiovisual_pong.Models
{
	public class WallModel
	{
		double top;
		double bottom;

		public WallModel(double containerHeight) {
			top = 0;
			bottom = containerHeight;
		}

		public void handleCollision(BallModel ball) {
			if (ball.top <= top || ball.bottom >= bottom)
				ball.setVelocityY(ball.velocity.y * -1);
		}
	}
}
