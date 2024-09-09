﻿using FootieWPF.ViewModels;
using System.Windows;

namespace FootieWPF.Views
{
    public partial class PlayerCardWindow : Window
    {
        // konstruktor koji prima view model te time dobiva podatke spremne za prikaz na UI
        public PlayerCardWindow(PlayerCardViewModel viewModel)
        {
            InitializeComponent();
            DataContext = viewModel;  
            AnimateWindow();
        }

        // metoda za animiranje prilikom učitavanja windowa
        private void AnimateWindow()
        {
            this.Opacity = 0;
            var fadeIn = new System.Windows.Media.Animation.DoubleAnimation(1, TimeSpan.FromSeconds(0.3));
            this.BeginAnimation(Window.OpacityProperty, fadeIn);
        }
    }
}
