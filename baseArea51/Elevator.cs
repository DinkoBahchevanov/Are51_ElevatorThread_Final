using System;
using System.Collections.Generic;
using System.Threading;

namespace Project_demo_Elevator_thread.baseArea51
{
   public class Elevator
    {
        public int currentFloor { get; set; }
       
        Semaphore semaphore;

        public List<Agent> _agents;
        public bool leftElevator = true;

        public Elevator()
       {
           semaphore = new Semaphore(1, 1);
           _agents = new List<Agent>();
           currentFloor = 0;
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

       private void LiftingAgent(Agent agent, Enum chosenFloor, int chosenFloorNum)
       {
           leftElevator = true;
          
           if (agent.currentFloor < currentFloor || agent.currentFloor > currentFloor)
           {
               Console.WriteLine("Waiting for elevator to arrive");
               for (int i = 0; i < Math.Abs(currentFloor - agent.currentFloor); i++)
               {
                   Console.WriteLine("...");
                   Thread.Sleep(1000);
               }
           }
           Console.WriteLine("Moving Agent(" + agent.name + ") to " + chosenFloorNum + " floor");
           currentFloor = agent.currentFloor;
           if (currentFloor == chosenFloorNum)
           {
               Console.WriteLine("Agent chose current floor, choosing again...");
               Thread.Sleep(200);
               leftElevator = false;
           }
           //moving the agent
           for (int i = 0; i < Math.Abs(chosenFloorNum - currentFloor); i++)
           {
               Console.WriteLine("...");
               Thread.Sleep(1000);
           }
           currentFloor = chosenFloorNum;
           agent.currentFloor = chosenFloorNum; 
           if ((agent.level.Equals(AgentLevel.Confidential) && !chosenFloor.Equals(FloorType.G)))
           { 
               Console.WriteLine("Agent can't access that floor..."); 
               leftElevator = false;

           } else if (agent.level .Equals( AgentLevel.Secret) && ((chosenFloor.Equals((Enum) FloorType.T1) || chosenFloor.Equals (FloorType.T2))))
           {
               Console.WriteLine("Agent can't access that floor...");
               leftElevator = false;
           }
           else
           {
               Console.WriteLine("Agent(" + agent.name + ") leaves the elevator on " + chosenFloor + " floor with Agent(" + agent.level + ") level");
               Leave(agent);
           }

           if (leftElevator == false)
           {
               chosenFloor = agent.GetRandomFloor();
               chosenFloorNum = Convert.ToInt32(chosenFloor) + 1;
               LiftingAgent(agent, chosenFloor, chosenFloorNum);
           }
       }

       public bool move(Agent agent, Enum ChoserFloor, int chosenFloorNum)
       {
           Thread thread = new Thread(() => LiftingAgent(agent,ChoserFloor, chosenFloorNum));
           thread.Start();
           thread.Join();

           //LiftingAgent(agent, ChoserFloor, chosenFloorNum);
           
           return leftElevator;
       }
    }
}