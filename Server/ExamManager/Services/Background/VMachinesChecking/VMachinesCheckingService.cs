using ExamManager.Models;
using ExamManager.Services;
using Microsoft.AspNetCore.SignalR;
using System.Threading.Channels;

namespace ExamManager.Services;

public class VMachinesCheckingService : BackgroundService, IVMachinesCheckingService
{
    ILogger<VMachinesCheckingService> _logger;
    INotificationService _notificationService;
    IServiceProvider _services;
    List<string> _vMachineIds;
    object _locker;
    public VMachinesCheckingService(INotificationService notificationService, IServiceProvider services, ILogger<VMachinesCheckingService> logger)
    {
        _notificationService = notificationService;

        _vMachineIds = new();
        _services = services;
        _logger = logger;
    }

    public async Task AddVirtualMachine(string vMachineId)
    {
        lock (_locker)
        {
            _vMachineIds.Add(vMachineId);
        }
    }

    public async Task RemoveVirtualMachine(string vMachineId)
    {
        lock (_locker)
        {
            _vMachineIds.Remove(vMachineId);
        }
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        while (!stoppingToken.IsCancellationRequested)
        {
            if (_vMachineIds.Count > 0)
            {
                _logger.LogInformation($"CHECKING VIRTUAL MACHINES STATUSES ({string.Join(", ", _vMachineIds)})");
                Queue<string> vMachinesStack;
                lock (_locker)
                {
                    vMachinesStack = new Queue<string>(_vMachineIds);
                }
                using (var scope = _services.CreateScope())
                {
                    var taskService = scope.ServiceProvider.GetRequiredService<IStudyTaskService>();

                    foreach (var vMachine in vMachinesStack)
                    {
                        await taskService.CheckTaskStatus(vMachine);
                    }
                }
            }

            await Task.Delay(5000);
        }
    }
}