using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Threading;

namespace VideoPlayer
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private DispatcherTimer _timer;
        private Random _rand;

        private int _minimumTimeInSecondsForInterval = 0,
            _maximumTimeInSecondsForInterval = 0;
        private List<string> _videoList = null;

        public MainWindow()
        {
            InitializeComponent();
            this.KeyUp += MainWindow_KeyUp;
            this.Loaded += MainWindow_Loaded;
            this.VideoScreen.MediaEnded += VideoScreen_MediaEnded;
        }

        private void VideoScreen_MediaEnded(object sender, RoutedEventArgs e)
        {
            this._timer.Interval = new TimeSpan(0, 0, _rand.Next(this._minimumTimeInSecondsForInterval,
                this._maximumTimeInSecondsForInterval));
            this._timer.Start(); 
        }

        private void MainWindow_Loaded(object sender, RoutedEventArgs e)
        {
            this._minimumTimeInSecondsForInterval =
                int.Parse(ConfigurationManager.AppSettings["MinimumBlackInSeconds"]);
            this._maximumTimeInSecondsForInterval =
                int.Parse(ConfigurationManager.AppSettings["MaximumBlackInSeconds"]);

            string videoDirectory = ConfigurationManager.AppSettings["VideoDirectory"];
            this._videoList = new List<string>();
            foreach (string videoFile in Directory.GetFiles(videoDirectory))
            {
                _videoList.Add(videoFile);
            }

            _rand = new Random();

            _timer = new DispatcherTimer();
            _timer.Tick += _timer_Tick;
            _timer.Interval = new TimeSpan(0, 0, _rand.Next(this._minimumTimeInSecondsForInterval,
                this._maximumTimeInSecondsForInterval));
            _timer.Start();
        }

        private void _timer_Tick(object sender, EventArgs e)
        {
            this._timer.Stop();
            string videoToPlay = this._videoList[_rand.Next(0, this._videoList.Count - 1)];
            this.VideoScreen.Source = new Uri(videoToPlay);
        }

        private void MainWindow_KeyUp(object sender, KeyEventArgs e)
        {
            this.Close();
        }
    }
}
