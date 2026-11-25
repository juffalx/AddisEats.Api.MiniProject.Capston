namespace AddisEats.Api.MiniProject.Capston.Services.Test;

public class TestScopedService
{
    public Guid InstanceId { get; } = Guid.NewGuid();

    public string GetMessage()
    {
        return $"Service Instance: {InstanceId}";
    }
}