namespace audiovisual_pong.Models
{
	public class BallModel
	{
		public float[] position { get; private set; }
		public float[] velocity { get; private set; }

		public BallModel(float[] dimensions) {
			position = new float[2] {dimensions[0] / 2, dimensions[1] /2};
			velocity = new float[2] {1, 1};
		}

		public void Move() {
			position[0] += velocity[0]; // x
			position[1] += velocity[1]; // y
		}
	}
}