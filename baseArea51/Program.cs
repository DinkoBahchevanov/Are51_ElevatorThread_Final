using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using Project_demo_Elevator_thread.BarClasses;

namespace Project_demo_Elevator_thread.baseArea51
{
    public class Program
    {
        static  List<Floor> floors = new List<Floor> {{new Floor(FloorType.G, 1)}
            ,{new Floor(FloorType.S, 2)}, new Floor(FloorType.T1, 3), new Floor(FloorType.T2, 4)};

        static Elevator elevator = new Elevator(floors);
        static void Main(string[] args)
        {
           
        //    var agents = 
          //      Enumerable.Range(1, 10)
          //          .Select(i => new Agent { name = i.ToString(), level = generateLevel(), Elevator = elevator })
         //           .ToList();

         List<Agent> agents = new List<Agent>();
            for (int i = 1; i <= 10; i++)
            {
                agents.Add(new Agent {name = i.ToString(), level = generateLevel(), Elevator = elevator});
            }
            
            foreach (var agent in agents)
            {
                agent.StartTrip();
            }

            while (agents.Any (a => !a.OutOfArea))
            {

            }
            Console.WriteLine("Show is over.");
            Console.ReadLine();
        }

        public static Enum generateLevel()
        {
            Random random = new Random();

            Enum level = null;
            Thread.Sleep(200);

            switch (random.Next(3) + 1)
            {
                case 1:
                     level = AgentLevel.Confidential;
                    break;
                case 2:
                     level =  AgentLevel.Secret;
                    break;
                case 3:
                     level =  AgentLevel.TopSecret;
                    break;

            }

            return level;
        }
        
    }
    }
