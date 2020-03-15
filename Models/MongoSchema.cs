using System;
using System.Collections.Generic;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace eTaskAdvisor.WebApi.Models
{
    public class Client
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string ClientId { get; set; }

        public string Password { get; set; }
        public string Token { get; set; }
    }

    public class Aspect
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string AspectId { get; set; }

        public string Name { get; set; }
        public string Description { get; set; }

        [BsonRepresentation(BsonType.ObjectId)]
        public string AspectTypeId { get; set; }
    }

    public class AspectResult : Aspect
    {
        public AspectType AspectType { get; set; }
    }

    public class AspectTempResult : Aspect
    {
        public IEnumerable<AspectType> AspectType { get; set; }
    }

    public class Factor
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string FactorId { get; set; }

        public string Name { get; set; }
        public string Description { get; set; }
    }

    public class Influence
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string InfluenceId { get; set; }

        public string Name { get; set; }
        public string Description { get; set; }
    }

    public class AspectType
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string AspectTypeId { get; set; }
        public string Name { get; set; }
        public string Value { get; set; }
        public string Description { get; set; }
    }

    public class Affect
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string AffectId { get; set; }

        [BsonRepresentation(BsonType.ObjectId)]
        public string AspectId { get; set; }

        [BsonRepresentation(BsonType.ObjectId)]
        public string FactorId { get; set; }

        [BsonRepresentation(BsonType.ObjectId)]
        public string InfluenceId { get; set; }

        public string Source { get; set; }
        public string Description { get; set; }
    }

    public class AffectTempResult : Affect
    {
        public IEnumerable<Aspect> Aspect { get; set; }
        public IEnumerable<AspectType> AspectType { get; set; }
        public IEnumerable<Factor> Factor { get; set; }
        public IEnumerable<Influence> Influence { get; set; }
    }

    public class AffectResult : Affect
    {
        public Aspect Aspect { get; set; }
        public AspectType AspectType { get; set; }
        public Factor Factor { get; set; }
        public Influence Influence { get; set; }
    }

    public class ClientTask
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string TaskId { get; set; }

        [BsonRepresentation(BsonType.ObjectId)]
        public string ClientId { get; set; }

        [BsonRepresentation(BsonType.ObjectId)]
        public string AspectId { get; set; }

        public string Subject { get; set; }
        public DateTime At { get; set; }
        public int Duration { get; set; }
        public bool Done { get; set; }
    }

    public class ClientTaskTempResult : ClientTask
    {
        public IEnumerable<Aspect> Aspect { get; set; }
        public IEnumerable<AspectType> AspectType { get; set; }
    }
    
    public class ClientTaskResult : ClientTask
    {
        public Aspect Aspect { get; set; }
        public AspectType AspectType { get; set; }
        public string AtFormatted => At.ToString("g");
    }
}