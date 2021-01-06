using System;
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
        /// 能提供釜底抽薪
        /// </summary>
        /// <returns></returns>
        bool CanProvideFuidichouxin();

        /// <summary>
        /// 能够提供借刀杀人
        /// </summary>
        /// <returns></returns>
        bool CanProvideJiedaosharen();
        /// <summary>
        /// 能提供闪
        /// </summary>
        /// <returns></returns>
        bool CanProvideShan();

        bool CanProvideYao();
        bool CanProviderWuxiekeji();

        bool CanProvideJuedou();
        bool CanProvideFenghuolangyan();
        bool CanProvideWanjianqifa();
        bool CanProvideTannangquwu();

        bool CanProvideWuzhongshengyou();

        bool CanProvideHudadiweilao();

        bool CanProvideXiuyangshengxi();
    }
}
