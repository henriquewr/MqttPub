using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using MqttPub.Data;
using MqttPub.Data.Entities;

namespace MqttPub.ViewModels.MqttConnectionModels.Create
{
    public partial class CreateMqttConnectionViewModel : ObservableObject
    {
        private readonly IRepository<MqttConnection> _mqttConnectionRepository;
        public CreateMqttConnectionViewModel(IRepository<MqttConnection> mqttConnectionRepository)
        {
            _mqttConnectionRepository = mqttConnectionRepository;
        }

        public async Task LoadMqttConnection(int id)
        {
            try
            {
                MainThread.BeginInvokeOnMainThread(() =>
                {
                    Shell.Current.CurrentPage.IsBusy = true;
                });

                var mqttConnection = await _mqttConnectionRepository.SelectFirstAsync(x => x.Id == id, x => new
                {
                    x.BrokerAddress,
                    x.Topic,
                    x.ClientId,
                    x.Port,
                });

                BrokerAddress = mqttConnection!.BrokerAddress;
                Topic = mqttConnection.Topic;
                ClientId = mqttConnection.ClientId;
                Port = mqttConnection.Port;
                Id = id;
            }
            finally
            {
                MainThread.BeginInvokeOnMainThread(() =>
                {
                    Shell.Current.CurrentPage.IsBusy = false;
                });
            }
        }
        [ObservableProperty]
        public partial int? Id { get; set; }

        [ObservableProperty]
        public partial string? BrokerAddress { get; set; }

        [ObservableProperty]
        public partial string? Topic { get; set; }

        [ObservableProperty]
        public partial string? ClientId { get; set; } = Guid.CreateVersion7().ToString();

        [ObservableProperty]
        public partial int Port { get; set; } = 1883;

        private bool IsValid()
        {
            var isValid = string.IsNullOrEmpty(BrokerAddress) == false
                && string.IsNullOrEmpty(Topic) == false
                && string.IsNullOrEmpty(ClientId) == false;

            return isValid;
        }

        [RelayCommand]
        public async Task Save()
        {
            if (!IsValid())
            {
                return;
            }

            MqttConnection mqttConnection;
            
            if (Id.HasValue)
            {
                mqttConnection = (await _mqttConnectionRepository.GetByIdAsync(Id.Value, false))!;

                mqttConnection.BrokerAddress = BrokerAddress!;
                mqttConnection.Topic = Topic!;
                mqttConnection.ClientId = ClientId!;
                mqttConnection.Port = Port;
            }
            else
            {
                mqttConnection = new()
                {
                    BrokerAddress = BrokerAddress!,
                    Topic = Topic!,
                    ClientId = ClientId!,
                    Port = Port,
                };
            }

            _mqttConnectionRepository.Update(mqttConnection);

            try
            {
                await _mqttConnectionRepository.SaveChangesAsync();

                await Shell.Current.CurrentPage.DisplayAlert("Success!", $"The MqttConnection was {(Id.HasValue ? "updated" : "created")}", "Ok");
                await Shell.Current.GoToAsync("..");
            }
            catch (Exception ex)
            {
                await Shell.Current.CurrentPage.DisplayAlert("Error!", ex.Message, "Ok");
            }
        }
    }
}