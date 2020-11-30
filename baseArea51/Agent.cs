using System;
using System.Threading;
using Project_demo_Elevator_thread.BarClasses;

namespace Project_demo_Elevator_thread.baseArea51
{
    enum AgentLevel { Confidential, Secret, TopSecret };
    enum FloorType { G, S, T1, T2, Stairs };
    public class Agent
    {
        
        public String name { get; set; }
        public Enum level { get; set; }

        private int currentFloor = 0;
        public Elevator Elevator { get; set; }
        
        
        ManualResetEvent eventOutOfElevator = new ManualResetEvent(false);

        private void EnterElevatorEvent ()
        {
            Console.WriteLine(name + " starting the trip");
            Thread.Sleep(500);
        }
        private void choosingAction()
        {
            EnterElevatorEvent(); 
            while (true)
            {

                if (currentFloor != 0)
                {
                    eventOutOfElevator.Set();
                    return;
                }
                //here if agent chooses a floor, it means he decides to use the elevator
                var chosenAction = GetRandomFloor();
                switch (chosenAction)
                {
                    case FloorType.G: 
                    case FloorType.S: 
                    case FloorType.T1:
                    case FloorType.T2:
                        EnteringElevator();
                        break;
                    default:
                        throw new ArgumentException(chosenAction + " action is not supported!");
                }
            }
        }

        private void EnteringElevator()
        {
            Enum chosenFloor = GetRandomFloor(); 
            Console.WriteLine(name + " Waits for his order");
            Elevator.PressButtonAndStartElevator(this);
            Console.WriteLine(name + " pressed the button for the elevator");
            int chosenFloorNum = 0;
            
            while (true)
            {
                chosenFloor = GetRandomFloor();
                if (chosenFloor.Equals(FloorType.G))
                {
                    chosenFloorNum = 1;
                } else if (chosenFloor.Equals(FloorType.S))
                {
                    chosenFloorNum = 2;
                } else if (chosenFloor.Equals(FloorType.T1))
                {
                    chosenFloorNum = 3;
                } else if (chosenFloor.Equals(FloorType.T2))
                {
                    chosenFloorNum = 4;
                }
                switch (chosenFloor)
                    {
                        case FloorType.G: 
                        case FloorType.S: 
                        case FloorType.T1: 
                        case FloorType.T2:
                            // simulating the waiting of the elevator to arrive
                            if (currentFloor < Elevator.currentFloor || currentFloor > Elevator.currentFloor)
                            {
                                Console.WriteLine("Waiting for elevator to arrive");
                                for (int i = 0; i < Math.Abs(Elevator.currentFloor - currentFloor); i++)
                                {
                                    Console.WriteLine("...");
                                    Thread.Sleep(1000);
                                }
                            }
                            Console.WriteLine("Moving Agent(" + name + ") to " + chosenFloorNum + " floor");
                            Elevator.currentFloor = currentFloor;
                            if (Elevator.currentFloor == chosenFloorNum)
                            {
                                Console.WriteLine("Agent chose current floor, choosing again...");
                                Thread.Sleep(200);
                                continue;
                            }
                            //moving the agent
                            for (int i = 0; i < Math.Abs(chosenFloorNum - Elevator.currentFloor); i++)
                            {
                                Console.WriteLine("...");
                                Thread.Sleep(1000);
                            }

                            Elevator.currentFloor = chosenFloorNum;
                            currentFloor = chosenFloorNum;
                            if ((this.level.Equals(AgentLevel.Confidential) && !chosenFloor.Equals(FloorType.G)))
                            {
                                Console.WriteLine("Agent can't access that floor...");
                                continue;
                            } else if (this.level .Equals( AgentLevel.Secret) && ((chosenFloor.Equals((Enum) FloorType.T1) 
                                || chosenFloor.Equals (FloorType.T2))))
                            {
                                Console.WriteLine("Agent can't access that floor...");
                                continue;
                            }
                            else
                            {
                                Console.WriteLine("Agent(" + name + ") leaves the elevator on " + chosenFloor + " floor with Agent(" + level + ") level");
                                Elevator.Leave(this);
                                return;

                            }
                            break;
                        default:
                            throw new ArgumentException(chosenFloor + " is not supported!");
                    }
            }
        }

        private Enum GetRandomFloor()
        {
            Random random = new Random();
            Enum floorType = null;
            switch (random.Next(4) + 1)
            {
                case 1:
                    floorType = FloorType.G;
                    break;
                case 2:
                    floorType = FloorType.S;
                    break;
                case 3:
                    floorType = FloorType.T1;
                    break;
                case 4:
                    floorType = FloorType.T2;
                    break;
            }
            return floorType;
        }

        public void StartTrip()
        {
            Thread t = new Thread(choosingAction);
            t.Start();
        }
        public bool OutOfArea
        {
            get
            {
                return eventOutOfElevator.WaitOne(0);
            }
        }
    }
}