using AutoMapper;
using NETCore.Terraform.SDK.Schema;

namespace NETCore.Terraform.SDK.Internal
{
    public class AttributeMapperConfiguration : Profile
    {
        public AttributeMapperConfiguration()
        {
            CreateMap<TerraformAttribute, Tfplugin5.Schema.Types.Attribute>()
                .ForMember(destination => destination.Computed, options => options.MapFrom(source => source.Computed))
                .ForMember(destination => destination.Deprecated, options => options.MapFrom(source => source.Deprecated))
                .ForMember(destination => destination.Description, options => options.MapFrom(source => source.Description))
                .ForMember(destination => destination.DescriptionKind, options => options.MapFrom(source => source.DescriptionKind))
                .ForMember(destination => destination.Name, options => options.Ignore())
                .ForMember(destination => destination.Optional, options => options.MapFrom(source => source.Optional))
                .ForMember(destination => destination.Required, options => options.MapFrom(source => !source.Optional))
                .ForMember(destination => destination.Sensitive, options => options.MapFrom(source => source.Sensitive))
                .ForAllOtherMembers(options => options.Ignore());

            CreateMap<string, Tfplugin5.Schema.Types.Attribute>()
                .ForMember(destination => destination.Name, options => options.MapFrom(source => source))
                .ForAllOtherMembers(options => options.Ignore());
        }
    }
}
