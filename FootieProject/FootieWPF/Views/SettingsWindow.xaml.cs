using DAO.Repos.Implementations;
using DAO.Services;
using FootieWPF.ViewModels;
using System.Windows;
using System.Windows.Input;

namespace FootieWPF.Views
{
    public partial class SettingsWindow : Window
    {
        public SettingsViewModel ViewModel { get; private set; }

        // konstrukor za settings window
        public SettingsWindow()
        {
            InitializeComponent();
            ViewModel = new SettingsViewModel(new FileRepository(), API.Instance);
            DataContext = ViewModel;
        }

        // metoda za omogućavanje shortcutova na tipkama enter i escape
        private void Window_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                if (ViewModel.ApplyCommand.CanExecute(null))
                {
                    ViewModel.ApplyCommand.Execute(null);
                }
            }
            else if (e.Key == Key.Escape)
            {
                if (ViewModel.CancelCommand.CanExecute(null))
                {
                    ViewModel.CancelCommand.Execute(null);
                }
            }
        }
    }
}
