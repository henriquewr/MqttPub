using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using MqttPub.Application.Services.MqttConnections.Abstractions;
using MqttPub.Application.Services.MqttConnections.Abstractions.ContractModels;

namespace MqttPub.ViewModels.MqttConnectionModels.Create
{
    public partial class CreateMqttConnectionViewModel : ObservableObject, ICreateMqttConnectionModel, IUpdateMqttConnectionModel
    {
        private readonly IMqttConnectionService _mqttConnectionService;
        public CreateMqttConnectionViewModel(IMqttConnectionService mqttConnectionService)
        {
            _mqttConnectionService = mqttConnectionService;
        }

        public async Task LoadMqttConnection(int id)
        {
            try
            {
                MainThread.BeginInvokeOnMainThread(() =>
                {
                    Shell.Current.CurrentPage.IsBusy = true;
                });

                var mqttConnection = await _mqttConnectionService.GetConnection(id);

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

        int IUpdateMqttConnectionModel.Id => Id!.Value;
        string ISaveMqttConnectionModel.BrokerAddress => BrokerAddress!;
        string ISaveMqttConnectionModel.Topic => Topic!;
        string ISaveMqttConnectionModel.ClientId => ClientId!;
        int ISaveMqttConnectionModel.Port => Port;

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

            try
            {
                await (Id.HasValue 
                    ? _mqttConnectionService.Update(this) 
                    : _mqttConnectionService.Create(this));

                await Shell.Current.CurrentPage.DisplayAlertAsync("Success!", $"The MqttConnection was {(Id.HasValue ? "updated" : "created")}", "Ok");
                await Shell.Current.GoToAsync("..");
            }
            catch (Exception ex)
            {
                await Shell.Current.CurrentPage.DisplayAlertAsync("Error!", ex.Message, "Ok");
            }
        }
    }
}