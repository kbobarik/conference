using System;
using System.Collections.Generic;
using System.Linq;
using conference.Models;
using DynamicData;
using ReactiveUI;

namespace conference.ViewModels;

public class RegistrationViewModel : ViewModelBase
{
    int numberID;
    private string fio;
    private string email;
    private string phoneNumber = "+7(___)-___-__-__";
    private string password;
    private string image;
    private string confirmPassword;
    List<Gender> genders;
    Gender selectedGender;
    List<Role> roles;
    Role selectedRole;
    List<TypeOfEvent> typeOfEvents;
    TypeOfEvent selectedTypeOfEvent;
    List<Event> events;
    bool isVisible;
    bool isAttach;

    public int NumberId
    {
        get => numberID;
        set => this.RaiseAndSetIfChanged(ref numberID, value);
    }

    public bool IsVisible
    {
        get => isVisible;
        set => this.RaiseAndSetIfChanged(ref isVisible, value);
    }

    public bool IsAttach
    {
        get => isAttach;
        set => this.RaiseAndSetIfChanged(ref isAttach, value);
    }

    public string FIO
    {
        get => fio;
        set => this.RaiseAndSetIfChanged(ref fio, value);
    }
    
    public string Image
    {
        get => image;
        set => this.RaiseAndSetIfChanged(ref image, value);
    }

    public string Email
    {
        get => email;
        set => this.RaiseAndSetIfChanged(ref email, value);
    }

    public string PhoneNumber
    {
        get => phoneNumber;
        set => this.RaiseAndSetIfChanged(ref phoneNumber, value);
    }

    public string Password
    {
        get => password;
        set => this.RaiseAndSetIfChanged(ref password, value);
    }

    public string ConfirmPassword
    {
        get => confirmPassword;
        set => this.RaiseAndSetIfChanged(ref confirmPassword, value);
    }

    public List<Role> Roles
    {
        get => roles;
        set => this.RaiseAndSetIfChanged(ref roles, value);
    }

    public Role SelectedRole
    {
        get => selectedRole;
        set => this.RaiseAndSetIfChanged(ref selectedRole, value);
    }

    public List<TypeOfEvent> TypeOfEvents
    {
        get => typeOfEvents;
        set => this.RaiseAndSetIfChanged(ref typeOfEvents, value);
    }

    public TypeOfEvent SelectedTypeOfEvent
    {
        get => selectedTypeOfEvent;
        set => this.RaiseAndSetIfChanged(ref selectedTypeOfEvent, value);
    }
    public List<Event> Events
    {
        get => events;
        set => this.RaiseAndSetIfChanged(ref events, value);
    }

    public List<Gender> Genders
    {
        get => genders;
        set => this.RaiseAndSetIfChanged(ref genders, value);
    }

    public Gender SelectedGender
    {
        get => selectedGender;
        set => this.RaiseAndSetIfChanged(ref selectedGender, value);
    }

    public RegistrationViewModel()
    {
        Random rnd = new Random();
        NumberId = rnd.Next(10000, 99999);
        Genders = Db.Genders.ToList();
        Genders.Add(new Gender(){Title = "Выберете пол", IdGender = 0});
        Genders.OrderBy(x=>x.IdGender);
        SelectedGender = Genders.First(x=>x.IdGender == 0);
        Roles = Db.Roles.ToList();
        Roles.Remove(Roles.FirstOrDefault(x => x.Title == "Организатор"));
        Roles.Remove(Roles.FirstOrDefault(x => x.Title == "Участник"));
        Roles.Add(new Role(){IdRole = 0, Title = "Выберете роль"});
        SelectedRole = Roles.FirstOrDefault(x=>x.IdRole == 0);
        TypeOfEvents = Db.TypeOfEvents.ToList();
        Events = Db.Events.ToList();
        IsVisible = false;
        IsAttach = false;
        Image = "";
    }
}