using System;
using Microsoft.Extensions.DependencyInjection;

namespace Sitecore.Framework.Diagnostics.Services
{
    [AttributeUsage(AttributeTargets.Class, Inherited = false)]
    public class DiagnosticsServiceDescriptorAttribute : Attribute
    {
        private readonly Type _descriptorType;

        public DiagnosticsServiceDescriptorAttribute(Type descriptorType)
        {
            _descriptorType = descriptorType;
        }

        public string Output(ServiceDescriptor serviceDescriptor, IServiceProvider services)
        {
            var instance = Activator.CreateInstance(_descriptorType);
            var method = _descriptorType.GetMethod("Output");

            if (instance != null)
            {
                return (string)method.Invoke(instance, new object[] { serviceDescriptor, services });
            }

            //No diagnostic data found.
            return null;
        }
    }
}