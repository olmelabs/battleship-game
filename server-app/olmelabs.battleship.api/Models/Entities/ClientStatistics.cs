using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson.Serialization.Options;
using System.Collections.Generic;

namespace olmelabs.battleship.api.Models.Entities
{
    public class ClientStatistics : BsonBase
    {
        public ClientStatistics()
        {
            CellHits = new Dictionary<int, int>();
        }

        /// <summary>
        /// contains cellid / hitcount pairs.
        /// </summary>
        [BsonDictionaryOptions(DictionaryRepresentation.ArrayOfArrays)]
        public Dictionary<int, int> CellHits { get; set; }

        public static ClientStatistics CreateNew()
        {
            ClientStatistics stat = new ClientStatistics();
            for (int i = 0; i < 100; i++)
            {
                stat.CellHits.Add(i, 0);
            }
            return stat;
        }
    }
}
