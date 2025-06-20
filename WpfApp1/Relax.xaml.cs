using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Shapes;
using System.Windows.Threading;
using System.Windows.Controls.Primitives;

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
        private DispatcherTimer memoryTimer; // Used for 0.2s delay
        private List<Button> memoryCards;
        private List<int> cardValues;
        private int firstCardIndex = -1;
        private int secondCardIndex = -1;
        private int matchesFound;

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
            timeLeft = 10;

            gameTimer = new DispatcherTimer();
            gameTimer.Interval = TimeSpan.FromMilliseconds(50);
            gameTimer.Tick += GameTimer_Tick;

            clickTimer = new DispatcherTimer();
            clickTimer.Interval = TimeSpan.FromSeconds(1);
            clickTimer.Tick += ClickTimer_Tick;

            memoryTimer = new DispatcherTimer();
            memoryTimer.Interval = TimeSpan.FromMilliseconds(500); // 0.2 seconds
            memoryTimer.Tick += MemoryTimer_Tick;

            BubbleGame_Click(null, null);
        }

        private void BubbleGame_Click(object sender, RoutedEventArgs e)
        {
            GameCanvas.Visibility = Visibility.Visible;
            ClickGamePanel.Visibility = Visibility.Collapsed;
            MemoryGamePanel.Visibility = Visibility.Collapsed;
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
            MemoryGamePanel.Visibility = Visibility.Collapsed;
            isClickGameActive = true;
            clickCount = 0;
            timeLeft = 10;
            ClickCountText.Text = "Số lần click: 0";
            ClickTimerText.Text = "Thời gian: " + timeLeft + "s";
            clickTimer.Start();
            ClickButton.IsEnabled = true;
        }

        private void MemoryGame_Click(object sender, RoutedEventArgs e)
        {
            GameCanvas.Visibility = Visibility.Collapsed;
            ClickGamePanel.Visibility = Visibility.Collapsed;
            MemoryGamePanel.Visibility = Visibility.Visible;
            isClickGameActive = false;
            InitializeMemoryGame();
        }

        private void InitializeMemoryGame()
        {
            MemoryGrid.Children.Clear();
            memoryCards = new List<Button>();
            cardValues = new List<int>();
            matchesFound = 0;
            MemoryStatusText.Text = "Tìm cặp hình!";

            List<int> values = new List<int> { 1, 1, 2, 2, 3, 3, 4, 4, 5, 5, 6, 6, 7, 7, 8, 8 };
            values = values.OrderBy(x => random.Next()).ToList();

            for (int i = 0; i < 16; i++)
            {
                Button card = new Button
                {
                    Width = 80,
                    Height = 80,
                    Content = "?", // Hidden face
                    Tag = values[i],
                    Background = new LinearGradientBrush
                    {
                        StartPoint = new Point(0, 0),
                        EndPoint = new Point(1, 1),
                        GradientStops = new GradientStopCollection
                        {
                            new GradientStop(Colors.LightGray, 0),
                            new GradientStop(Colors.DarkGray, 1)
                        }
                    },
                    Foreground = Brushes.Black,
                    FontWeight = FontWeights.Bold,
                    BorderBrush = Brushes.Gray,
                    BorderThickness = new Thickness(1)
                };
                card.Click += Card_Click;
                MemoryGrid.Children.Add(card);
                memoryCards.Add(card);
            }
        }

        private void Card_Click(object sender, RoutedEventArgs e)
        {
            if (firstCardIndex != -1 && secondCardIndex != -1) return;

            Button card = sender as Button;
            int index = memoryCards.IndexOf(card);

            if (firstCardIndex == -1)
            {
                firstCardIndex = index;
                FlipCard(card, true); // Flip to face-up
            }
            else if (secondCardIndex == -1 && index != firstCardIndex)
            {
                secondCardIndex = index;
                FlipCard(card, true); // Flip second card to face-up
                memoryTimer.Start(); // Start 0.2s timer to check match
            }
        }

        private void CheckMatch()
        {
            if (memoryCards[firstCardIndex].Tag.Equals(memoryCards[secondCardIndex].Tag))
            {
                matchesFound++;
                MemoryStatusText.Text = $"Tìm cặp hình! ({matchesFound}/8 cặp)";
                if (matchesFound == 8)
                {
                    MemoryStatusText.Text = "Hoàn thành! Nhấn để chơi lại.";
                }
            }
            else
            {
                // Flip back mismatched cards after timer
                FlipCard(memoryCards[firstCardIndex], false);
                FlipCard(memoryCards[secondCardIndex], false);
            }
            firstCardIndex = -1;
            secondCardIndex = -1;
        }

        private void FlipCard(Button card, bool isFaceUp)
        {
            if (isFaceUp)
            {
                card.Content = card.Tag;
                card.Background = new LinearGradientBrush
                {
                    StartPoint = new Point(0, 0),
                    EndPoint = new Point(1, 1),
                    GradientStops = new GradientStopCollection
                    {
                        new GradientStop(Colors.LightCoral, 0),
                        new GradientStop(Colors.Salmon, 1)
                    }
                };
                card.BorderBrush = Brushes.DarkOrange;
                card.BorderThickness = new Thickness(2);
                card.Effect = new System.Windows.Media.Effects.DropShadowEffect
                {
                    Color = Colors.Black,
                    Direction = 320,
                    ShadowDepth = 3,
                    Opacity = 0.5
                };
                card.Foreground = Brushes.White;
            }
            else
            {
                card.Content = "?";
                card.Background = new LinearGradientBrush
                {
                    StartPoint = new Point(0, 0),
                    EndPoint = new Point(1, 1),
                    GradientStops = new GradientStopCollection
                    {
                        new GradientStop(Colors.LightGray, 0),
                        new GradientStop(Colors.DarkGray, 1)
                    }
                };
                card.BorderBrush = Brushes.Gray;
                card.BorderThickness = new Thickness(1);
                card.Effect = null; // Remove shadow for face-down
                card.Foreground = Brushes.Black;
            }
        }

        private void MemoryTimer_Tick(object sender, EventArgs e)
        {
            memoryTimer.Stop();
            CheckMatch(); // Perform match check after 0.2s
        }

        private void SpawnBubble()
        {
            Ellipse bubble = new Ellipse
            {
                Width = random.Next(20, 50),
                Height = random.Next(20, 50),
                Fill = new LinearGradientBrush
                {
                    StartPoint = new Point(0, 0),
                    EndPoint = new Point(1, 1),
                    GradientStops = new GradientStopCollection
                    {
                        new GradientStop(Colors.LightBlue, 0),
                        new GradientStop(Colors.Cyan, 1)
                    }
                }
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