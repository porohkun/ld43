using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Game
{
    public abstract class UsableItem : Item
    {
        public abstract void Use(Player player);

        public abstract void Refresh();
    }
}
