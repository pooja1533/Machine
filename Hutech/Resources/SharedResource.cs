using Elfie.Serialization;
using Irony;
using Microsoft.Extensions.Localization;
using System.Reflection;
using System.Resources;
namespace Hutech.Resources
{
    public class SharedResource
    {
        
    }
    public class LanguageService
    {
        private readonly IStringLocalizer _localizer;
        public LanguageService(IStringLocalizerFactory factory)
        {
            var type = typeof(SharedResource);
            var assemblyName = new AssemblyName(type.GetTypeInfo().Assembly.FullName);
            _localizer = factory.Create("SharedResource", assemblyName.Name);
        }

        public string Getkey(string key)
        {
            var keyValue = _localizer.GetString(key);

            return keyValue.ToString();
        }
    }
}
