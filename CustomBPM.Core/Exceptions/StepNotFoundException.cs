// --------------------------------------------------------------------------------------------------------------------
// <copyright file="StepNotFoundException.cs" company="Ingenius Systems">
//   Copyright (c) Ingenius Systems
//   Create on 11:14:36 by Еламан Абдуллин
// </copyright>
// <summary>
//   Defines the StepNotFoundException type.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

using System;

namespace CustomBPM.Core.Exceptions
{
    public class StepNotFoundException:Exception
    {
        public StepNotFoundException()
            : base("Шаг не найден")
        {
            
        }
         
    }
}