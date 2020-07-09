using AutoMapper;
using NETCore.Terraform.SDK.Schema;

namespace NETCore.Terraform.SDK.Internal
{
    public class DescriptionKindMapperConfiguration : Profile
    {
        public DescriptionKindMapperConfiguration()
        {
            CreateMap<TerraformDescriptionKind, Tfplugin5.StringKind>().ConvertUsing((source, _) =>
            {
                return source switch
                {
                    TerraformDescriptionKind.Text => Tfplugin5.StringKind.Plain,
                    TerraformDescriptionKind.Markdown => Tfplugin5.StringKind.Markdown,
                    _ => Tfplugin5.StringKind.Plain,
                };
            });
        }
    }
}
