using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace SharedConfiguration.Interfaces
{
    public interface IAppConfiguration
    {
        public T GetOptions<T>() where T : IAppOptions, new();
        public T GetOptions<T>(string sectionName) where T : IAppOptions, new();
    }
}
