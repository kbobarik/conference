using System;
using System.Collections.Generic;
using System.Linq;
using conference.Models;
using Microsoft.EntityFrameworkCore;
using ReactiveUI;

namespace conference.ViewModels;

public class PublicViewModel : ViewModelBase
{
    List<Event> events;

    public List<Event> Events
    {
        get => events;
        set => this.RaiseAndSetIfChanged(ref events, value);
    }

    List<Event> events0;

    public List<Event> Events0
    {
        get => events0;
        set => this.RaiseAndSetIfChanged(ref events0, value);
    }

    List<TypeOfEvent> typeOfEvents;
    TypeOfEvent selectedEvent;
    Boolean isVisible;

    public Boolean IsVisible
    {
        get => isVisible;
        set => this.RaiseAndSetIfChanged(ref isVisible, value);
    }

    public TypeOfEvent SelectedEvent
    {
        get => selectedEvent;
        set
        {
            this.RaiseAndSetIfChanged(ref selectedEvent, value);
            Search();
        }
    }

    public List<TypeOfEvent> TypeOfEvents
    {
        set => this.RaiseAndSetIfChanged(ref typeOfEvents, value);
        get => typeOfEvents;
    }

    List<string> dateEvents;

    public List<string> DateEvents
    {
        get => dateEvents;
        set => this.RaiseAndSetIfChanged(ref dateEvents, value);
    }
    string selectedDate;

    public string SelectedDate
    {
        get => selectedDate;
        set
        {
            this.RaiseAndSetIfChanged(ref selectedDate, value);
            Search();
        }
    }


    public PublicViewModel()
    {
        Events0 = Db.Events.Include(x => x.IdTypeOfEventNavigation).ToList();
        Events = Events0;
        TypeOfEvents = Db.TypeOfEvents.ToList();
        TypeOfEvents.Add(new TypeOfEvent() { IdTypeOfEvent = 0, Title = "Выберете тип мероприятия" });
        TypeOfEvents = TypeOfEvents.OrderBy(x => x.IdTypeOfEvent).ToList();
        SelectedEvent = TypeOfEvents.FirstOrDefault(x => x.IdTypeOfEvent == 0);
        IsVisible = false;
        DateEvents = Events0.Select(x=>x.Date.ToString()).Distinct().Order().ToList();
        DateEvents.Insert(0,"Выберете дату");
        SelectedDate = DateEvents[0];
    }

    private void Search()
    {
        Events = Events0;
        if (SelectedEvent.IdTypeOfEvent != 0 && SelectedDate!="Выберете дату")
        {
            Events = Events0.Where(x => x.IdTypeOfEvent == SelectedEvent.IdTypeOfEvent&&x.Date.ToString() == SelectedDate).ToList();
        }
        else
        {
            if (SelectedEvent.IdTypeOfEvent != 0)
            {
                Events = Events0.Where(x => x.IdTypeOfEvent == SelectedEvent.IdTypeOfEvent).ToList();
            }

            if (SelectedDate != "Выберете дату")
            {
                Events = Events0.Where(x=>x.Date.ToString() == SelectedDate).ToList();
            }
        }
       
        IsVisible = Events.Count == 0;
    }

    public void NavigationToSignIn()
    {
        MainWindowViewModel.Self.UC = new SignInView();
    }
        
}