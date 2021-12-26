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
			radius = 25;

			position = getCenter();
			velocity = getRandomVelocity();
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

		public void MoveToCenter() {
			position = getCenter();
			velocity = getRandomVelocity();
		}

		private Dimensions getCenter() {
			Dimensions center = new Dimensions(containerDimensions.x / 2, containerDimensions.y / 2);
			return center;
		}

		private Dimensions getRandomVelocity() {
			double maxVelocity = 50;
			double minVelocity = 20;

			Random random = new Random();
			double velocityX = random.NextDouble() * (maxVelocity - minVelocity) + minVelocity;
			double velocityY = random.NextDouble() * (maxVelocity - minVelocity) + minVelocity;

			Dimensions velocity = new Dimensions(velocityX, velocityY);
			return velocity;
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