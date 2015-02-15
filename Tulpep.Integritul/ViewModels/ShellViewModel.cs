using Caliburn.Micro;
namespace Tulpep.Integritul.ViewModels
{
    public class ShellViewModel : Conductor<Screen>, IShell
    {
        public ShellViewModel()
        {
            base.DisplayName = "Tulpep Integritul";
            ChangeScreen(new HomeViewModel());
        }

        public void ChangeScreen(Screen desiredScreen)
        {
            ActivateItem(desiredScreen);
        }
    
    }
}