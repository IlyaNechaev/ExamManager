using ExamManager.Models;
using ExamManager.Models.RequestModels;

namespace ExamManager.Mapping;


public partial class MappingProfile
{
    [PerformMapping]
    private void RequestsMapping()
    {
        CreateMap<CreateTaskRequest, StudyTask>()
            .ForMember(task => task.Title, options => options.MapFrom(request => request.title))
            .ForMember(task => task.Description, options => options.MapFrom(request => request.description))
            .ForMember(task => task.VirtualMachines, options => options.MapFrom(request => MapVirtualMachines(request.virtualMachines)))
            ;

        // CreateUsersRequest -> IEnumerable<User>
        CreateMap<CreateUsersRequest.UserView, User>()
            .ForMember(user => user.FirstName, options => options.MapFrom(view => view.firstName))
            .ForMember(user => user.LastName, options => options.MapFrom(view => view.lastName))
            .ForMember(user => user.Login, options => options.MapFrom(view => view.login))
            .ForMember(user => user.PasswordHash, options => options.MapFrom(view => _securityService.Encrypt(view.password)))
            ;

    }

    private VirtualMachineImage[] MapVirtualMachines(CreateTaskRequest.VirtualMachineView[] vmViews)
    {
        return vmViews.Select(view => new VirtualMachineImage
        {
            Title = view.title,
            ID = view.id,
            Order = view.order
        }).ToArray();
    }
}