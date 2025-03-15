using CommunityToolkit.Mvvm.ComponentModel;
using System.Diagnostics;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace ExampleProject.View
{
    /// <summary>
    /// ObservableRectangle.xaml の相互作用ロジック
    /// </summary>
    [INotifyPropertyChanged]
    public partial class EditableRectangle : UserControl
    {
        [ObservableProperty]
        public double _x;
        [ObservableProperty]
        public double _y;

        private double StartX;
        private double StartY;

        private Point DragStartPoint;
        private Stack<Cursor> CursorStack = new(1);

        public EditableRectangle()
        {
            InitializeComponent();
        }

        partial void OnXChanged(double oldValue, double newValue)
        {
            Canvas.SetLeft(this, newValue);
        }

        partial void OnYChanged(double oldValue, double newValue)
        {
            Canvas.SetTop(this, newValue);
        }

        private void OnMouseEnter(object sender, System.Windows.Input.MouseEventArgs e)
        {
            if (this.Cursor != Cursors.SizeAll)
            {
                CursorStack.Push(Cursor);
            }
            this.Cursor = Cursors.SizeAll;
        }

        private void OnMouseLeave(object sender, MouseEventArgs e)
        {
            if (Mouse.Captured == this)
            {
                Mouse.Capture(null);
            }
        }

        private void OnMouseMove(object sender, MouseEventArgs e)
        {
            var windowActive = Application.Current.Windows.OfType<Window>().FirstOrDefault()?.IsActive;
            if (windowActive != true)  // 親ウィンドウがアクティブ
            {
                return;
            }
            if (Mouse.Captured != this)
            {
                return;
            }
            if (e.LeftButton != MouseButtonState.Pressed)
            {
                return;
            }
            // GetPositionしているときに対象の要素の位置を変えるとおかしくなるので
            // 親からの位置にしている
            var currentPosition = e.GetPosition((IInputElement)this.Parent);
            var offset = currentPosition - DragStartPoint;
            this.X = offset.X;
            this.Y = offset.Y;
            Debug.WriteLine($"{(e.RoutedEvent, e.Handled)} {(X, Y)} current:{currentPosition}, drag:{DragStartPoint}");
        }

        private void OnMouseDown(object sender, MouseButtonEventArgs e)
        {
            Debug.WriteLine("Down Called");
            if (Mouse.Captured == this)
            {
                return;
            }
            DragStartPoint = e.GetPosition(this);
            StartX = X;
            StartY = Y;
            Mouse.Capture(this);
        }

        private void OnMouseUp(object sender, MouseButtonEventArgs e)
        {
            if (Mouse.Captured != this)
            {
                return;
            }
            Mouse.Capture(null);
        }
    }
}
