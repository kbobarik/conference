using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.Media.Imaging;
using Avalonia.Platform.Storage;
using conference.Models;
using conference.Views;
using DynamicData;
using ReactiveUI;

namespace conference.ViewModels;

public class RegistrationViewModel : ViewModelBase
{
    int numberID;
    private string fio;
    private string email;
    private string phoneNumber;
    private string password;
    private string image;
    private string info;
    private string confirmPassword;
    List<Gender> genders;
    Gender selectedGender;
    List<Role> roles;
    Role selectedRole;
    List<TypeOfEvent> typeOfEvents;
    TypeOfEvent selectedTypeOfEvent;
    List<Event> events;
    Event selectedEvent;
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
        set
        {
            this.RaiseAndSetIfChanged(ref isVisible, value);
            PasswordChar = value ? "" : "*";
        } 
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
    
    public string Info
    {
        get => info;
        set => this.RaiseAndSetIfChanged(ref info, value);
    }

    public string Email
    {
        get => email;
        set => this.RaiseAndSetIfChanged(ref email, value);
    }

    public string PhoneNumber
    {
        get => phoneNumber;
        set
        {
            if ( value == null) return;
            this.RaiseAndSetIfChanged(ref phoneNumber, value);
        }
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

    public Event SelectedEvent
    {
        get => selectedEvent;
        set => this.RaiseAndSetIfChanged(ref selectedEvent, value);
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

    private string _passwordChar;

    public string PasswordChar
    {
        get => _passwordChar;
        set => this.RaiseAndSetIfChanged(ref _passwordChar, value);
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
    public async Task SelectAndSaveImage()
    {
        if (Application.Current?.ApplicationLifetime is not IClassicDesktopStyleApplicationLifetime desktop ||
            desktop.MainWindow?.StorageProvider is not { } provider)
            throw new NullReferenceException("Missing StorageProvider instance.");

        var files = await provider.OpenFilePickerAsync(new FilePickerOpenOptions()
        {
            Title = "Выберите аватарку",
            AllowMultiple = false
        });

        if (files.Count > 0)
        {
            await using var readStream = await files[0].OpenReadAsync();
            var savePath = Path.Combine(Environment.CurrentDirectory, "Image",NumberId + ".png");
            await using var writeStream = new FileStream(savePath, FileMode.Create);
            await readStream.CopyToAsync(writeStream);
        }
        Image = NumberId+".png";
    }

    public void Cancel()
    {
        MainWindowViewModel.Self.UC = new OrganizatorView();
    }

    public void Save()
    {
        if (Password == "" || ConfirmPassword == "" || FIO == "" || SelectedGender.IdGender == 0 || PhoneNumber == "" ||
            SelectedRole.IdRole == 0 || SelectedTypeOfEvent.IdTypeOfEvent == 0)
        {
            Info = "Заполните все необходимые поля";
            return;
        }
        if (Password.Length < 6)
        {
            Info = "Пароль должен содержать больше 6 символов";
            return;
        }
        if (Password.Length < 6)
        {
            Info = "Пароль должен содержать больше 6 символов";
            return;
        }

        if (!Password.Any(char.IsUpper))
        {
            Info = "Пароль должен хотя бы одну заглавную букву";
            return;
        }
        if (!Password.Any(char.IsLower))
        {
            Info = "Пароль должен хотя бы одну прописную букву";
            return;
        }
        if (!Password.Any(char.IsDigit))
        {
            Info = "Пароль должен хотя бы одну цифру";
            return;
        }
        if (Password.All(ch => char.IsLetterOrDigit(ch)))
        {
            Info = "Пароль должен хотя бы один спецсивол";
            return;
        }
        if (Password != ConfirmPassword)
        {
            Info = "Пароли не совпадают";
            return;
        }

        string[] words = FIO.Split(" ");
        User newUser = new User()
        {
            IdUser = NumberId,
            FirstName = words[1],
            LastName = words[0],
            Patronymic = words[2],
            Email = Email,
            PhoneNumber = PhoneNumber,
            Password = Password,
            IdRoleNavigation = SelectedRole,
            IdGenderNavigation = SelectedGender,
            IdTypeOfEventsNavigation = SelectedTypeOfEvent
        };
        if (Image != "")
        {
            newUser.Image = Image;
        }
        try
        {
            Db.Users.Add(newUser);
            Db.SaveChanges();
            MainWindowViewModel.Self.UC = new OrganizatorView();
        }
        catch (Exception e)
        {
            Info = e.Message;
        }
    }
}