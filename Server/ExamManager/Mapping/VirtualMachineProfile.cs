using AutoMapper;
using ExamManager.Services;
using ExamManager.Models;
using ExamManager.Models.RequestModels;
using System.Reflection;

namespace ExamManager.Mapping;

public partial class MappingProfile
{
    [PerformMapping]
    private void VirtualMachineProfile()
    {
        CreateMap<CreateTaskRequest.VirtualMachineView, VirtualMachineImage>()
            .ForMember(vmImage => vmImage.ID, options => options.MapFrom(vmView => vmView.id))
            .ForMember(vmImage => vmImage.Title, options => options.MapFrom(vmView => vmView.title))
            .ForMember(vmImage => vmImage.Order, options => options.MapFrom(vmView => vmView.order));
    }
}