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
			if (ball.position.y-ball.radius <= top || ball.position.y+ball.radius >= bottom)
				ball.setVelocityY(ball.velocity.y * -1);
		}
	}
}
