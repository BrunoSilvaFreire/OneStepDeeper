using Lunari.Tsuki2D.Platforming.Input;
namespace OSD {
    public class OSDPlatformerSource : InputSystemPlatformerSource {
        protected override void TransferTo(IPlatformerInput input) {
            if (input is not OSDPlatformerInput osd) {
                return;
            }
            base.TransferTo(input);
        }
    }
}