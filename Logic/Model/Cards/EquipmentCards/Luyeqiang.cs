using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Logic.Cards;
using Logic.Enums;
using Logic.GameLevel;
using Logic.Log;
using Logic.Model.Cards.BaseCards;
using Logic.Model.Cards.Interface;
using Logic.Model.Cards.MutedCards;
using Logic.Model.Enums;
using Logic.Model.RequestResponse.Request;
using Logic.Model.Skill.Interface;

namespace Logic.Model.Cards.EquipmentCards
{
    /// <summary>
    /// 芦叶枪
    /// </summary>
    public class Luyeqiang : EquipmentBase, IWeapon, ISkillButton
    {
        public Luyeqiang()
        {
            this.Description = "芦叶枪";
            this.Name = "Luyeqiang";
            this.DisplayName = "芦叶枪";
            this.Image = "/Resources/card/equipment/card_luyeqiang.jpg";
            BaseAttackFactor.ShaDistance = 3;
        }

        protected override async Task OnEquip()
        {
            PlayerContext.Player.CurrentPlayerHero.BaseAttackFactor.ShaDistance +=
                BaseAttackFactor.ShaDistance - 1;

            await Task.FromResult(0);
        }

        protected override async Task OnUnEquip()
        {
            PlayerContext.Player.CurrentPlayerHero.BaseAttackFactor.ShaDistance -=
                BaseAttackFactor.ShaDistance - 1;
            await Task.FromResult(0);
        }

        public override bool CanProvideSha()
        {
            return true;
        }

        public bool IsEnabled()
        {
            //能够出杀的时候
            return Sha.CanBePlayed(PlayerContext); ;
        }

        public SkillButtonInfo GetButtonInfo()
        {
            return new SkillButtonInfo()
            {
                Text = "芦叶枪",
                Description = "",
                SkillType = Enums.SkillTypeEnum.Luyeqiang,
                OnClick = async (context, roundContext, responseContext) =>
                {
                    var res = await PlayerContext.Player.ResponseCard(new CardRequestContext()
                    {
                        CardScope = CardScopeEnum.InHandAndEquipment,
                        AttackType = AttackTypeEnum.Luyeqiang,
                        MaxCardCountToPlay = 2,
                        MinCardCountToPlay = 2,
                        SrcPlayer = PlayerContext.Player,
                        TargetPlayers = new List<Player.Player>() { PlayerContext.Player }
                    }, responseContext, roundContext);
                    if (res.ResponseResult == ResponseResultEnum.Success || (res.Cards != null && res.Cards.Count == 2))
                    {
                        responseContext.ResponseResult = ResponseResultEnum.Success;
                        responseContext.Cards = new List<CardBase>()
                        {
                            new ChangedCard(res.Cards,new Sha()
                            {
                                CardId = -1,
                                Color = res.Cards.Any(c=>c.Color==CardColorEnum.Red)?CardColorEnum.Red:CardColorEnum.Black,
                                FlowerKind=res.Cards.First().FlowerKind,
                            }.AttachPlayerContext(PlayerContext)
                            ){CardChangeType = CardChangeTypeEnum.Combined}.AttachPlayerContext(PlayerContext)
                        };
                        Console.WriteLine($"{PlayerContext.Player.PlayerId}的【{PlayerContext.Player.CurrentPlayerHero.Hero.DisplayName}】打出两张牌{String.Join(",", res.Cards?.Select(p => p.ToString()) ?? new List<string>())}当做杀来用。");

                        PlayerContext.GameLevel.LogManager.LogAction(
                                   new RichTextParagraph(
                                   new RichTextWrapper($"{PlayerContext.Player.PlayerId}【{PlayerContext.Player.CurrentPlayerHero.Hero.DisplayName}】", RichTextWrapper.GetColor(ColorEnum.Blue)),
                                   new RichTextWrapper("打出两张牌"),
                                   new RichTextWrapper(String.Join(",", res.Cards?.Select(p => p.ToString()) ?? new List<string>()), RichTextWrapper.GetColor(ColorEnum.Red)),
                                   new RichTextWrapper("当做“"),
                                   new RichTextWrapper("杀", RichTextWrapper.GetColor(ColorEnum.Red)),
                                   new RichTextWrapper("”来用"),
                                   new RichTextWrapper("。")
                                ));

                    }
                    else
                    {
                        responseContext.ResponseResult = ResponseResultEnum.Failed;
                    }

                }
            };
        }
    }
}
