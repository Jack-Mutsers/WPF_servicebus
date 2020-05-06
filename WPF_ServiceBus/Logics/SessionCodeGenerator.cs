using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
//using Database.Controllers;
using System.Threading.Tasks;

namespace WPF_ServiceBus.Logics
{
    public class SessionCodeGenerator
    {
        public string GenerateSessionCode()
        {
            string sessionCode = "";
            int length = 5;

            // uncomment all commented lines below when implementing it in a asp net applicatie
            //SessionController sessionController = new SessionController();

            StringBuilder str_build = new StringBuilder();
            Random random = new Random();

            char letter;

            bool inUse = true;
            while (inUse == true)
            {
                for (int i = 0; i < length; i++)
                {
                    int val = random.Next(0, 2);
                    if (val == 0)
                    {
                        double flt = random.NextDouble();
                        int shift = Convert.ToInt32(Math.Floor(25 * flt));
                        letter = Convert.ToChar(shift + 65);
                        str_build.Append(letter);
                    }
                    else
                    {
                        int newInt = random.Next(0, 10);
                        str_build.Append(newInt.ToString());
                    }

                }

                sessionCode = str_build.ToString();

                inUse = false; //sessionController.CheckIfSessionExists(sessionCode);
            }

            //sessionController.CreateSession(sessionCode);

            return sessionCode;
        }

    }
}
