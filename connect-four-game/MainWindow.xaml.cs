﻿using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Shapes;

namespace connect_four_game
{

    public partial class MainWindow : Window
    {
        private Table table;
        private Player player1;
        private Player player2;
        private int filled;
        private bool game;
        public enum Turn
        {
            FIRST,
            SECOND
        };
        private Turn playerTurn;
        public MainWindow()
        {
            InitializeComponent();
            table = new Table();
            player1 = new Player("Ensar");
            player2 = new Player("Tarik");
            FirstPlayer.Text = player1.Name;
            SecondPlayer.Text = player2.Name;
            playerTurn = Turn.FIRST;
            game = false;
            filled = 0;
            DrawTable();
        }

        private void DrawTable()
        {
            StackPanel mainSP = new StackPanel();
            Grid.SetRow(mainSP, 1);
            mainSP.HorizontalAlignment = HorizontalAlignment.Center;
            mainSP.VerticalAlignment = VerticalAlignment.Center;

            for (int i = 0; i < 6; i++)
            {
                StackPanel fieldSP = new StackPanel();
                fieldSP.Orientation = Orientation.Horizontal;
                if(i != 0)
                {
                    fieldSP.Margin = new Thickness(0, 10, 0, 0);
                }
                for(int j = 0; j < 7; j++)
                {
                    Ellipse e = new Ellipse();
                    e.Name = $"r{i}c{j}";
                    GameGrid.RegisterName(e.Name, e);
                    e.Height = 100;
                    e.Width = 100;
                    if(j != 0)
                    {
                        e.Margin = new Thickness(10,0,0,0);
                    }
                    e.Fill = Brushes.Transparent;
                    e.Stroke = Brushes.Black;
                    e.StrokeThickness = 2;
                    e.MouseLeftButtonDown += Field_MouseLeftButtonDown;
                    if(i == 0)
                    {
                        StackPanel firstRow = new StackPanel();
                        TextBlock rowNum = new TextBlock();
                        rowNum.Text = $"{j + 1}";
                        rowNum.HorizontalAlignment = HorizontalAlignment.Center;
                        rowNum.VerticalAlignment = VerticalAlignment.Center;
                        rowNum.FontWeight = FontWeights.Bold;
                        rowNum.FontSize = 30;
                        firstRow.Children.Add(rowNum);
                        firstRow.Children.Add(e);
                        fieldSP.Children.Add(firstRow);
                    } else
                    {
                        fieldSP.Children.Add(e);
                    }
                    
                }
                mainSP.Children.Add(fieldSP);
            }

            GameGrid.Children.Add(mainSP);
        }

        private void Field_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            if (!game) return;
            Ellipse clickedEllipse = sender as Ellipse;
            int row = int.Parse(clickedEllipse.Name[clickedEllipse.Name.Length - 1].ToString());
            for (int i = 5; i >= 0; i--)
            {
                if(table.Fields[i, row].Value == Field.Val.VOID)
                {
                    if(playerTurn == Turn.FIRST)
                    {
                        table.Fields[i, row].Value = Field.Val.RED;
                        this.Dispatcher.Invoke(() =>
                        {
                            Ellipse ellipse = GameGrid.FindName($"r{i}c{row}") as Ellipse;
                            ellipse.Fill = Brushes.Red;
                        });
                        playerTurn = Turn.SECOND;
                    } else
                    {
                        table.Fields[i, row].Value = Field.Val.YELLOW;
                        this.Dispatcher.Invoke(() =>
                        {
                            Ellipse ellipse = GameGrid.FindName($"r{i}c{row}") as Ellipse;
                            ellipse.Fill = Brushes.Yellow;
                        });
                        playerTurn = Turn.FIRST;
                    }
                    filled++;
                    if(filled > 6)
                    {
                        HasWinner();
                    }
                    break;
                }
            }
            HighlightPlayer();
        }

        private void HasWinner()
        {
            int winner = 0;
            int[,] winnerFields = new int[4, 2];
            for(int i = 5; i >= 0; i--) // Horizontal win
            {
                if(winner != 0)
                    break;
                for(int j = 0; j < 4; j++)
                {
                    if(table.Fields[i,j].Value == Field.Val.RED && table.Fields[i, j+1].Value == Field.Val.RED && table.Fields[i, j+2].Value == Field.Val.RED && table.Fields[i, j+3].Value == Field.Val.RED)
                    {
                        winner = 1;
                    } else if(table.Fields[i, j].Value == Field.Val.YELLOW && table.Fields[i, j + 1].Value == Field.Val.YELLOW && table.Fields[i, j + 2].Value == Field.Val.YELLOW && table.Fields[i, j + 3].Value == Field.Val.YELLOW)
                    {
                        winner = 2;
                    }
                    if(winner != 0)
                    {
                        winnerFields[0, 0] = i;
                        winnerFields[0, 1] = j;

                        winnerFields[1, 0] = i;
                        winnerFields[1, 1] = j + 1;

                        winnerFields[2, 0] = i;
                        winnerFields[2, 1] = j + 2;

                        winnerFields[3, 0] = i;
                        winnerFields[3, 1] = j + 3;
                        break;
                    }
                }
            }

            for (int j = 0; j < 7; j++) // Vertical win
            {
                if (winner != 0)
                    break;
                for (int i = 5; i > 2; i--)
                {
                    if (table.Fields[i, j].Value == Field.Val.RED && table.Fields[i-1, j].Value == Field.Val.RED && table.Fields[i-2, j].Value == Field.Val.RED && table.Fields[i-3, j].Value == Field.Val.RED)
                    {
                        winner = 1;
                    }
                    else if (table.Fields[i, j].Value == Field.Val.YELLOW && table.Fields[i - 1, j].Value == Field.Val.YELLOW && table.Fields[i - 2, j].Value == Field.Val.YELLOW && table.Fields[i - 3, j].Value == Field.Val.YELLOW)
                    {
                        winner = 2;
                    }
                    if (winner != 0)
                    {
                        winnerFields[0, 0] = i;
                        winnerFields[0, 1] = j;

                        winnerFields[1, 0] = i - 1;
                        winnerFields[1, 1] = j;

                        winnerFields[2, 0] = i - 2;
                        winnerFields[2, 1] = j;

                        winnerFields[3, 0] = i - 3;
                        winnerFields[3, 1] = j;
                        break;
                    }
                }
            }

            for (int i = 5; i > 2; i--) // Down left - up right diagonal win
            {
                if (winner != 0)
                    break;
                for (int j = 0; j < 4; j++)
                {
                    if (table.Fields[i, j].Value == Field.Val.RED && table.Fields[i - 1, j + 1].Value == Field.Val.RED && table.Fields[i - 2, j + 2].Value == Field.Val.RED && table.Fields[i - 3, j + 3].Value == Field.Val.RED)
                    {
                        winner = 1;
                    }
                    else if (table.Fields[i, j].Value == Field.Val.YELLOW && table.Fields[i - 1, j + 1].Value == Field.Val.YELLOW && table.Fields[i - 2, j + 2].Value == Field.Val.YELLOW && table.Fields[i - 3, j + 3].Value == Field.Val.YELLOW)
                    {
                        winner = 2;
                    }

                    if (winner != 0)
                    {
                        winnerFields[0, 0] = i;
                        winnerFields[0, 1] = j;

                        winnerFields[1, 0] = i - 1;
                        winnerFields[1, 1] = j + 1;

                        winnerFields[2, 0] = i - 2;
                        winnerFields[2, 1] = j + 2;

                        winnerFields[3, 0] = i - 3;
                        winnerFields[3, 1] = j + 3;
                        break;
                    }
                }
            }

            for (int i = 5; i > 2; i--) // Up left - down right diagonal win
            {
                if (winner != 0)
                    break;
                for (int j = 6; j > 2; j--)
                {
                    if (table.Fields[i, j].Value == Field.Val.RED && table.Fields[i - 1, j - 1].Value == Field.Val.RED && table.Fields[i - 2, j - 2].Value == Field.Val.RED && table.Fields[i - 3, j - 3].Value == Field.Val.RED)
                    {
                        winner = 1;
                    }
                    else if (table.Fields[i, j].Value == Field.Val.YELLOW && table.Fields[i - 1, j - 1].Value == Field.Val.YELLOW && table.Fields[i - 2, j - 2].Value == Field.Val.YELLOW && table.Fields[i - 3, j - 3].Value == Field.Val.YELLOW)
                    {
                        winner = 2;
                    }

                    if (winner != 0)
                    {
                        winnerFields[0, 0] = i;
                        winnerFields[0, 1] = j;

                        winnerFields[1, 0] = i - 1;
                        winnerFields[1, 1] = j - 1;

                        winnerFields[2, 0] = i - 2;
                        winnerFields[2, 1] = j - 2;

                        winnerFields[3, 0] = i - 3;
                        winnerFields[3, 1] = j - 3;
                        break;
                    }
                }
            }

            if (winner != 0)
            {
                for(int i = 0; i < 4; i++)
                {
                    Ellipse el = GameGrid.FindName($"r{winnerFields[i,0]}c{winnerFields[i, 1]}") as Ellipse;
                    el.StrokeThickness = 15;
                }
                game = false;
                StartButton.IsEnabled = true;
                if(winner == 1)
                {
                    player1.Score++;
                } else
                {
                    player2.Score++;
                }
                Score.Text = $"{player1.Score} : {player2.Score}";
            } else if(filled == 42)
            {
                game = false;
                StartButton.IsEnabled = true;
            }

        }

        private void StartButton_Click(object sender, RoutedEventArgs e)
        {
            game = true;
            StartButton.IsEnabled = false;
            filled = 0;
            for (int i = 0; i < 6; i++)
            {
                for (int j = 0; j < 7; j++)
                {
                    this.Dispatcher.Invoke(() =>
                    {
                        Ellipse el = GameGrid.FindName($"r{i}c{j}") as Ellipse;
                        el.Fill = Brushes.Transparent;
                        el.StrokeThickness = 2;
                    });
                    table.Fields[i, j] = new Field(); // Resetting
                }
            }
            HighlightPlayer();
        }

        private void HighlightPlayer()
        {
            if(playerTurn == Turn.FIRST)
            {
                FirstPlayer.TextDecorations = TextDecorations.Underline;
                SecondPlayer.TextDecorations = null;
            } else
            {
                SecondPlayer.TextDecorations = TextDecorations.Underline;
                FirstPlayer.TextDecorations = null;
            }
        }

        
        private void Window_KeyDown(object sender, KeyEventArgs e)
        {
            bool turn = false;
            Ellipse el = new Ellipse();
            if(e.Key == Key.D1 || e.Key == Key.NumPad1)
            {
                el = GameGrid.FindName($"r{0}c{0}") as Ellipse;
                turn = true;
                
            }
            else if (e.Key == Key.D2 || e.Key == Key.NumPad2)
            {
                el = GameGrid.FindName($"r{0}c{1}") as Ellipse;
                turn = true;
            }
            else if (e.Key == Key.D3 || e.Key == Key.NumPad3)
            {
                el = GameGrid.FindName($"r{0}c{2}") as Ellipse;
                turn = true;
            }
            else if (e.Key == Key.D4 || e.Key == Key.NumPad4)
            {
                el = GameGrid.FindName($"r{0}c{3}") as Ellipse;
                turn = true;
            }
            else if (e.Key == Key.D5 || e.Key == Key.NumPad5)
            {
                el = GameGrid.FindName($"r{0}c{4}") as Ellipse;
                turn = true;
            }
            else if (e.Key == Key.D6 || e.Key == Key.NumPad6)
            {
                el = GameGrid.FindName($"r{0}c{5}") as Ellipse;
                turn = true;
            }
            else if (e.Key == Key.D7 || e.Key == Key.NumPad7)
            {
               el = GameGrid.FindName($"r{0}c{6}") as Ellipse;
                turn = true;
            }
            else if(e.Key == Key.Space && StartButton.IsEnabled)
            {
                StartButton.RaiseEvent(new RoutedEventArgs(Button.ClickEvent));
            }


            if (turn)
            {
                MouseButtonEventArgs arg = new MouseButtonEventArgs(Mouse.PrimaryDevice, 0, MouseButton.Left);
                arg.RoutedEvent = Ellipse.MouseLeftButtonDownEvent;
                el.RaiseEvent(arg);
            }
        }
    }
}
