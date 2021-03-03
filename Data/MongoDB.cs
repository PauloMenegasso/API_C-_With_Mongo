using System;
using api.Data.Collections;
using Microsoft.Extensions.Configuration;
using MongoDB.Bson.Serialization;
using MongoDB.Bson.Serialization.Conventions;
using MongoDB.Driver;

namespace Api.Data
{
	public class MongoDB
	{
		public IMongoDatabase DB { get; }

		public MongoDB(IConfiguration configuration)
		{
			try{
				var settings = MongoClientSettings.FromUrl(new MongoUrl(configuration["ConnectionStrings:MongoDB"]));
				var client = new MongoClient(settings);
				DB = client.GetDatabase(configuration["BankName"]);
				MapClasses();
			}
			catch (Exception ex)
			{
				throw new MongoException("It was not possible to connect to MongoDB", ex);
			}
		}

		internal IMongoCollection<T> GetCollections<T>(string v)
		{
			throw new NotImplementedException();
		}

		private void MapClasses()
		{
			var conventionPack = new ConventionPack { new CamelCaseElementNameConvention() };
			ConventionRegistry.Register("camelCase", conventionPack, t => true);

			if (!BsonClassMap.IsClassMapRegistered(typeof(Infectado)))
			{
				BsonClassMap.RegisterClassMap<Infectado>(i =>
				{
					i.AutoMap();
					i.SetIgnoreExtraElements(true);
				});
			}
		}
	}
}