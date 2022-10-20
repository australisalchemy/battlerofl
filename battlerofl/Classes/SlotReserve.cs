using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace battlerofl
{
    public static class SlotReserve
    {
        public enum SlotReservation
        {
            SERVER_FULL,
            SERVER_CHANGING_MAPS,
            TOO_MANY_ATTEMPTS,
            JOINING_GAME,
            UNKNOWN_ERROR
        }

        public static SlotReservation reserveSlot
        {
            get;
            set;
        }
    }
}
