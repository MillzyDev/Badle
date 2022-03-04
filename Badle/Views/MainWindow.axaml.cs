using Avalonia;
using Avalonia.Controls;
using Avalonia.Input;
using Avalonia.Interactivity;
using Avalonia.Markup.Xaml;
using Avalonia.Media;
using Badle.Wordle;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Badle.Views
{
    public partial class MainWindow : Window
    {
        private TextBox _input;
        private Button _enter;
        private TextBlock _output;
        private Button _retry;

        private int _currentTurn = 0;
        private string? answer;

        private static readonly Random random = new Random();
        private static readonly string keyboard = "qwertyuiopasdfghjklzxcvbnm";

        public MainWindow()
        {
            InitializeComponent();
#if DEBUG
            this.AttachDevTools();
#endif
            _input = this.FindControl<TextBox>("Input");

            _enter = this.FindControl<Button>("Enter");
            _enter.Click += EnterWord;

            _retry = this.FindControl<Button>("Retry");
            _retry.Click += Reset;

            _output = this.FindControl<TextBlock>("Output");

            foreach (char c in keyboard)
            {
                this.FindControl<Button>(new string(c, 1)).Click += KeyboardClick;
            }
        }

        private void InitializeComponent()
        {
            AvaloniaXamlLoader.Load(this);
        }

        public void EnterWord(object? _, RoutedEventArgs __)
            => EnterWord();

        public void EnterWord()
        {
            string[] answers = Words.Instance.Answers;
            if (answer == null) answer = answers[random.Next(answers.Length - 1)];

            if (_input.Text?.Length < 5)
            {
                _output.Text = "Not a five letter word.";
                return;
            }
            Words words = Words.Instance;
            if (!words.Acceptables.Contains(_input?.Text?.ToLower()))
            {
                _output.Text = "Not in words list.";
                return;
            }

            _currentTurn++;
#pragma warning disable CS8604 // Possible null reference argument.
            AddWordToGrid(_input?.Text, answer, _currentTurn);
#pragma warning restore CS8604 // Possible null reference argument.

            if (_input.Text == answer)
            {
                _output.Text = $"You Guessed The Word! (\"{answer}\")";
                _input.IsVisible = false;
                _enter.IsVisible = false;
                _retry.IsVisible = true;
            }
            else if (_currentTurn == 6) 
            {
                _output.Text = $"You've used all your guesses. The word was \"{answer}\".";
                _input.IsVisible = false;
                _enter.IsVisible = false;
                _retry.IsVisible = true;
            }

            _input.Text = "";
        }

        public void Reset(object? sender, RoutedEventArgs e)
        {
            for (int i = 1; i <= 6; i++)
            {
                for (int j = 1; j <= 5; j++)
                {
                    var button = this.FindControl<Button>($"{i}{j}");
                    button.Content = "";
                    button.Classes.Clear();
                } 
            }

            answer = null;
            _output.Text = "";

            _currentTurn = 0;

            _input.IsVisible = true;
            _enter.IsVisible = true;

            _retry.IsVisible = false;

            foreach (char key in keyboard)
            {
                var keyButton = this.FindControl<Button>(new string(key, 1));
                keyButton.Classes.Clear();
            }
        }

        private void AddWordToGrid(string word, string answer, int turn) {
            List<char> charList = new(answer.ToCharArray());

            for (int i = 1; i <= 5; i++)
            {
                var button = this.FindControl<Button>($"{turn}{i}");
                char letter = word[i - 1];
                button.Content = letter.ToString().ToUpper();

                var letterButton = this.FindControl<Button>(new string(letter, 1));

                if (answer[i - 1] == letter)
                {
                    button.Classes.Add("isHere");
                    charList.Remove(letter);

                    if (letterButton.Classes.Count > 0)
                        letterButton.Classes.Clear();
                    letterButton.Classes.Add("isHere");

                    if (CountOccurances(charList, letter) == 0)
                    {
                        for (int j = 1; j <= 5; j++)
                        {
                            var wrongColour = this.FindControl<Button>($"{turn}{j}");
                            if (wrongColour.Content?.ToString() == $"{letter.ToString().ToUpper()}" && wrongColour.Classes.Contains("contains"))
                                wrongColour.Classes.Clear();
                        }
                    }
                }
                else if (charList.Contains(letter))
                {
                    button.Classes.Add("contains");
                    charList.Remove(letter);

      
                    if (letterButton.Classes.Count == 0)
                    {
                        letterButton.Classes.Add("contains");
                    }
                }
                else if (letterButton.Classes.Count == 0)
                { 
                        letterButton.Classes.Add("incorrect");
                }
            }

        }

        public void KeyboardClick(object? sender, RoutedEventArgs e)
        {
            _input.Text += (e?.Source as Button).Name;
        }

        private int CountOccurances(IEnumerable<char> items, char query)
        {
            int count = 0;
            foreach (char item in items)
            {
                if (item == query) 
                { 
                    count++;
                }
            }
            return count;
        }
    }
}