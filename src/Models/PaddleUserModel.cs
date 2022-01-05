
namespace audiovisual_pong.Models
{
	public class PaddleUserModel : PaddleModel {
		public PaddleUserModel(Dimensions leftTopCornerPosition, double width, double height):
			base(leftTopCornerPosition, width, height){}
		
		public void Move(string direction, double containerHeight) {
			if (direction == "up") {
				if (base.top - base.velocity > 0)
					base.ChangeYPosition(-1 * base.velocity);
				else if (base.top > 0)
					base.ChangeYPosition(-1 * base.top);
			}
			else if (direction == "down") {
				if (base.bottom + base.velocity < containerHeight)
					base.ChangeYPosition(base.velocity);
				else if (base.bottom < containerHeight)
					base.ChangeYPosition(containerHeight - base.bottom);
			}
		}
	}
}
