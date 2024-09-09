using DAO.Models;
using DAO.Repos.Implementations;
using DAO.Repos.Interfaces;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace FootieForms
{
    public partial class PlayerData : UserControl
    {
        private readonly IFileRepository _fileRepository;
        public Player Player { get; private set; }
        public bool IsFavorite { get; private set; }
        private string _playerImagePath;

        // konstruktor za karticu igrača koji prima odgovarajućeg igrača, podatak o tome je li favorit te file repository za spremanje slika igrača u file
        public PlayerData(Player player, bool isFavorite, IFileRepository fileRepository)
        {
            InitializeComponent();
            _fileRepository = fileRepository;
            LoadPlayerData(player, isFavorite);
        }

        // metoda za učitavanje podataka svih o igraču u njegovu karticu te rutinske provjere
        public void LoadPlayerData(Player player, bool isFavorite)
        {
            Player = player;
            IsFavorite = isFavorite;

            lblPlayerName.Text = player.Name;
            lblShirtNumber.Text = player.ShirtNumber.ToString();
            lblPosition.Text = player.Position;
            lblCaptain.Text = player.Captain ? "Yes" : "No";

            _playerImagePath = _fileRepository.LoadPlayerImagePath(player.Name);
            if (!string.IsNullOrEmpty(_playerImagePath) && File.Exists(_playerImagePath))
            {
                pbPlayerPicture.Image = Image.FromFile(_playerImagePath);
            }
            else
            {
                pbPlayerPicture.Image = Properties.Resources.WPW_2021_BahrainGP_4; 
            }

            pbStarIcon.Visible = isFavorite;
        }

        // metoda koja otvara prozor za odabiranje slika igrača pritiskom na gumb te rutinske provjere kao i ograničenje na odabir samo fileova sa ekstenzijama za slike
        private void btnChooseImage_Click(object sender, EventArgs e)
        {
            using (OpenFileDialog openFileDialog = new OpenFileDialog())
            {
                openFileDialog.Filter = "Image Files|*.jpg;*.jpeg;*.png;*.bmp";
                if (openFileDialog.ShowDialog() == DialogResult.OK)
                {
                    _playerImagePath = openFileDialog.FileName;
                    pbPlayerPicture.Image = Image.FromFile(_playerImagePath);
                    _fileRepository.SavePlayerImagePath(Player.Name, _playerImagePath);
                }
            }
        }

        private void PlayerData_Load(object sender, EventArgs e)
        {
        }

    }
}
