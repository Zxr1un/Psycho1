using System;
using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Animation;

namespace PsychoVS2.Windows
{
    public partial class Result : Window
    {
        public Result()
        {
            InitializeComponent();
            Loaded += OnWindowLoaded;
            TestDateLabel.Content = $"Дата прохождения: {DateTime.Now:dd.MM.yyyy HH:mm}";
        }

        private void OnWindowLoaded(object sender, RoutedEventArgs e)
        {
            // Запуск анимаций при загрузке окна
            StartAnimations();
        }

        private void StartAnimations()
        {
            // Анимация появления заголовка
            var fadeIn = (Storyboard)FindResource("FadeInAnimation");
            fadeIn.Begin(TitleLabel);

            // Анимация появления блоков с результатами
            var slideIn1 = (Storyboard)FindResource("SlideInAnimation");
            Storyboard.SetTarget(slideIn1, GeneralResultsBorder);
            slideIn1.Begin();

            var slideIn2 = (Storyboard)FindResource("SlideInAnimation");
            Storyboard.SetTarget(slideIn2, DetailedResultsBorder);
            slideIn2.BeginTime = TimeSpan.FromSeconds(0.3);
            slideIn2.Begin();

            // Пульсирующая анимация для кнопки
            var pulse = (Storyboard)FindResource("PulseAnimation");
            pulse.RepeatBehavior = RepeatBehavior.Forever;
            pulse.Begin(ExitButton);

            // Анимация появления графика
            AnimateChart();
        }

        private void AnimateChart()
        {
            // Анимация столбцов графика
            var bar1Animation = new DoubleAnimation
            {
                To = 1,
                Duration = TimeSpan.FromSeconds(1),
                EasingFunction = new CubicEase { EasingMode = EasingMode.EaseOut },
                BeginTime = TimeSpan.FromSeconds(0.5)
            };

            var bar2Animation = new DoubleAnimation
            {
                To = 1,
                Duration = TimeSpan.FromSeconds(1),
                EasingFunction = new CubicEase { EasingMode = EasingMode.EaseOut },
                BeginTime = TimeSpan.FromSeconds(0.7)
            };

            var bar3Animation = new DoubleAnimation
            {
                To = 1,
                Duration = TimeSpan.FromSeconds(1),
                EasingFunction = new CubicEase { EasingMode = EasingMode.EaseOut },
                BeginTime = TimeSpan.FromSeconds(0.9)
            };

            var bar4Animation = new DoubleAnimation
            {
                To = 1,
                Duration = TimeSpan.FromSeconds(1),
                EasingFunction = new CubicEase { EasingMode = EasingMode.EaseOut },
                BeginTime = TimeSpan.FromSeconds(1.1)
            };

            Bar1.RenderTransform.BeginAnimation(ScaleTransform.ScaleYProperty, bar1Animation);
            Bar2.RenderTransform.BeginAnimation(ScaleTransform.ScaleYProperty, bar2Animation);
            Bar3.RenderTransform.BeginAnimation(ScaleTransform.ScaleYProperty, bar3Animation);
            Bar4.RenderTransform.BeginAnimation(ScaleTransform.ScaleYProperty, bar4Animation);
        }

        private void ExitButton_Click(object sender, RoutedEventArgs e)
        {
            // Анимация закрытия окна
            var closeAnimation = new DoubleAnimation
            {
                To = 0,
                Duration = TimeSpan.FromSeconds(0.3),
                EasingFunction = new CubicEase { EasingMode = EasingMode.EaseOut }
            };

            closeAnimation.Completed += (s, _) => Close();

            BeginAnimation(OpacityProperty, closeAnimation);
            Test_choice testChoiceWindow = new Test_choice();
            testChoiceWindow.Show();
            this.Close();
        }
    }
}