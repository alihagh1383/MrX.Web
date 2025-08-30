using MongoDB.Driver;

namespace MrX.Web.FileStorage
{
    public class MongoStorage : IStorage
    {
        private readonly IMongoCollection<File> collection;
        private class File
        {
            public string Id { get; set; } = MrX.Web.Security.Random.String(50, true, false, true);
            public string Value { get; set; } = string.Empty;
        }
        public MongoStorage(string conStr, string db, string collection)
        {
            IMongoDatabase Db = new MongoClient(conStr).GetDatabase(db);
            this.collection = Db.GetCollection<File>(collection);
        }
        public bool DeleteFile(string Name)
        {
            return (collection.DeleteOne(Builders<File>.Filter.Eq(p => p.Id, Name)) is { IsAcknowledged: true, DeletedCount: > 0 });
        }

        public bool DownloadFile(string Name, out string Text)
        {
            File? file = collection.Find(p => p.Id == Name).FirstOrDefault();
            if (file is null) { Text = ""; return false; }
            Text = file.Value;
            return true;
        }

        public bool Exists(string Name, out bool exist)
        {
            exist = collection.Find(p => p.Id == Name).Any();
            return true;
        }

        public bool UploadFile(string Name, in string Text)
        {
            collection.InsertOne(new File { Id = Name, Value = Text });
            return true;
        }
    }
}
