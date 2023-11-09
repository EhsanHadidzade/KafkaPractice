using Mapster;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KafkaProducer.Infrastructure.Mapper
{
    public class Mapper
    {
        public static TDestination Map<TSource, TDestination>(TSource sourceObject) where TSource : class where TDestination : class
        {
            TypeAdapterConfig
                .GlobalSettings
                .NewConfig<TSource, TDestination>()
                .PreserveReference(true)
                .MaxDepth(8);
            return sourceObject.Adapt<TDestination>();
        }

    }
}
