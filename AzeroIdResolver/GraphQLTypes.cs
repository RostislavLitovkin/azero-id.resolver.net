using System;

namespace AzeroIdResolver
{
    public class GraphQLVariables
    {
        public object where { get; set; }

        public int limit { get; set; }

        public int offset { get; set; }

        public string orderBy { get; set; }
    }

    public class DomainResponseType
    {
        public List<Domain> Domains { get; set; }
    }

    public class Domain
    {
        public string RegisteredAt { get; set; }
        public Owner Owner { get; set; }
    }

    public class Owner
    {
        public string Id { get; set; }
    }
}

