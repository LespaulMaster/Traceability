using System;
using System.Collections.Generic;
using System.Text;

namespace ForkLift
{
    class StanjeSistema
    {
        int state;
        public String ime;
        public String error;
        //TODO ubaciti entitete koji su povezani za terminal (radnik, propis, regal...)
        public StanjeSistema()
        {
            //pocetno stanje, odjavljen radnik
            state = 0;
            
            
            
        }

        public bool goToNextState(String barcode)
        {
            bool status = true;

            switch (state)
            {
                case 0:
                    if (barcode == "1")
                    {
                        ime = "Marko Krstic";
                    }
                    else
                    {
                        error = "Prijavite se na terminal";
                        status = false;
                    }
                    break;
            }

            return status;
            
        }
    }
}
