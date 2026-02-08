namespace MqttPub.Application.Services.MqttActions.Abstractions.ContractModels
{
    public interface IMqttActionItemModel<TConnection>
    {
        int Id { get; set; }
        string Name { get; set; }
        TConnection MqttConnection { get; set; }
    }

    public interface IMqttActionItemMqttConnectionModel 
    {
        int Id { get; set; }
        string BrokerAddress { get; set; }
        string Topic { get; set; }
    }
}
