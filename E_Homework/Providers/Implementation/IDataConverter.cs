using E_Homework.DTO.Models;

namespace E_Homework.Providers.Implementation
{
    public interface IDataConverter
    {
        public IEnumerable<CommonDeviceData> ConvertData (Object  data);
    }
}
