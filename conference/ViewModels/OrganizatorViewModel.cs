using System;
using conference.Models;
using conference.Views;
using ReactiveUI;

namespace conference.ViewModels;


public class OrganizatorViewModel:ViewModelBase
{
    public string welcome;
    User organizator;
    private string genderOrganizator;
    public User Organizator{ get=>organizator; set => this.RaiseAndSetIfChanged(ref organizator, value); }
    public string Welcome
    {
        get=> welcome;
        set =>this.RaiseAndSetIfChanged(ref welcome,value);
    }
    public string GenderOrganizator
    {
        get=> genderOrganizator;
        set =>this.RaiseAndSetIfChanged(ref genderOrganizator,value);
    }

    public OrganizatorViewModel()
    {
        Organizator = MainWindowViewModel.Self.LoginedUser;
        Welcome = DateTime.Now.Hour switch
        {
            >= 9 and <= 11 => "Доброе утро!",
            > 11 and <= 18 => "Добрый день!",
            > 18 and <= 24 => "Добрый вечер!",
            _ => "Доброй ночи!"
        };
        GenderOrganizator = Organizator.IdGenderNavigation.Title == "Женский" ? "Mrs " : "Ms ";
    }

    public void NavigateToRegistration()
    {
        MainWindowViewModel.Self.UC = new RegistrationView();
    }
    
}