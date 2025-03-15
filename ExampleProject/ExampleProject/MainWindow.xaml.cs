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
    EditableRectangle Child1;
    public MainWindow()
    {
        InitializeComponent();

        Child1 = new EditableRectangle { Width = 200, Height = 100, Background = Brushes.Red };
        MainCanvas.Children.Add(Child1);
        //Start();
    }

    async Task Start()
    {
        for (int i = 0; i < 100; i++)
        {

            await Task.Delay(TimeSpan.FromMilliseconds(33));
            Child1.X += 2;
        }
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