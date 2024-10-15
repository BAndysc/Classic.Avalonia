using System.Collections.Generic;
using System.Collections.ObjectModel;
using Avalonia.Markup.Xaml.MarkupExtensions;
using Avalonia.Media;

namespace Classic.Demo;

public class MainWindowViewModel
{
    public ObservableCollection<PersonViewModel> People { get; }

    public MainWindowViewModel()
    {
        var people = new List<PersonViewModel>
        {
            new PersonViewModel("Neil", "Armstrong"),
            new PersonViewModel("Buzz", "Lightyear") {Selected = true},
            new PersonViewModel("James", "Kirk")
        };
        People = new ObservableCollection<PersonViewModel>(people);
    }
}