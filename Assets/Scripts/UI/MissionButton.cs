namespace UI
{
    public class MissionButton : UIButton
    {
        protected override void OnClick()
        {
            MissionManager.Mission();
        }
    }
}