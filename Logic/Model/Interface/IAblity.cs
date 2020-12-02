﻿using System;
using System.Collections.Generic;
using System.Text;

namespace Logic.Model.Interface
{
    /// <summary>
    /// 拥有的能力
    /// </summary>
    public interface IAbility
    {
        /// <summary>
        /// 能提供杀
        /// </summary>
        /// <returns></returns>
        bool CanProvideSha();
        /// <summary>
        /// 能提供闪
        /// </summary>
        /// <returns></returns>
        bool CanProvideShan();
        bool CanProvideJuedou();
        bool CanProvideFenghuolangyan();
        bool CanProvideWanjianqifa();
        bool CanProvideTannangquwu();
    }
}
