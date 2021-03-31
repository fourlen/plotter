using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Graph
{
    class NumbersSingleton
    {
        private static NumbersSingleton instance = null;
        private double num = -10;
        private NumbersSingleton() { }
        public static NumbersSingleton GetInstance()
        {
            if (instance == null)
            {
                instance = new NumbersSingleton();
            }
            return instance;
        }
        public double GetNumber()
        {
            return num;
        }
        public void Shift()
        {
            num += 1.0 / 100;
        }
        public void Reload()
        {
            num = -10;
        }
    }
}