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
        public bool isProcess = true;
        public Process()
        {

        }

        public void startProcess()
        {
            while (isProcess)
            {
                long has = hash.getHashScreen();
                Console.WriteLine(has);
            }
        }
    }
}
