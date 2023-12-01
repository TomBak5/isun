using isun.Interfaces;
using isun.Models.ResponseModels;
using LiteDB;

namespace isun.Implementations
{
    public class LightDbDataSaver : IDataSaver
    {
        private readonly WeatherResponse _data;
        public LightDbDataSaver(WeatherResponse data)
        {
            _data = data;
        }

        public IEnumerable<WeatherResponse> GetData()
        {
            using (var db = new LiteDatabase("DataBase.db"))
            {
                var collection = db.GetCollection<WeatherResponse>("weathers");

                return collection.FindAll();
            }
        }

        public void Write()
        {
            using (var db = new LiteDatabase("DataBase.db"))
            {
                var collection = db.GetCollection<WeatherResponse>("weathers");

                collection.Insert(_data);
            }
        }
    }
}
