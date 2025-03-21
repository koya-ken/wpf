using ExampleProject.src.View;
using ExampleProject.View;
using System.Windows;
using System.Windows.Media;

namespace ExampleProject;

/// <summary>
/// Interaction logic for MainWindow.xaml
/// </summary>
public partial class MainWindow : Window
{
    public MainWindow()
    {
        InitializeComponent();

        MainCanvas.Children.Add(new EditableRectangle
        {
            Width = 200,
            Height = 100,
            Text = "Rect1",
            Background = new SolidColorBrush(Color.FromArgb(128, 255, 0, 0))
        });
        MainCanvas.Children.Add(new EditableRectangle
        {
            Width = 200,
            Height = 100,
            Text = "Rect2",
            Background = new SolidColorBrush(Color.FromArgb(128, 0, 255, 255))
        });

        var obj = new
        {
            DoSomething = new Action(() => Console.WriteLine("Hello from anonymous class!"))
        };

    }

    private void Button_Click(object sender, RoutedEventArgs e)
    {
        new SubWindow
        {
            Owner = this,
            WindowStartupLocation = WindowStartupLocation.CenterOwner
        }.ShowDialog();
    }

    private void MenuItem_Click(object sender, RoutedEventArgs e)
    {

    }
}