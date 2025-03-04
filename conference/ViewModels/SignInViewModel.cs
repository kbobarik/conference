using System;
using System.Diagnostics;
using System.Linq;
using Avalonia.Threading;
using conference.Models;
using conference.Views;
using Microsoft.EntityFrameworkCore;
using ReactiveUI;

namespace conference.ViewModels;

public class SignInViewModel:ViewModelBase
{
    DispatcherTimer _timer = new DispatcherTimer();
    DispatcherTimer _timer2 = new DispatcherTimer();
    private int timerCounter = 10;
    string username;
    string password;
    string info = "";
    private int counter = 0;
    bool isEnabled = true;
    public string Username{get=>username;set=>this.RaiseAndSetIfChanged(ref username, value);}
    public int TimerCounter{get=>timerCounter;set=>this.RaiseAndSetIfChanged(ref timerCounter, value);}
    public bool IsEnabled{get=>isEnabled;set=>this.RaiseAndSetIfChanged(ref isEnabled, value);}
    public string Password{get=>password;set=>this.RaiseAndSetIfChanged(ref password, value); }
    public string Info{get=>info;set=>this.RaiseAndSetIfChanged(ref info, value); }

    
    public void NavigateToPublicView()
    {
        MainWindowViewModel.Self.UC = new PublicView();
    }

    public void Enter()
    {
        int number;
        if (!int.TryParse(Username, out number))
        {
            Info = "Please enter a valid integer.";
            return;
        };
        User userFromBase = Db.Users.Include(user => user.IdRoleNavigation).FirstOrDefault(x=>x.Password ==Password&& x.IdUser == Convert.ToInt32(Username));
        if (userFromBase != null)
        {
            switch (userFromBase.IdRoleNavigation.Title)
            {
                case "Огранизатор":
                    MainWindowViewModel.Self.UC = new OrganizatorView();
                    break;
                case "Участник":
                    MainWindowViewModel.Self.UC = new ParticipantView();
                    break;
                case "Жюри":
                    MainWindowViewModel.Self.UC = new JureView();
                    break;
                case "Модератор":
                    MainWindowViewModel.Self.UC = new ModeratorView();
                    break;
                default:
                    break;
            }
        }
        else
        {
            
            counter ++;
            if (counter == 3)
            {
                Info = "Следущая попытка доступна через 10 секунд";
                IsEnabled = false;
                _timer.Interval = new TimeSpan(0, 0, 10);
                _timer.Tick += new EventHandler(Timer_Tick);
                _timer.Start();
            }
            else
            {
                
                Info = "Неверные данные для входа";
            }
        }
    }

    private void Timer_Tick(object sender, EventArgs e)
    {
        IsEnabled = true;
        counter = 0;
    }
}