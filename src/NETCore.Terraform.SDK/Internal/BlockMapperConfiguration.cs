using AutoMapper;
using NETCore.Terraform.SDK.Schema;

namespace NETCore.Terraform.SDK.Internal
{
    public class BlockMapperConfiguration : Profile
    {
        public BlockMapperConfiguration()
        {
            CreateMap<TerraformBlock, Tfplugin5.Schema.Types.Block>()
                .ForMember(destination => destination.Attributes, options => options.MapFrom(source => source.Attributes))
                .ForMember(destination => destination.BlockTypes, options => options.MapFrom(source => source.NestedBlocks))
                .ForMember(destination => destination.Deprecated, options => options.MapFrom(source => source.Deprecated))
                .ForMember(destination => destination.Description, options => options.MapFrom(source => source.Description))
                .ForMember(destination => destination.DescriptionKind, options => options.MapFrom(source => source.DescriptionKind))
                .ForAllOtherMembers(options => options.Ignore());
        }
    }
}
