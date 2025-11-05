using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Primitives;

using SharedConfiguration.Interfaces;

namespace SharedConfiguration
{
    public class AppConfiguration : IConfiguration, IAppConfiguration
    {
        private readonly IConfiguration _inner;

        public AppConfiguration(IConfiguration inner)
        {
            _inner = inner;
        }

        #region IConfiguration implementation
        public string? this[string key]
        {
            get => _inner[key];
            set => _inner[key] = value;
        }

        public IEnumerable<IConfigurationSection> GetChildren() => _inner.GetChildren();
        public IChangeToken GetReloadToken() => _inner.GetReloadToken();
        public IConfigurationSection GetSection(string key) => _inner.GetSection(key);
        #endregion

        public T GetOptions<T>() where T : IAppOptions, new()
        {
            var sectionName = typeof(T).Name
                .Replace("App", "")
                .Replace("Options", "") + "Options";

            var instance = new T();
            _inner.GetSection(sectionName).Bind(instance);
            return instance;
        }

        public T GetOptions<T>(string sectionName) where T : IAppOptions, new()
        {
            var instance = new T();
            _inner.GetSection(sectionName).Bind(instance);
            return instance;
        }
    }
}
