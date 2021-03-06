namespace audiovisual_pong.Models
{
	public class BallModel
	{
		Dimensions containerDimensions;
		public Dimensions position { get; private set; }
		public Dimensions velocity { get; private set; }
		public double radius { get; private set; }

		public double top {get; private set; }
		public double bottom {get; private set; }
		public double left {get; private set; }
		public double right {get; private set; }

		public BallModel(Dimensions containerDimensions) {
			this.containerDimensions = containerDimensions;
			radius = 25;
			position = getCenter();
			setSides();
			velocity = getRandomVelocity();
		}

		public void Move() {
			if (top + velocity.y < 0)
				position.y = radius;
			else if (position.y + radius + velocity.y > containerDimensions.y)
				position.y = containerDimensions.y - radius;
			else
				position.y += velocity.y;
			position.x += velocity.x;
			setSides();
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

		private void setSides() {
			top = position.y - radius;
			bottom = position.y + radius;
			left = position.x - radius;
			right = position.x + radius;
		}

		private Dimensions getRandomVelocity() {
			double maxVelocity = 50;
			double minVelocity = 20;

			Random random = new Random();
			int signX = random.NextDouble() < 0.5 ? -1 : 1;
			int signY = random.NextDouble() < 0.5 ? -1 : 1;
			double velocityX = (random.NextDouble() * (maxVelocity - minVelocity) + minVelocity) * signX;
			double velocityY = (random.NextDouble() * (maxVelocity - minVelocity) + minVelocity) * signY;

			Dimensions velocity = new Dimensions(velocityX, velocityY);
			return velocity;
		}
	}
}
