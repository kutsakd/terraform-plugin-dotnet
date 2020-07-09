using AutoMapper;
using NETCore.Terraform.SDK.Schema;
using ProtocolSchema = Tfplugin5.Schema.Types.NestedBlock.Types;

namespace NETCore.Terraform.SDK.Internal
{
    public class NestingBlockMapperConfiguration : Profile
    {
        public NestingBlockMapperConfiguration()
        {
            CreateMap<TerraformNestingMode, ProtocolSchema.NestingMode>().ConvertUsing((source, _) =>
            {
                return source switch
                {
                    TerraformNestingMode.Single => ProtocolSchema.NestingMode.Single,
                    TerraformNestingMode.Group => ProtocolSchema.NestingMode.Group,
                    TerraformNestingMode.List => ProtocolSchema.NestingMode.List,
                    TerraformNestingMode.Set => ProtocolSchema.NestingMode.Set,
                    TerraformNestingMode.Map => ProtocolSchema.NestingMode.Map,
                    _ => ProtocolSchema.NestingMode.Invalid,
                };
            });
        }
    }
}
