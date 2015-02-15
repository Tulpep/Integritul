using Caliburn.Micro;
namespace Tulpep.Integritul.ViewModels
{
    public class ShellViewModel : Conductor<Screen>, IShell
    {
        public ShellViewModel()
        {
            base.DisplayName = "Signtul Active Directory Sync";
            ChangeScreen(new HomeViewModel());
        }

        public void ChangeScreen(Screen desiredScreen)
        {
            ActivateItem(desiredScreen);
        }
    
    }
}