using MqttPub.ViewModels;

namespace MqttPub
{
    public partial class MainPage : ContentPage
    {
        public MainPage(MainPageViewModel mainPageModel)
        {
            InitializeComponent();
            BindingContext = mainPageModel;
        }
    }
}