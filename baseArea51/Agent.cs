using System;
using System.Threading;

namespace Project_demo_Elevator_thread.baseArea51
{
    enum AgentLevel { Confidential, Secret, TopSecret };
    enum FloorType { G, S, T1, T2 };
    public class Agent
    {
        
        public String name { get; set; }
        public Enum level { get; set; }

        public int currentFloor = 0;
        public Elevator Elevator { get; set; }
        
        
        public ManualResetEvent eventOutOfElevator = new ManualResetEvent(false);

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

        //tova za sega ne se izpolzva
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
                            bool isOutOfElev = Elevator.move(this, chosenFloor, chosenFloorNum);
                            if (isOutOfElev == false)
                            {
                              continue;  
                            }
                            eventOutOfElevator.Set();
                            return;
                            break;
                        default:
                            throw new ArgumentException(chosenFloor + " is not supported!");
                    }
            }
        }

        public Enum GetRandomFloor()
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