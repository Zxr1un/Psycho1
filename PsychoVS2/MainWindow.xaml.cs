using PsychoVS2.Windows;
using System;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Animation;

namespace PsychoVS2
{
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            WindowState = WindowState.Maximized;
            Loaded += MainWindow_Loaded;
        }

        private async void MainWindow_Loaded(object sender, RoutedEventArgs e)
        {
            StartWavesAnimation();

            await Task.Delay(500);
            // Сначала анимируем логотип
            await AnimateLogo();

            // Затем плавно появляется вся надпись
            await AnimateTextAppear();

            // После этого запускаем плавающую анимацию букв
            StartFloatingAnimation();

            // И наконец появляется кнопка
            await AnimateButtonAppear();
        }

        private async Task AnimateLogo()
        {
            Storyboard logoStoryboard = (Storyboard)Resources["LogoAnimation"];
            logoStoryboard.Begin(LogoContainer);
            await Task.Delay(1000);
        }

        private async Task AnimateTextAppear()
        {
            Storyboard textStoryboard = (Storyboard)Resources["TextAppearAnimation"];
            textStoryboard.Begin(LettersContainer);
            await Task.Delay(1500);
        }

        private void StartWavesAnimation()
        {
            // Запускаем индивидуальные анимации для каждой волны
            Storyboard topWave1Storyboard = (Storyboard)Resources["TopWave1Animation"];
            Storyboard topWave2Storyboard = (Storyboard)Resources["TopWave2Animation"];
            Storyboard bottomWave1Storyboard = (Storyboard)Resources["BottomWave1Animation"];
            Storyboard bottomWave2Storyboard = (Storyboard)Resources["BottomWave2Animation"];

            // Запускаем анимации с небольшими задержками для разнообразия
            topWave1Storyboard.Begin(TopWave1);

            var topWave2Timer = new System.Windows.Threading.DispatcherTimer();
            topWave2Timer.Interval = TimeSpan.FromMilliseconds(500);
            topWave2Timer.Tick += (s, e) =>
            {
                topWave2Storyboard.Begin(TopWave2);
                topWave2Timer.Stop();
            };
            topWave2Timer.Start();

            var bottomWave1Timer = new System.Windows.Threading.DispatcherTimer();
            bottomWave1Timer.Interval = TimeSpan.FromMilliseconds(1000);
            bottomWave1Timer.Tick += (s, e) =>
            {
                bottomWave1Storyboard.Begin(BottomWave1);
                bottomWave1Timer.Stop();
            };
            bottomWave1Timer.Start();

            var bottomWave2Timer = new System.Windows.Threading.DispatcherTimer();
            bottomWave2Timer.Interval = TimeSpan.FromMilliseconds(1500);
            bottomWave2Timer.Tick += (s, e) =>
            {
                bottomWave2Storyboard.Begin(BottomWave2);
                bottomWave2Timer.Stop();
            };
            bottomWave2Timer.Start();
        }

        private void StartFloatingAnimation()
        {
            TextBlock[] letters = { LetterP, Letters1, Lettery, Letterc, Letterh,
                          Lettero, LetterT, Lettere, Letters2, Lettert };

            Random random = new Random();

            foreach (var letter in letters)
            {
                double duration = 2 + random.NextDouble();
                double amplitude = 3 + random.NextDouble() * 2;
                double delay = random.NextDouble() * 0.5;

                DoubleAnimation floatAnimation = new DoubleAnimation
                {
                    From = -amplitude,
                    To = amplitude,
                    Duration = TimeSpan.FromSeconds(duration),
                    AutoReverse = true,
                    RepeatBehavior = RepeatBehavior.Forever,
                    EasingFunction = new SineEase { EasingMode = EasingMode.EaseInOut }
                };

                var transform = letter.RenderTransform as TranslateTransform;
                if (transform != null)
                {
                    transform.BeginAnimation(TranslateTransform.YProperty, floatAnimation);
                }
            }
        }

        private async Task AnimateButtonAppear()
        {
            DoubleAnimation opacityAnimation = new DoubleAnimation
            {
                From = 0,
                To = 1,
                Duration = TimeSpan.FromSeconds(0.8),
                EasingFunction = new CubicEase { EasingMode = EasingMode.EaseOut }
            };

            StartButton.BeginAnimation(OpacityProperty, opacityAnimation);
            await Task.Delay(800);
        }

        private void CloseButton_Click(object sender, RoutedEventArgs e)
        {
            Application.Current.Shutdown();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            Test_choice testChoiceWindow = new Test_choice();
            testChoiceWindow.Show();
            this.Close();
        }
    }
}