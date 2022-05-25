using ExamManager.Models;

namespace ExamManager.Services;

public interface IVirtualMachineService
{

    /// <summary>
    /// Запустить виртуальную машину
    /// </summary>
    /// <param name="virtualMachineId">Идентификатор образа виртуальной машины</param>
    /// <param name="owner">Пользватель, пытающийся запустить виртуальную машину</param>
    /// <returns></returns>
    public Task StartVirtualMachine(string virtualMachineId, Guid owner);

    /// <summary>
    /// Остановить виртуальную машину
    /// </summary>
    /// <param name="virtualMachineId">Идентификатор образа виртуальной машины</param>
    /// <param name="owner">Пользватель, пытающийся остановить виртуальную машину</param>
    /// <returns></returns>
    public Task StopVirtualMachine(string virtualMachineId, Guid owner);

    public Task<VirtualMachine> GetVirtualMachine(string virtualMachineID);
}