using System;
using System.Collections.Generic;
using System.Text;
using Logic.Model.RequestResponse.Request;

namespace Logic.Model.Cards.Interface
{
    /// <summary>
    /// 需要选择目标
    /// </summary>
    public interface INeedTargets
    {
        SelectedTargetsRequest GetSelectTargetRequest();
    }
}
