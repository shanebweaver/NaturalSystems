using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;

namespace NaturalSystems;

public sealed partial class GameView : UserControl
{
    public GameViewModel ViewModel { get; } = new GameViewModel();

    public GameView()
    {
        InitializeComponent();
        Loaded += OnLoaded;
    }

    private void OnLoaded(object sender, RoutedEventArgs e)
    {
        GameCanvas.Game = ViewModel.Game;
    }
}
