using AutoMapper;
using NETCore.Terraform.SDK.Schema;

namespace NETCore.Terraform.SDK.Internal
{
    public class NestedBlockMapperConfiguration : Profile
    {
        public NestedBlockMapperConfiguration()
        {
            CreateMap<TerraformNestedBlock, Tfplugin5.Schema.Types.NestedBlock>()
                .ForMember(destination => destination.Block, options => options.MapFrom(source => source.Block))
                .ForMember(destination => destination.MaxItems, options => options.MapFrom(source => source.MaxItems))
                .ForMember(destination => destination.MinItems, options => options.MapFrom(source => source.MinItems))
                .ForMember(destination => destination.Nesting, options => options.MapFrom(source => source.Mode))
                .ForMember(destination => destination.TypeName, options => options.Ignore())
                .ForAllOtherMembers(options => options.Ignore());

            CreateMap<string, Tfplugin5.Schema.Types.NestedBlock>()
                .ForMember(destination => destination.TypeName, options => options.MapFrom(source => source))
                .ForAllOtherMembers(options => options.Ignore());
        }
    }
}
