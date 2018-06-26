using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Riritia.Core
{
    public class Entity
    {
        String name;
        Guid id;
        public String Name {
            get { return name; }
        }
        public Guid ID {
            get { return id; }
        }
        HashSet<String> aliases;
        public Entity(String name) {
            this.name = name;
            this.id = Guid.NewGuid();
            aliases = new HashSet<string>();
        }
        public Entity(String name, Guid guid)
        {
            aliases = new HashSet<string>();
            this.name = name;
            this.id = guid;
        }
        public bool isAlias(String alias) {
            return aliases.Contains(alias);
        }
        public void addAlias(String alias) {
            aliases.Add(alias);
        }
    }
}
