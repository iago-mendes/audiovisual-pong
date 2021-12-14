namespace audiovisual_pong.Models
{
	public class BallModel
	{
		public Dimensions position { get; private set; }
		public Dimensions velocity { get; private set; }

		public BallModel(Dimensions containerDimensions) {
			position = new Dimensions(containerDimensions.x / 2, containerDimensions.y / 2);
			velocity = new Dimensions(1, 1);
		}

		public void Move() {
			position.x += velocity.x;
			position.y += velocity.y;
		}
	}

	public class Dimensions {
		public float x { get; set; }
		public float y { get; set; }

		public Dimensions(float x, float y) {
			this.x = x;
			this.y = y;
		}
	}
}