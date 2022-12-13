using System;
using System.Linq;

namespace ProSimpAuth
{
    public record MySecrets
    {
        public string Visitor { get; set; } = Guid.NewGuid().ToString();
        public string User { get; set; } = Guid.NewGuid().ToString();
        public string Admin { get; set; } = Guid.NewGuid().ToString();
    }
}
