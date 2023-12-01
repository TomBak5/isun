using isun.Interfaces;

namespace isun.Tools
{
    public class DataProcessor
    {
        public static void Save(IDataSaver dataSaver)
        {
            dataSaver.Write();
        }
    }
}
