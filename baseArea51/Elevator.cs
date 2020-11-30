﻿using System.Collections.Generic;
using System.Threading;
using Project_demo_Elevator_thread.BarClasses;

namespace Project_demo_Elevator_thread.baseArea51
{
   public class Elevator
    {
        public int currentFloor { get; set; }
       
       Semaphore semaphore;

       List<Agent> _agents;

       public Elevator()
       {
           semaphore = new Semaphore(1, 10);
           _agents = new List<Agent>();
       }

       public void PressButtonAndStartElevator(Agent agent)
       {
           semaphore.WaitOne();
           lock (_agents)
           {
               _agents.Add(agent);
           }
       }

       public void Leave(Agent agent)
       {
           semaphore.Release();
           lock (_agents)
           {
               _agents.Remove(agent);
           }
       }
    }
}