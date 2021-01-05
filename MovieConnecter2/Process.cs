using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MovieConnecter2
{
    class Process
    {
        public HashImage hash = new HashImage();

        public Process()
        {

        }

        public void startProcess()
        {
            while (true)
            {
                int has = hash.getHashScreen();
                Console.WriteLine(has);
            }
        }
    }
}
