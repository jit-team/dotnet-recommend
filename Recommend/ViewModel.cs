using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections.ObjectModel;
using Model;
using GalaSoft.MvvmLight.Command;
using GalaSoft.MvvmLight;
using System.Windows;

namespace Recommend
{
    class ViewModel : ViewModelBase
    {
        private readonly List<Movie> _movies;
        private readonly List<Tuple<int, int, float>> _trainData;

        public ObservableCollection<Movie> Movies { get; set; }

        public ObservableCollection<Movie> Recomendations { get; set; }


        public Movie SelectedMovie { get; set; }

        public int NewRank { get; set; }

        public RelayCommand AddRankCommand { get; set; }

        public Dictionary<int, int> Ranking { get; set; }

        public string Log { get; set; }

        public ViewModel()
        {
            // please adjust this to meet your system layout ...
            // obviously this should be changed to load file dialog usage :)
            try
            {
                _movies = MovieLoader.Read("u.item");
                _trainData = MovieLoader.ReadPredictions("u.data");
            }
            catch (Exception)
            {
                MessageBox.Show("Error loading data files! Please check ViewModel.cs for file location!", "Error loading files", MessageBoxButton.OK, MessageBoxImage.Error);
                Application.Current.Shutdown();
            }


            Movies = new ObservableCollection<Movie>(_movies.OrderBy(m=>m.Title));
            NewRank = 1;
            RaisePropertyChanged(() => NewRank);
            SelectedMovie = Movies.First();
            RaisePropertyChanged(() => SelectedMovie);

            Ranking = new Dictionary<int, int>();
            AddRankCommand = new RelayCommand(() => { 
                if (!Ranking.ContainsKey(SelectedMovie.Id)) {
                    Log += string.Format("{0} - rank: {1}\n", SelectedMovie, NewRank);
                    RaisePropertyChanged(() => Log);
                    Ranking.Add(SelectedMovie.Id, NewRank); 
                    Movies.Remove(SelectedMovie);
                    SelectedMovie = Movies.First();
                    RaisePropertyChanged(() => SelectedMovie);
                }
                var rec = Engine.GetRecommendations(_movies, Ranking, _trainData);
                var foo = rec.OrderByDescending(e => e.Rank);//.Where(e => !Ranking.ContainsKey(e.Id));
                Recomendations =new ObservableCollection<Movie>(foo);
                this.RaisePropertyChanged(() => Recomendations);
            });

        }
    }
}