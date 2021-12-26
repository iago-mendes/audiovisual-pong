namespace audiovisual_pong.Models
{
	public class ScoreModel
	{
		public int value { get; private set; } = 0;

		public ScoreModel() {
			Reset();
		}

		public void Increment() {
			value++;
		}

		public void Reset() {
			value = 0;
		}
	}
}
