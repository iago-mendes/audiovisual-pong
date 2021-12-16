namespace audiovisual_pong.Models
{
	public class BallModel
	{
		Dimensions containerDimensions;
		public Dimensions position { get; private set; }
		public Dimensions velocity { get; private set; }
		public double radius { get; private set; }

		public BallModel(Dimensions containerDimensions) {
			this.containerDimensions = containerDimensions;
			position = new Dimensions(containerDimensions.x / 2, containerDimensions.y / 2);
			velocity = new Dimensions(5, 50);
			radius = 25;
		}

		public void Move() {
			if (position.y - radius + velocity.y < 0)
				position.y = radius;
			else if (position.y + radius + velocity.y > containerDimensions.y)
				position.y = containerDimensions.y - radius;
			else
				position.y += velocity.y;
			
			position.x += velocity.x;
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