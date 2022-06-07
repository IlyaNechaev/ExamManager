namespace ExamManager.Services;

public interface IVMachinesCheckingService
{
    public Task AddVirtualMachine(string vMachineId);
    public Task RemoveVirtualMachine(string vMachineId);
}