
namespace audiovisual_pong.Models
{
	public class PaddleComputerModel : PaddleModel {
		public PaddleComputerModel(Dimensions leftTopCornerPosition, double width, double height):
			base(leftTopCornerPosition, width, height){}

		public void Move(double ballYPosition, double containerHeight) {
			double center = (base.top + base.bottom) / 2;
			double velocity = base.velocity / 2;

			if (center < ballYPosition) {
				if (base.bottom + velocity < containerHeight)
					base.ChangeYPosition(velocity);
				else if (base.bottom < containerHeight)
					base.ChangeYPosition(containerHeight - base.bottom);
			}
			else if (center > ballYPosition) {
				if (base.top - velocity > 0)
					base.ChangeYPosition(-1 * velocity);
				else if (base.top > 0)
					base.ChangeYPosition(-1 * base.top);
			}
		}
	}
}
