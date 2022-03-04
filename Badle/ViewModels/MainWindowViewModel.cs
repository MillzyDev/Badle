using Avalonia.Controls;
using Avalonia.Media;
using Badle.Views;
using ReactiveUI;
using System;
using System.Collections.Generic;
using System.Reactive;
using System.Text;

namespace Badle.ViewModels
{
    public class MainWindowViewModel : ViewModelBase
    {
        TextBox _input;
        MainWindow _window;

        public ReactiveCommand<string, Unit> KeyboardButton { get; set; }
        public ReactiveCommand<Unit, Unit> EnterButton { get; set; }

        public MainWindowViewModel(MainWindow window)
        {
            _window = window; 
            _input = _window.FindControl<TextBox>("Input");
            KeyboardButton = ReactiveCommand.Create<string>(KeyboardButtonPress);
            EnterButton = ReactiveCommand.Create(EnterButtonPress);
        }

        public void KeyboardButtonPress(string c)
        {
            if (_input.Text?.Length < 5) _input.Text += c;
        }

        public void EnterButtonPress()
            => _window.EnterWord();
    }
}
