using BatmanVillains;
using HeroesGeneric;
using Superman;
using SupermanVillains;
using System;
using VillainsGeneric.Interfaces;

namespace AdapterPattern
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Hello World!");

            IVillainGenerator batVillains = new BatVillainGenerator();
            IVillainGenerator supesVillains = new SupesVillainGenerator();

            var response1 = batVillains.getRandomVillain();
            var response2 = supesVillains.getRandomVillain();

            response1.DisplayVillain();
            response2.DisplayVillain();

            //Now, the scenario is, Superman cannot punch a Batman villain directly
            //for the sake of argument. 

            //Adaptee adaptee = new Adaptee();
            //ITarget target = new Adapter(adaptee);

            //Console.WriteLine("Adaptee interface is incompatible with the client.");
            //Console.WriteLine("But with adapter client can call it's method.");

            //Console.WriteLine(target.GetRequest());

            PunchAnyVillain<string> punchAnyVillain = new PunchAnyVillain<string>();
            ISupesPunchesVillain<string> supesPunchesVillain = new PunchingAdapter<string>(punchAnyVillain);

            supesPunchesVillain.OneSolidPunch();

        }
    }
}
