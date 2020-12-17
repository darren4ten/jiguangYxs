using System;
using System.Threading.Tasks;
using Logic.Cards;
using Logic.GameLevel;
using Logic.Model.Player;

namespace Logic.Model.Cards.BaseCards
{
    /// <summary>
    /// 闪
    /// </summary>
    public class Shan : CardBase
    {
        public Shan()
        {
            this.Description = "闪";
            this.Name = "Shan";
            this.DisplayName = "闪";
        }

        public static bool CanBePlayed(PlayerContext playerContext)
        {
            if (CanBeidongPlayCard<Shan>(playerContext))
            {
                return true;
            }
            return false;
        }

        /// <summary>
        /// 闪不能被主动打出
        /// </summary>
        /// <returns></returns>
        public override bool CanBePlayed()
        {
            return CanBePlayed(PlayerContext);
        }

        public override Task Popup()
        {
            throw new NotImplementedException();
        }
    }
}
