using System;
using System.Collections.Generic;
using System.Text;

namespace SOLIDDesignPrinciples
{
    //In this class, I am trying to look into Single Responsibility Principle

    public class SingleResponsibilityPrinciple
    {
        //this is just an empty class which came with the file creation
        //this is not actually used anywhere so it does not contain anything.
    }

    //Imagine a monkey that is assignment to turn on/off the lights
    //This class follows the SingleResponsibilityPrinciple
    //Look at TurnOnOffLightsBad class for an opposite, bad implementation example.
    public class TurnOnOffLights
    {
        private bool LightStatus { set; get; }

        public TurnOnOffLights()
        {
            //by default, the lights are off
            LightStatus = false;
        }

        public void TurnOnTheLightsMonkey()
        {
            LightStatus = true;
        }

        public void TurnOffTheLightsMonkey()
        {
            LightStatus = false;
        }

        public string MessageAboutLightStatus()
        {
            string messagetoshow = "";

            if (LightStatus == true)
            {
                messagetoshow = "The Light is currently turned ON";
            }
            if (LightStatus == false)
            {
                messagetoshow = "The Light is currently turned OFF";
            }

            return messagetoshow;
        }

    }

    //This demonstrates a bad implementation of a class
    //which DOES NOT FOLLOW the SingleResponsibilityPrinciple
    //Look at TurnOnOffLights for this done correctly
    public class TurnOnOffLightsBad
    {
        private bool LightStatus { set; get; }
        private bool LightFaulty { set; get; }

        public TurnOnOffLightsBad()
        {
            //by default, the lights are off
            LightStatus = false;
            //by default, Light is Faulty
            LightFaulty = true;
        }

        public void TurnOnTheLightsMonkey()
        {
            LightStatus = true;
        }

        public void TurnOffTheLightsMonkey()
        {
            LightStatus = false;
        }

        public string MessageAboutLightStatus()
        {
            string messagetoshow = "";

            if (LightStatus == true)
            {
                messagetoshow = "The Light is currently turned ON";
            }
            if (LightStatus == false)
            {
                messagetoshow = "The Light is currently turned OFF";
            }

            return messagetoshow;
        }

        //this function breaks the Single Responsibility Principle
        //this class is about Turning on and off lights
        //it should just focus on that.
        //changing bulbs should be done by a separate class.
        public void ChangeTheBulbsMonkey()
        {
            //I am assuming that this is set to faulty
            //by some other method or class or from somewhere else
            if(LightFaulty == true)
            {
                //do the change bulb action.

                //set faulty status to reflect the change
                LightFaulty = false;
            }
        }
    }
}
