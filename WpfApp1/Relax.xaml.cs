using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Shapes;
using System.Windows.Threading;

namespace WpfApp1
{
    public partial class Relax : UserControl
    {
        private DispatcherTimer gameTimer;
        private List<Ellipse> bubbles;
        private Random random;
        private int bubbleScore;
        private DispatcherTimer clickTimer;
        private int clickCount;
        private int highScore;
        private bool isClickGameActive;
        private int timeLeft;

        public Relax()
        {
            InitializeComponent();
            InitializeGames();
        }

        private void InitializeGames()
        {
            bubbles = new List<Ellipse>();
            random = new Random();
            bubbleScore = 0;
            clickCount = 0;
            highScore = 0;
            ScoreText.Text = "Score: 0";
            HighScoreText.Text = "Kỉ lục: 0";
            timeLeft = 10; // Initialize time left

            // Initialize bubble game timer
            gameTimer = new DispatcherTimer();
            gameTimer.Interval = TimeSpan.FromMilliseconds(50);
            gameTimer.Tick += GameTimer_Tick;

            // Initialize click game timer
            clickTimer = new DispatcherTimer();
            clickTimer.Interval = TimeSpan.FromSeconds(1);
            clickTimer.Tick += ClickTimer_Tick;

            // Start with bubble game by default
            BubbleGame_Click(null, null);
        }

        private void BubbleGame_Click(object sender, RoutedEventArgs e)
        {
            GameCanvas.Visibility = Visibility.Visible;
            ClickGamePanel.Visibility = Visibility.Collapsed;
            isClickGameActive = false;
            ScoreText.Text = "Score: " + bubbleScore;
            gameTimer.Start();
            DispatcherTimer spawnTimer = new DispatcherTimer();
            spawnTimer.Interval = TimeSpan.FromMilliseconds(1000);
            spawnTimer.Tick += (s, e) => SpawnBubble();
            spawnTimer.Start();
        }

        private void ClickGame_Click(object sender, RoutedEventArgs e)
        {
            GameCanvas.Visibility = Visibility.Collapsed;
            ClickGamePanel.Visibility = Visibility.Visible;
            isClickGameActive = true;
            clickCount = 0;
            timeLeft = 10; // Reset timer
            ClickCountText.Text = "Số lần click: 0";
            ClickTimerText.Text = "Thời gian: " + timeLeft + "s";
            clickTimer.Start();
            ClickButton.IsEnabled = true; // Ensure button is enabled at start
        }

        private void SpawnBubble()
        {
            Ellipse bubble = new Ellipse
            {
                Width = random.Next(20, 50),
                Height = random.Next(20, 50),
                Fill = new SolidColorBrush(Color.FromRgb(
                    (byte)random.Next(256),
                    (byte)random.Next(256),
                    (byte)random.Next(256)
                ))
            };
            Canvas.SetLeft(bubble, random.NextDouble() * (GameCanvas.ActualWidth - bubble.Width));
            Canvas.SetTop(bubble, GameCanvas.ActualHeight - bubble.Height);
            GameCanvas.Children.Add(bubble);
            bubbles.Add(bubble);
        }

        private void GameTimer_Tick(object sender, EventArgs e)
        {
            for (int i = bubbles.Count - 1; i >= 0; i--)
            {
                Ellipse bubble = bubbles[i];
                double top = Canvas.GetTop(bubble);
                top -= 2;
                if (top < -bubble.Height)
                {
                    GameCanvas.Children.Remove(bubble);
                    bubbles.RemoveAt(i);
                }
                else
                {
                    Canvas.SetTop(bubble, top);
                }
            }
        }

        private void GameCanvas_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            if (!isClickGameActive)
            {
                Point clickPoint = e.GetPosition(GameCanvas);
                for (int i = bubbles.Count - 1; i >= 0; i--)
                {
                    Ellipse bubble = bubbles[i];
                    double left = Canvas.GetLeft(bubble);
                    double top = Canvas.GetTop(bubble);
                    double right = left + bubble.Width;
                    double bottom = top + bubble.Height;
                    if (clickPoint.X >= left && clickPoint.X <= right &&
                        clickPoint.Y >= top && clickPoint.Y <= bottom)
                    {
                        GameCanvas.Children.Remove(bubble);
                        bubbles.RemoveAt(i);
                        bubbleScore += 10;
                        ScoreText.Text = "Score: " + bubbleScore;
                        break;
                    }
                }
            }
        }

        private void ClickButton_Click(object sender, RoutedEventArgs e)
        {
            if (isClickGameActive)
            {
                clickCount++;
                ClickCountText.Text = "Số lần click: " + clickCount;
            }
        }

        private void ClickTimer_Tick(object sender, EventArgs e)
        {
            timeLeft--;
            if (timeLeft > 0)
            {
                ClickTimerText.Text = "Thời gian: " + timeLeft + "s";
            }
            else
            {
                clickTimer.Stop();
                if (clickCount > highScore)
                {
                    highScore = clickCount;
                    HighScoreText.Text = "Kỉ lục: " + highScore;
                }
                ClickTimerText.Text = "Thời gian: 0s";
                ClickButton.IsEnabled = false;
            }
        }
    }
}