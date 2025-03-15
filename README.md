# exeにiconを設定

https://learn.microsoft.com/ja-jp/visualstudio/ide/how-to-specify-an-application-icon-visual-basic-csharp?view=vs-2022

csprojに以下を追加
```
<Project Sdk="Microsoft.NET.Sdk.WindowsDesktop">
  <PropertyGroup>
    <OutputType>WinExe</OutputType>
    <TargetFramework>net8.0-windows</TargetFramework>
    <ApplicationIcon>your_icon.ico</ApplicationIcon>
  </PropertyGroup>
</Project>
```

# Windowにアイコンを設定

以下の手段がある
- ソースで指定
- Windowのxamlで指定
- ApplicationでWindow全体に指定

## ソースで指定
```
using System.Windows;
using System.Windows.Media.Imaging;

namespace ExampleProject
{
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            this.Icon = new BitmapImage(new Uri("pack://application:,,,/Assets/your_icon.ico"));
        }
    }
}
```
## xamlで指定
```
<Window x:Class="ExampleProject.MainWindow"
        xmlns="http://schemas.microsoft.com/winfx/2006/xaml/presentation"
        xmlns:x="http://schemas.microsoft.com/winfx/2006/xaml"
        Title="My App"
        Height="450" Width="800"
        Icon="Assets/your_icon.ico">
    <Grid>
        <TextBlock Text="Hello World!" VerticalAlignment="Center" HorizontalAlignment="Center"/>
    </Grid>
</Window>
```
## Applicationで指定
```
<Application x:Class="ExampleProject.App"
             xmlns="http://schemas.microsoft.com/winfx/2006/xaml/presentation"
             xmlns:x="http://schemas.microsoft.com/winfx/2006/xaml"
             StartupUri="MainWindow.xaml">
    <Application.Resources>
        <Style TargetType="Window">
            <Setter Property="Icon" Value="Assets/your_icon.ico"/>
        </Style>
    </Application.Resources>
</Application>
```


# フォルダ構成を変える

下記ファイルについては変えると面倒そうなので  
こいつらはプロジェクトのルートに置く
- App.xaml
- App.xaml.cs
- AssemblyInfo.cs

MainWindow.xamlを別の場所に置いた場合  
App.xamlの下記部分を書き換える  

```
  StartupUri="MainWindow.xaml"
```

# ボタンクリックイベントの追加

Visual Studioでxamlを編集し、  
xml上でClick属性を追加する時tabで補完すると勝手にソースに関数を追加してくれる。

# ダイアログ表示

Windowのxamlを作成しShowDialogで表示すると他のウインドウが操作不可になる。  
OwnerとWindowStartupLocationを設定すれば以下の設定だと初期位置が表示したウインドウの中心になる。
```
    private void Button_Click(object sender, RoutedEventArgs e)
    {
        new SubWindow
        {
            Owner = this,
            WindowStartupLocation = WindowStartupLocation.CenterOwner
        }.ShowDialog();
    }
```

# UI文字列のリソース化

resxを作成してXamlから参照するのがよさそうだが癖がある

- リソースファイルを作成
- UIのxamlから参照
- コードから参照

リソースファイルは専用のフォルダを作ってそこに格納する  
リソースファイルからクラスが生成され、他のファイルからアクセスする時はクラスを参照している。  

リソースファイルのプロパティからカスタムツールを  
PublicResXFileCodeGeneratorに設定しないとエラーになる。  
よくわからない

# フォントの指定
https://learn.microsoft.com/ja-jp/dotnet/desktop/wpf/app-development/pack-uris-in-wpf?view=netframeworkdesktop-4.8
https://zenn.dev/noburo/articles/cc45cc3c65cac1


# theme
https://qiita.com/norimatsu_yusuke/items/3a7a22f0d852d99e18cc