using System;
using System.Collections.Generic;
using Game.Combat;
using UnityEngine;
using UnityEngine.UI;

namespace Game.Enemies
{
    public class HpBar : MonoBehaviour
    {
        //data
        [SerializeField] private Health targetHealth;
        [SerializeField] private Image filler;
        
        //components
        private Slider slider;
        
        //vars
        private static readonly Color[] Colors =
        {
            new Color(0,221f/255,25f/255),
            new Color(220f/255,1,22f/255),
            new Color(1,106f/255,0),
            new Color(193f/255,0,0)
        };
 
        //all 4 quarter values
        private float hp4;
        private float hp3;
        private float hp2;
        private float hp1;
        
        private void Start()
        {
            slider = GetComponent<Slider>();
            
            slider.maxValue = targetHealth.GetHealth();
            slider.value = slider.maxValue;

            //get quarter values
            hp1 = slider.maxValue / 4;      //  1/4
            hp2 = slider.maxValue / 4 * 2;  //  2/4
            hp3 = slider.maxValue / 4 * 3;  //  3/4
            hp4 = slider.maxValue;          //  4/4
            
            UpdateColor(slider.maxValue);
        }
        
        private void UpdateColor(float value)
        {
            if (value <= hp4 && value > hp3)
            {
                filler.color = Colors[0];
            }
            else if (value <= hp3 && value > hp2)
            {
                filler.color = Colors[1];
            }
            else if (value <= hp2 && value > hp1)
            {
                filler.color = Colors[2];
            }
            else
            {
                filler.color = Colors[3];
            }
        }

        public void UpdateBar(float value)
        {
            slider.value = value;
            UpdateColor(value);
        }
        
    }
}