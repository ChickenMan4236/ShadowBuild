﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShadowBuild.Exceptions
{
    public class EventNotFoundException : Exception
    {
        public EventNotFoundException(string mess) : base(mess)
        {
            
        }
    }
}