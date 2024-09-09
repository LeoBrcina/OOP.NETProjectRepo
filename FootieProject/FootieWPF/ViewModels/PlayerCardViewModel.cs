using System.ComponentModel;
using System.IO;
using DAO.Repos.Implementations;
using DAO.Services;

namespace FootieWPF.ViewModels
{
    public class PlayerCardViewModel : INotifyPropertyChanged
    {
        private string _name;
        private long _shirtNumber;
        private string _position;
        private int _goals;
        private int _yellowCards;
        private string _imagePath;

        private readonly FileRepository _fileRepository;

        private const string DefaultImagePath = "pack://application:,,,/Resources/WPW_2021_BahrainGP_4.png";

        // konstruktor koji prima sve potrebne podatke za prikazivanje igračeve kartice, slike i sl.
        public PlayerCardViewModel(string name, long shirtNumber, string position, int goals, int yellowCards, FileRepository fileRepository)
        {
            _name = name;
            _shirtNumber = shirtNumber;
            _position = position;
            _goals = goals;
            _yellowCards = yellowCards;
            _fileRepository = fileRepository;

            LoadImage(name);
        }

        public string Name
        {
            get => _name;
            set { _name = value; OnPropertyChanged(nameof(Name)); }
        }

        public long ShirtNumber
        {
            get => _shirtNumber;
            set { _shirtNumber = value; OnPropertyChanged(nameof(ShirtNumber)); }
        }

        public string Position
        {
            get => _position;
            set { _position = value; OnPropertyChanged(nameof(Position)); }
        }

        public int Goals
        {
            get => _goals;
            set { _goals = value; OnPropertyChanged(nameof(Goals)); }
        }

        public int YellowCards
        {
            get => _yellowCards;
            set { _yellowCards = value; OnPropertyChanged(nameof(YellowCards)); }
        }

        public string ImagePath
        {
            get => _imagePath;
            set { _imagePath = value; OnPropertyChanged(nameof(ImagePath)); }
        }

        // metoda  koja uz rutinske provjere učitava igračevu sliku iz file-a te ju postavlja na karticu, u suprotnom postavlja defaultnu sliku
        public void LoadImage(string playerName)
        {
            var imagePath = _fileRepository.LoadPlayerImagePath(playerName);

            if (!File.Exists(imagePath))
            {
                ImagePath = DefaultImagePath;
            }
            else
            {
                ImagePath = imagePath;
            }
        }

        // event i metoda koja se okida na svaki event promjene
        public event PropertyChangedEventHandler PropertyChanged;

        protected void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
