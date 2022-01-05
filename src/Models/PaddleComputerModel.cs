
namespace audiovisual_pong.Models
{
	public class PaddleComputerModel : PaddleModel {
		public PaddleComputerModel(Dimensions leftTopCornerPosition, double width, double height):
			base(leftTopCornerPosition, width, height){}

		public void Move(double ballHeight) {
			double center = (top + bottom) / 2;
			if (center < ballHeight)
				base.Move(velocity);
			else if (center > ballHeight)
				base.Move(-1 * velocity);
		}
	}
}
