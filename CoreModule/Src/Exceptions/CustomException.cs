﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CoreModule.Src
{
    public class CustomException:Exception
    {
        public CustomException(string message):base(message)
        {
            
        }
    }
}
