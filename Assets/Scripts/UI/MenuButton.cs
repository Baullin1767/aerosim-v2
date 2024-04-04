namespace UI
{
    public class MenuButton : UIButton
    {
        protected override void OnClick()
        {
            MissionManager.Intro();
        }
    }
}