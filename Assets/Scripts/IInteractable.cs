using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assets.Scripts
{
    interface IInteractable
    {
        // Interact with the tile. Returns false if the object is not interactable at the moment
        bool Interact(PlayerController caller);
    }
}
