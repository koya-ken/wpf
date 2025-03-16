using CommunityToolkit.Mvvm.ComponentModel;
using System.Diagnostics;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace ExampleProject.View
{
    internal struct MouseGrabState
    {
        public object Sender;
        public Point DragStartPoint;
        public double StartX;
        public double StartY;
        public double StartWidth;
        public double StartHeight;
        public bool XEditable;
        public bool YEditable;

        public bool IsGrab(object target, MouseEventArgs e)
        {
            var windowActive = Application.Current.Windows.OfType<Window>().FirstOrDefault()?.IsActive ?? false;
            return windowActive && Sender == target && e.LeftButton == MouseButtonState.Pressed;
        }

        public void GrabAction(object target, MouseEventArgs e, Action<MouseGrabState> action)
        {
            if (!IsGrab(target, e))
            {
                return;
            }
            action(this);
        }

        public bool IsLeave(object target)
        {
            return Sender == target;
        }

        public void DoStateAction(Action<MouseGrabState> action)
        {
            action(this);
        }
    }

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

        private MouseGrabState? RectGrabState = null;
        private MouseGrabState? ToolGrabState = null;
        private Point DragStartPoint;


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
            e.Handled = true;
            this.Cursor = Cursors.SizeAll;
        }

        private void OnMouseLeave(object sender, MouseEventArgs e)
        {
            if (RectGrabState?.IsLeave(sender) ?? false)
            {
                e.Handled = true;
                RectGrabState = null;
                Mouse.Capture(null);
            }
        }

        private void OnMouseMove(object sender, MouseEventArgs e)
        {
            Debug.WriteLine($"OnMouseMove {sender} {Mouse.Captured}");
            RectGrabState?.GrabAction(Mouse.Captured, e, (state) =>
            {
                e.Handled = true;
                // GetPositionしているときに対象の要素の位置を変えるとおかしくなるので
                // 親からの位置にしている
                var currentPosition = e.GetPosition((IInputElement)this.Parent);
                var offset = currentPosition - state.DragStartPoint;
                X = offset.X;
                Y = offset.Y;
            });

            // GetPositionしているときに対象の要素の位置を変えるとおかしくなるので
            // 親からの位置にしている
            ToolGrabState?.GrabAction(Mouse.Captured, e, (state) =>
            {
                e.Handled = true;
                var currentPosition = e.GetPosition((IInputElement)this.Parent);
                var offset = currentPosition - state.DragStartPoint;

                double minWidth = 50;
                double minHeight = 50;

                if (state.XEditable)
                {
                    var _x = state.StartX + offset.X;
                    X = Math.Min(_x, (state.StartX + state.StartWidth - minWidth));
                    var _w = (state.StartX - X) + state.StartWidth;
                    Width = Math.Max(_w, minWidth);
                    Debug.WriteLine($"GrabAction {(state.StartX, X)}");
                }
                else
                {
                    var t = state.StartWidth + offset.X;
                    Width = t < minWidth ? minWidth : t;
                }

                if (state.YEditable)
                {
                    var _y = state.StartY + offset.Y;
                    Y = Math.Min(_y, (state.StartY + state.StartHeight - minHeight));
                    var _h = (state.StartY - Y) + state.StartHeight;
                    Height = Math.Max(_h, minHeight);
                }
                else
                {
                    var t = state.StartHeight + offset.Y;
                    Height = t < minHeight ? minHeight : t;
                }
            });
        }

        private void OnMouseDown(object sender, MouseButtonEventArgs e)
        {
            Debug.WriteLine($"OnMouseDown {sender}");
            if (RectGrabState?.IsGrab(sender, e) ?? false)
            {
                return;
            }
            e.Handled = true;
            RectGrabState = new MouseGrabState
            {
                Sender = sender,
                DragStartPoint = e.GetPosition(this),
                StartX = X,
                StartY = Y
            };
            var parent = (Panel)this.Parent;
            // BringToFront
            parent.Children.Remove(this);
            parent.Children.Add(this);
            Mouse.Capture(this);
        }

        private void OnMouseUp(object sender, MouseButtonEventArgs e)
        {
            Debug.WriteLine("OnMouseUp");
            if (RectGrabState?.IsLeave(sender) ?? false)
            {
                e.Handled = true;
                RectGrabState = null;
                Mouse.Capture(null);
            }
        }

        private void OnUnloaded(object sender, RoutedEventArgs e)
        {

        }

        private void OnLoaded(object sender, RoutedEventArgs e)
        {

        }

        private void ToolBoxMouseDown(object sender, MouseButtonEventArgs e)
        {
            Debug.WriteLine($"ToolBoxMouseDown {sender}");
            if (ToolGrabState?.IsGrab(sender, e) ?? false)
            {
                return;
            }


            string? name = (sender as FrameworkElement)?.Name;

            bool xEditable = false;
            bool yEditable = false;

            switch (name)
            {
                case "LeftTop":
                    xEditable = true;
                    yEditable = true;
                    break;
                case "RightTop":
                    xEditable = false;
                    yEditable = true;
                    break;
                case "LeftBottom":
                    xEditable = true;
                    yEditable = false;
                    break;
                case "RightBottom":
                    xEditable = false;
                    yEditable = false;
                    break;
                case null:
                    Console.WriteLine("No name assigned");
                    break;
                default:
                    Console.WriteLine($"Mouse entered: {name}");
                    break;
            }
            e.Handled = true;
            ToolGrabState = new MouseGrabState
            {
                Sender = sender,
                DragStartPoint = e.GetPosition((IInputElement)this.Parent),
                StartX = X,
                StartY = Y,
                StartWidth = Width,
                StartHeight = Height,
                XEditable = xEditable,
                YEditable = yEditable
            };
            Mouse.Capture((IInputElement)sender);
        }

        private void ToolBoxMouseUp(object sender, MouseButtonEventArgs e)
        {
            Debug.WriteLine("ToolBoxMouseUp");
            if (ToolGrabState?.IsLeave(sender) ?? false)
            {
                e.Handled = true;
                ToolGrabState = null;
                Mouse.Capture(null);
            }
        }

        private void ToolBoxMouseLeave(object sender, MouseEventArgs e)
        {
            Debug.WriteLine("ToolBoxMouseLeave");
            //if (ToolGrabState?.IsLeave(sender) ?? false)
            //{
            //    e.Handled = true;
            //    ToolGrabState = null;
            //    Mouse.Capture(null);
            //}

        }

        private void ToolBoxMouseMove(object sender, MouseEventArgs e)
        {
        }
    }
}
