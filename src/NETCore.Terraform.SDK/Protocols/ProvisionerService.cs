using Grpc.Core;
using System.Threading.Tasks;
using Tfplugin5;

namespace NETCore.Terraform.SDK.Protocols
{
    public class ProvisionerService : Provisioner.ProvisionerBase
    {
        public override Task<GetProvisionerSchema.Types.Response> GetSchema(
            GetProvisionerSchema.Types.Request request, ServerCallContext context)
        {
            return base.GetSchema(request, context);
        }

        public override Task ProvisionResource(ProvisionResource.Types.Request request,
            IServerStreamWriter<ProvisionResource.Types.Response> responseStream, ServerCallContext context)
        {
            return base.ProvisionResource(request, responseStream, context);
        }

        public override Task<Stop.Types.Response> Stop(Stop.Types.Request request, ServerCallContext context)
        {
            return base.Stop(request, context);
        }

        public override Task<ValidateProvisionerConfig.Types.Response> ValidateProvisionerConfig(
            ValidateProvisionerConfig.Types.Request request, ServerCallContext context)
        {
            return base.ValidateProvisionerConfig(request, context);
        }
    }
}
