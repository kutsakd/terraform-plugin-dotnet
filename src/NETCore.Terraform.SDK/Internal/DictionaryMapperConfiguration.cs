using AutoMapper;
using Google.Protobuf.Collections;
using System.Collections.Generic;

namespace NETCore.Terraform.SDK.Internal
{
    public class DictionaryMapperConfiguration : Profile
    {
        public DictionaryMapperConfiguration()
        {
            CreateMap(typeof(IDictionary<,>), typeof(RepeatedField<>)).ConvertUsing(typeof(DictionaryConverter<,,>));
        }

        private class DictionaryConverter<TKey, TSource, TDestination> :
            ITypeConverter<IDictionary<TKey, TSource>, RepeatedField<TDestination>>
        {
            public RepeatedField<TDestination> Convert(
                IDictionary<TKey, TSource> dictionary, RepeatedField<TDestination> destination, ResolutionContext context)
            {
                destination ??= new RepeatedField<TDestination>();
                foreach (var sourceInstance in dictionary)
                {
                    var destinationInstance = context.Mapper.Map<TDestination>(sourceInstance.Value);
                    destinationInstance = context.Mapper.Map(sourceInstance.Key, destinationInstance);
                    destination.Add(destinationInstance);
                }

                return destination;
            }
        }
    }
}
