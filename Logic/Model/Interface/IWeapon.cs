using System;
using System.Collections.Generic;
using System.Text;

namespace Logic.Interface
{
    public interface IWeapon
    {
        int DefenseDistance { get; set; }

        int TannangDistance { get; set; }

        int AttackDistance { get; set; }

        int MaxShaCount { get; set; }

        int MaxShaTargetCount { get; set; }
    }
}
