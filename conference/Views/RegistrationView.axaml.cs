﻿using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using conference.ViewModels;

namespace conference.Views;

public partial class RegistrationView : UserControl
{
    public RegistrationView()
    {
        InitializeComponent();
        DataContext = new RegistrationViewModel();
    }
}