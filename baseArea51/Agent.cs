using System;
using System.Threading;
using Project_demo_Elevator_thread.BarClasses;

namespace Project_demo_Elevator_thread.baseArea51
{
    enum AgentLevel { Confidential, Secret, TopSecret };
    public class Agent
    {
        
        public String name { get; set; }
        public Enum level { get; set; }

        private int currentFloor = 0;
         
        Semaphore _semaphore = new Semaphore(1, 1); 
         public Elevator Elevator { get; set; }
        
        
        ManualResetEvent eventAtHome = new ManualResetEvent(false);

        private void EnterElevatorEvent ()
        {
            Console.WriteLine(name + " starting the trip");
            Thread.Sleep(500);
        }
        private void StartTripInternal()
        {
            EnterElevatorEvent(); 
            while (true)
            {

                if (currentFloor != 0)
                {
                    eventAtHome.Set();
                    return;
                }
                var chosenAction = GetRandomFloor();
                switch (chosenAction)
                {
                    case FloorType.G: 
                    case FloorType.S: 
                    case FloorType.T1:
                    case FloorType.T2:
                        EnteringElevator();
                        break;
                    case FloorType.Stairs:
                        Console.WriteLine(name + " goes outside using Stairs.");
                        eventAtHome.Set();
                        return;
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
            int ChosenFloorNum = 0;
            while (true)
            {
                chosenFloor = GetRandomFloor();
                if (chosenFloor.Equals(FloorType.G))
                {
                    ChosenFloorNum = 1;
                } else if (chosenFloor.Equals(FloorType.S))
                {
                    ChosenFloorNum = 2;
                } else if (chosenFloor.Equals(FloorType.T1))
                {
                    ChosenFloorNum = 3;
                } else if (chosenFloor.Equals(FloorType.T2))
                {
                    ChosenFloorNum = 4;
                }
                switch (chosenFloor)
                    {
                        case FloorType.G: case FloorType.S: case FloorType.T1: case FloorType.T2:
                            if (currentFloor < Elevator.currentFloor || currentFloor > Elevator.currentFloor)
                            {
                                Console.WriteLine("Waiting for elevator to arrive");
                                for (int i = 0; i < Math.Abs(Elevator.currentFloor - currentFloor); i++)
                                {
                                    Thread.Sleep(1000);
                                }
                            }
                            Console.WriteLine("Moving Agent(" + name + ") to " + ChosenFloorNum + " floor");
                            Elevator.currentFloor = currentFloor;
                            if (Elevator.currentFloor == ChosenFloorNum)
                            {
                                Console.WriteLine("Agent chose current floor, choosing again...");
                                chosenFloor = GetRandomFloor();
                                Thread.Sleep(200);
                                continue;
                            }
                            for (int i = 0; i < Math.Abs(ChosenFloorNum - Elevator.currentFloor); i++)
                            {
                                Thread.Sleep(1000);
                            }

                            Elevator.currentFloor = ChosenFloorNum;
                            currentFloor = ChosenFloorNum;
                            if ((this.level.Equals(AgentLevel.Confidential) && !chosenFloor.Equals(FloorType.G)))
                            {
                                Console.WriteLine("Agent can't access that floor...");
                                chosenFloor = GetRandomFloor();
                                continue;
                            } else if (this.level .Equals( AgentLevel.Secret) && ((chosenFloor.Equals((Enum) FloorType.T1) 
                                || chosenFloor.Equals (FloorType.T2))))
                            {
                                Console.WriteLine("Agent can't access that floor...");
                                chosenFloor = GetRandomFloor();
                                continue;
                            }
                            else
                            {
                                Console.WriteLine(name + " leaves the elevator on " + chosenFloor + " floor with Agent(" + level + ") level");
                                chosenFloor = GetRandomFloor();
                                Elevator.Leave(this);
                                return;

                            }
                            break;
                        case FloorType.Stairs:
                            Elevator.Leave(this);
                            return;
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
                default: floorType = FloorType.Stairs;
                    break;
            }

            

            return floorType;
        }

        public void StartTrip()
        {
            Thread t = new Thread(StartTripInternal);
            t.Start();
        }
        public bool OutOfArea
        {
            get
            {
                return eventAtHome.WaitOne(0);
            }
        }
    }
    }