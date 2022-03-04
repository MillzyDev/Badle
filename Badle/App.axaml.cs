using Avalonia;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.Markup.Xaml;
using Badle.ViewModels;
using Badle.Views;

namespace Badle
{
    public class App : Application
    {
        public override void Initialize()
        {
            AvaloniaXamlLoader.Load(this);
        }

        public override void OnFrameworkInitializationCompleted()
        {
            if (ApplicationLifetime is IClassicDesktopStyleApplicationLifetime desktop)
            {
                desktop.MainWindow = new MainWindow
                {
                    DataContext = desktop.MainWindow,
                    Width = 450,
                    MinWidth = 450,
                    Height = 700,
                    MinHeight = 700
                };
            }

            base.OnFrameworkInitializationCompleted();
        }
    }
}
