using System;

namespace Project_demo_Elevator_thread.baseArea51
{
    enum FloorType { G, S, T1, T2, Stairs };
    public class Floor
    {
        private Enum type;
        private int number;

        public Floor(Enum type, int number)
        {
            this.number = number;
            this.type = type;
        }
    }
}