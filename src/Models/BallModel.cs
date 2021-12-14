namespace audiovisual_pong.Models
{
	public class BallModel
	{
		public Dimensions position { get; private set; }
		public Dimensions velocity { get; private set; }
		public double radius { get; private set; }

		public BallModel(Dimensions containerDimensions) {
			position = new Dimensions(containerDimensions.x / 2, containerDimensions.y / 2);
			velocity = new Dimensions(0, 25);
			radius = 25;
		}

		public void Move() {
			position.x += velocity.x;
			position.y += velocity.y;
		}

		public void setVelocityY(double y) {
			velocity.y = y;
		}

		public void setVelocityX(double x) {
			velocity.x = x;
		}
	}

	public class Dimensions {
		public double x { get; set; }
		public double y { get; set; }

		public Dimensions(double x, double y) {
			this.x = x;
			this.y = y;
		}
	}
}