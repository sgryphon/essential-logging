using System.Runtime.Serialization;

namespace Essential.LoggerProvider.Ecs
{
    public class Agent
    {
        public Agent(string type, string version)
        {
            Type = type;
            Version = version;
        }

        // agent.type = "Essential.LoggerProvider.Elasticsearch"
        [DataMember(Name = "type")] public string Type { get; }

        // agent.version
        [DataMember(Name = "version")] public string Version { get; }
    }
}
