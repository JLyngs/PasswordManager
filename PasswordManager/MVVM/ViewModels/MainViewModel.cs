using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PasswordManager.Repositories;
using PasswordManager.MVVM.Model;
using System.Windows;
using FontAwesome.Sharp;
using System.Windows.Input;

namespace PasswordManager.MVVM.ViewModels
{
    public class MainViewModel : ViewModelBase
    {
        //Fields
        private UserAccountModel _currentUserAccount;
        private ViewModelBase _currentChildView;
        private string _caption;
        private IconChar _icon;


        private IUserRepository userRepository;

        public UserAccountModel CurrentUserAccount
        {
            get
            {
                return _currentUserAccount;
            }

            set
            {
                _currentUserAccount = value;
                OnPropertyChanged(nameof(CurrentUserAccount));
            }
        }

        public ViewModelBase CurrentChildView
        {
            get
            {
                return _currentChildView;
            }
            set
            {
                _currentChildView = value;
                OnPropertyChanged(nameof(CurrentChildView));
            }
        }

        public string Caption
        {
            get
            {
                return _caption;
            }
            set
            {
                _caption = value;
                OnPropertyChanged(nameof(Caption));
            }
        }

        public IconChar Icon
        {
            get
            {
                return _icon;
            }
            set
            {
                _icon = value;
                OnPropertyChanged(nameof(Icon));
            }
        }

        //--> Commands
        public ICommand ShowDashBoardViewCommand { get; }
        public ICommand ShowPasswordsViewCommand { get; }
        public ICommand ShowUserSettingsViewCommand { get; }
        public ICommand ShowUserAccountViewCommand { get; }

        

        public MainViewModel()
        {
            userRepository = new UserRepository();
            CurrentUserAccount = new UserAccountModel();

            //Initialize commands
            ShowDashBoardViewCommand = new ViewModelCommand(ExecuteShowDashBoardViewCommand);
            ShowPasswordsViewCommand = new ViewModelCommand(ExecuteShowPasswordsViewCommand);
            ShowUserSettingsViewCommand = new ViewModelCommand(ExecuteShowUserSettingsViewCommand);
            ShowUserAccountViewCommand = new ViewModelCommand(ExecuteShowUserAccountViewCommand);

            //Default View
            ExecuteShowDashBoardViewCommand(null);

            LoadCurrentUserData();
        }

        private void ExecuteShowUserAccountViewCommand(object obj)
        {
            CurrentChildView = new UserAccountViewModel();
            Caption = "User Account";
            Icon = IconChar.UserAlt;
        }

        private void ExecuteShowUserSettingsViewCommand(object obj)
        {
            CurrentChildView = new UserSettingsViewModel();
            Caption = "User Settings";
            Icon = IconChar.Cog;
        }

        private void ExecuteShowPasswordsViewCommand(object obj)
        {
            CurrentChildView = new UserPasswordsViewModel();
            Caption = "User Passwords";
            Icon = IconChar.Key;
        }

        private void ExecuteShowDashBoardViewCommand(object obj)
        {
            CurrentChildView = new DashBoardViewModel();
            Caption = "Dashboard";
            Icon = IconChar.Home;
        }

        private void LoadCurrentUserData()
        {
            var user = userRepository.GetByUsername(Thread.CurrentPrincipal.Identity.Name);
            if(user != null)
            {
                CurrentUserAccount.Username = user.Username;
                CurrentUserAccount.DisplayName = $"{user.Name} {user.LastName}";
                CurrentUserAccount.ProfilePicture = null;
            }
            else
            {
                CurrentUserAccount.DisplayName="Invalid user, not logged in";
                //Hide child views.
            }
        }
    }
}
