using System;
using System.Collections.Generic;
using System.ComponentModel.Design;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Logic.Cards;
using Logic.Enums;
using Logic.GameLevel;
using Logic.GameLevel.Panel;
using Logic.Model.Cards.BaseCards;
using Logic.Model.Cards.EquipmentCards;
using Logic.Model.Cards.EquipmentCards.Defense;
using Logic.Model.Cards.Interface;
using Logic.Model.Cards.JinlangCards;
using Logic.Model.Cards.MutedCards;
using Logic.Model.Enums;
using Logic.Model.Interface;
using Logic.Model.Mark;
using Logic.Model.Player;
using Logic.Model.RequestResponse.Request;
using Logic.Model.RequestResponse.Response;
using Logic.Model.Skill.Interface;

namespace Logic.ActionManger
{
    /// <summary>
    /// AI行为管理器 
    /// </summary>
    public class AiActionManager : ActionManagerBase
    {
        public AiActionManager() { }

        public AiActionManager(PlayerContext playContext) : base(playContext)
        {
        }

        public override async Task<bool> OnRequestTriggerSkill(SkillTypeEnum skillType, CardRequestContext cardRequestContext)
        {
            if (skillType == SkillTypeEnum.Bolangchui)
            {
                return ShouldTriggerSkill_Bolangchui(cardRequestContext);
            }
            else if (skillType == SkillTypeEnum.Longlindao)
            {
                return ShouldTriggerSkill_Longlindao(cardRequestContext);
            }
            else if (skillType == SkillTypeEnum.Luyeqiang)
            {
                return ShouldTriggerSkill_Luyeqiang(cardRequestContext);
            }
            else if (skillType == SkillTypeEnum.Panlonggun)
            {
                return ShouldTriggerSkill_Panlonggun(cardRequestContext);
            }
            else if (skillType == SkillTypeEnum.Yuruyi)
            {
                return ShouldTriggerSkill_Yuruyi(cardRequestContext);
            }


            return await Task.FromResult(false);
        }

        public override async Task<CardResponseContext> OnParallelRequestResponseCard(CardRequestContext cardRequestContext)
        {
            var res = await GetResponseCardByCardType_GroupRequestWithConfirm(cardRequestContext);
            if (res != null && cardRequestContext.RequestCard is Wuxiekeji && res.Cards?.Any() == true)
            {
                res.ResponseResult = ResponseResultEnum.Wuxiekeji;
            }
            return res;
        }

        /// <summary>
        /// 被动出牌
        /// </summary>
        /// <param name="cardRequestContext"></param>
        /// <returns></returns>
        public override async Task<CardResponseContext> OnRequestResponseCard(CardRequestContext cardRequestContext)
        {
            await Task.Delay(500);
            //任意牌，则检查手牌、装备牌是否可以出牌
            CardResponseContext response = null;
            //处理选择牌的请求
            if (cardRequestContext.AttackType == AttackTypeEnum.SelectCard)
            {
                return await OnRequestPickCardFromPanel(new PickCardFromPanelRequest()
                {
                    MaxCount = cardRequestContext.MaxCardCountToPlay,
                    MinCount = cardRequestContext.MinCardCountToPlay,
                    Panel = cardRequestContext.Panel,
                    RequestId = cardRequestContext.RequestId
                });
            }
            else if (cardRequestContext.AttackType == AttackTypeEnum.Jiedaosharen)
            {
                //处理借刀杀人
                return await GetResponseCardByCardType_Jiedaosharen(cardRequestContext);
            }


            //处理其他类型
            if (cardRequestContext.CardScope == CardScopeEnum.Any)
            {
                response = await GetResponseCardByCardType_Any(cardRequestContext);
            }
            //非任意牌
            //1. 只允许手牌
            //2. 只允许武器牌
            //3. 只允许防具
            //4. 只允许装备牌
            else if (cardRequestContext.CardScope == Enums.CardScopeEnum.InHand)
            {
                response = await GetResponseCardByCardType_InHands(cardRequestContext);
            }
            else if (cardRequestContext.CardScope == Enums.CardScopeEnum.InHandAndEquipment)
            {
                var exculdeEquipCards = new List<string>();
                if (cardRequestContext.AttackType == AttackTypeEnum.Bolangchui)
                {
                    exculdeEquipCards.Add(nameof(Bolangchui));
                }
                else if (cardRequestContext.AttackType == AttackTypeEnum.Luyeqiang)
                {
                    exculdeEquipCards.Add(nameof(Luyeqiang));
                }
                else if (cardRequestContext.AttackType == AttackTypeEnum.Panlonggun)
                {
                    exculdeEquipCards.Add(nameof(Panlonggun));
                }
                response = await GetResponseCardByCardType_InHandsAmdEquipment(cardRequestContext, exculdeEquipCards);
            }
            else
            {
                throw new NotImplementedException($"{cardRequestContext.CardScope}类型的请求未实现。");
            }
            //触发被动出牌事件
            return response;
        }

        public override async Task<CardResponseContext> OnRequestPickCardFromPanel(PickCardFromPanelRequest request)
        {
            //基本逻辑是
            //1. 如果选取的牌是来自友方，则优先抽取负面标记牌
            var response = new CardResponseContext()
            {
                Cards = new List<CardBase>() { }
            };
            if (request.Panel.CardOwner != null && request.Panel.CardOwner.IsSameGroup(PlayerContext.Player) && request.Panel.MarkCards != null)
            {
                var markCards = request.Panel.MarkCards.Where(p => p.Mark != null);
                //如果有画地为牢，则将其取掉
                var huadiweilaoMark = markCards.FirstOrDefault(p => nameof(p.Mark) == nameof(HuadiweilaoMark));
                if (huadiweilaoMark != null)
                {
                    //Console.WriteLine($"{PlayerContext.Player.PlayerId}的【{PlayerContext.Player.GetCurrentPlayerHero().Hero.DisplayName}】从{request.Panel.CardOwner.PlayerId}的【{request.Panel.CardOwner.GetCurrentPlayerHero().Hero.DisplayName}】抽取了{huadiweilaoMark.Card.DisplayName}");
                    //request.Panel.CardOwner.Marks.Remove(huadiweilaoMark.Mark);
                    huadiweilaoMark.SelectedBy = PlayerContext.Player;
                    response.Cards.Add(huadiweilaoMark.Card);
                }
            }

            if (response.Cards.Count >= request.MaxCount)
            {
                return response;
            }

            //2. 如果有装备牌，则选择装备牌。优先级从高到低是武器、防具、防御马、进攻马
            if (request.Panel.EquipmentCards?.Any() == true)
            {
                //判断是否有武器
                await RemoveEquipment<IWeapon>(response, request.Panel);
                if (response.Cards.Count >= request.MaxCount)
                {
                    return response;
                }

                //判断是否有防具
                await RemoveEquipment<IDefender>(response, request.Panel);
                if (response.Cards.Count >= request.MaxCount)
                {
                    return response;
                }

                //判断是否有防御马
                await RemoveEquipment<Fangyuma>(response, request.Panel);
                if (response.Cards.Count >= request.MaxCount)
                {
                    return response;
                }

                //判断是否有进攻马
                await RemoveEquipment<Jingongma>(response, request.Panel);
                if (response.Cards.Count >= request.MaxCount)
                {
                    return response;
                }
            }

            //3. 如果有手牌，则选择手牌
            if (request.Panel.InHandCards?.Any() == true)
            {
                var maxCount = request.MaxCount - response.Cards.Count;
                var takeCards = request.Panel.InHandCards.Take(maxCount);
                response.Cards.AddRange(takeCards.Select(t =>
                {
                    t.SelectedBy = PlayerContext.Player;
                    return t.Card;
                }));
            }

            //4. 如果有五谷丰登，则按照Value、Priority降序选择牌
            if (request.Panel.UnknownCards?.Any() == true)
            {
                var maxCount = request.MaxCount - response.Cards.Count;
                var takeCards = request.Panel.UnknownCards.Where(c => c.SelectedBy == null).
                    Select(c => new { Key = c, Value = GetCardAiValue(c.Card) }).OrderByDescending(c => c.Value.Value).ThenBy(c => c.Value.Priority).Take(maxCount).Select(c => c.Key);
                response.Cards.AddRange(takeCards.Select(t =>
                {
                    t.SelectedBy = PlayerContext.Player;
                    return t.Card;
                }));
            }
            return response;
        }

        /// <summary>
        /// 请求开始我的回合，主要用于询问是否需要发动技能（如：醉酒）
        /// </summary>
        /// <returns></returns>
        public override async Task OnRequestStartStep_EnterMyRound()
        {
            await Task.FromResult(0);
        }

        /// <summary>
        /// 请求处理摸牌阶段逻辑。
        /// </summary>
        /// <returns></returns>
        public override async Task OnRequestStartStep_PickCard()
        {
            await Task.FromResult(0);
        }

        /// <summary>
        /// 请求处理出牌阶段逻辑。
        /// </summary>
        /// <returns></returns>
        public override async Task OnRequestStartStep_PlayCard()
        {
            #region 按照固定逻辑处理
            //具体的逻辑依下面顺序来执行，如果有变化则再从头执行一次。
            //注（变化）：
            //      1. 杀成功
            //      2. 己方掉血
            //      3. 有新手牌进来（杀贪、五谷丰登、探囊取物、妙计导致的摸牌等。。）
            //      4.出过牌
            //TODO: 增加检查能够提供相应牌的逻辑的判断。如，释权可以黑色当釜底抽薪
            bool shouldContinueLoop = false;
            do
            {
                shouldContinueLoop = false;

                #region 是否有釜底抽薪？或者能够提供釜底抽薪

                if (!shouldContinueLoop)
                {
                    shouldContinueLoop = await PlayCard<Fudichouxin>(
                        PlayerContext.Player.GetAllSkillButtons().Where(s => s.IsEnabled() && s is IAbility ability && ability.CanProvideFuidichouxin()));
                }
                #endregion

                #region 是否有借刀杀人
                if (!shouldContinueLoop)
                {
                    shouldContinueLoop = await PlayCard<Jiedaosharen>(
                        PlayerContext.Player.GetAllSkillButtons().Where(s => s.IsEnabled() && s is IAbility ability && ability.CanProvideJiedaosharen()));
                }

                #endregion

                #region 是否有探囊取物
                if (!shouldContinueLoop)
                {
                    shouldContinueLoop = await PlayCard<Tannangquwu>(
                        PlayerContext.Player.GetAllSkillButtons().Where(s => s.IsEnabled() && s is IAbility ability && ability.CanProvideTannangquwu()));
                }
                #endregion

                //是否有无中生有？
                if (!shouldContinueLoop)
                {
                    shouldContinueLoop = await PlayCard<Wuzhongshengyou>(
                        PlayerContext.Player.GetAllSkillButtons().Where(s => s.IsEnabled() && s is IAbility ability && ability.CanProvideWuzhongshengyou()));
                }

                //是否有药？
                if (!shouldContinueLoop)
                {
                    shouldContinueLoop = await PlayCard<Yao>(
                        PlayerContext.Player.GetAllSkillButtons().Where(s => s.IsEnabled() && s is IAbility ability && ability.CanProvideYao()));
                }

                //是否要画地为牢？
                if (!shouldContinueLoop)
                {
                    shouldContinueLoop = await PlayCard<Huadiweilao>(
                        PlayerContext.Player.GetAllSkillButtons().Where(s => s.IsEnabled() && s is IAbility ability && ability.CanProvideHudadiweilao()));
                }

                //是否万箭齐发？
                if (!shouldContinueLoop)
                {
                    shouldContinueLoop = await PlayCard<Wanjianqifa>(
                        PlayerContext.Player.GetAllSkillButtons().Where(s => s.IsEnabled() && s is IAbility ability && ability.CanProvideWanjianqifa()));
                }

                //是否烽火狼烟？
                if (!shouldContinueLoop)
                {
                    shouldContinueLoop = await PlayCard<Fenghuolangyan>(
                        PlayerContext.Player.GetAllSkillButtons().Where(s => s.IsEnabled() && s is IAbility ability && ability.CanProvideFenghuolangyan()));
                }

                //是否决斗？
                if (!shouldContinueLoop)
                {
                    shouldContinueLoop = await PlayCard<Juedou>(
                        PlayerContext.Player.GetAllSkillButtons().Where(s => s.IsEnabled() && s is IAbility ability && ability.CanProvideJuedou()));
                }

                //是否休养生息？
                if (!shouldContinueLoop)
                {
                    //如果血量不满的队友的数量>血量不满的敌人，则打出
                    var alivePlayers = PlayerContext.GameLevel.GetAlivePlayers();
                    if (alivePlayers.Count(a => a.IsSameGroup(PlayerContext.Player) && a.GetCurrentPlayerHero().CurrentLife < a.GetCurrentPlayerHero().GetAttackFactor().MaxLife) >
                        alivePlayers.Count(a => !a.IsSameGroup(PlayerContext.Player) && a.GetCurrentPlayerHero().CurrentLife < a.GetCurrentPlayerHero().GetAttackFactor().MaxLife))
                    {
                        shouldContinueLoop = await PlayCard<Xiuyangshengxi>(
                            PlayerContext.Player.GetAllSkillButtons().Where(s => s.IsEnabled() && s is IAbility ability && ability.CanProvideXiuyangshengxi()));
                    }
                }

                //是否装备武器（虎符。。。）？
                if (!shouldContinueLoop)
                {
                    shouldContinueLoop = await PlayWeaponCards();
                }

                //是否有能杀？（有杀或者技能能提供杀）如果能杀，是否需要发动增强杀的技能（如三板斧，侠胆、傲剑、武穆）
                //如果有主动技能能够增强杀（如三板斧,侠胆）
                if (!shouldContinueLoop)
                {
                    shouldContinueLoop = await TriggerSkill_EnhanceSha();
                }

                if (!shouldContinueLoop)
                {
                    shouldContinueLoop = await PlayCard<Sha>(
                        PlayerContext.Player.GetAllSkillButtons().Where(s => s.IsEnabled() && s is IAbility ability && ability.CanProvideSha()));
                }

                //是否要触发技能（毒计、傲剑、侠胆）？
                if (!shouldContinueLoop)
                {
                    shouldContinueLoop = await TriggerSkills();
                }

                //是否手捧雷？
                if (!shouldContinueLoop)
                {
                    shouldContinueLoop = await PlayCard<Shoupenglei>(null);
                }

            } while (shouldContinueLoop);

            #endregion

            #region 动态处理优先级

            ////出牌逻辑
            ////0. 如果有可以触发的技能，则询问是福触发技能
            ////  如果可以触发,则触发。要解决的问题是，TODO:如果某个技能按钮在触发多次（成功或者失败多次）之后依然可以触发该如何解决？
            ////      正常的如：毒计-手中有多张红牌，该技能能触发多次；装有虎符情况下傲剑、武穆都可以多次触发
            ////      非正常的如：三板斧显示可以触发但攻击范围内没有敌人。同理如此情况下的傲剑、武穆。

            //var shouldStopSkill = false;
            //while (!shouldStopSkill)
            //{
            //    var avSkillBtns = PlayerContext.Player.GetAllSkillButtons().Where(s => s.IsEnabled());
            //    bool isSuccessTrigger = false;
            //    foreach (var skillButton in avSkillBtns)
            //    {
            //        var shouldTrigger = await PlayerContext.Player.ActionManager.OnRequestTriggerSkill(skillButton.GetButtonInfo().SkillType, null);
            //        if (shouldTrigger)
            //        {
            //            var response = new CardResponseContext();
            //            await skillButton.GetButtonInfo().OnClick(new CardRequestContext(), PlayerContext.Player.RoundContext, response);
            //            isSuccessTrigger = response.ResponseResult == ResponseResultEnum.Failed ||
            //                               response.ResponseResult == ResponseResultEnum.Success ||
            //                               response.ResponseResult == ResponseResultEnum.Cancelled;
            //        }
            //    }

            //    shouldStopSkill = !isSuccessTrigger;
            //}


            ////将手中的牌按照优先级降序排列，循环检查每张牌是否可以被主动打出，
            ////1. 如果可以被主动打出，则请求选择目标（如果目标可选，则出牌，如果没有可选目标，则跳过）
            ////2. 如果本次循环有出过牌，则继续下次循环，否则结束出牌。
            //var shouldStop = false;
            //while (!shouldStop)
            //{
            //    var orderedCards = PlayerContext.Player.CardsInHand.OrderByDescending(c => GetCardAiValue(c).Priority).ToList();
            //    bool playedCard = false;
            //    orderedCards.ForEach(async o =>
            //    {
            //        if (o.CanBePlayed())
            //        {
            //            var targets = await o.SelectTargets();
            //            if (targets == null || !targets.Any())
            //            {
            //                return;
            //            }
            //            await o.PlayCard(new CardRequestContext()
            //            {
            //                TargetPlayers = targets
            //            }, PlayerContext.Player.RoundContext);
            //            playedCard = true;
            //        }
            //    });
            //    shouldStop = !playedCard;
            //}

            #endregion
            await Task.FromResult(0);
        }

        /// <summary>
        /// 请求处理弃牌阶段逻辑
        /// </summary>
        /// <returns></returns>
        public override async Task OnRequestStartStep_ThrowCard()
        {
            //弃牌阶段，按照价值、优先级升序取弃牌数张牌弃掉
            var cards = PlayerContext.Player.CardsInHand.OrderBy(c => GetCardAiValue(c).Value);
            var attackFactor = PlayerContext.Player.MergeAttackDynamicFactor(PlayerContext.Player.GetCurrentPlayerHero().BaseAttackFactor,
                  PlayerContext.Player.RoundContext.AttackDynamicFactor);
            var throwCount = cards.Count() - Math.Max(attackFactor.MaxCardCountInHand, PlayerContext.Player.GetCurrentPlayerHero().CurrentLife);
            if (throwCount > 0)
            {
                Console.WriteLine($"{PlayerContext.Player.PlayerId}【{PlayerContext.Player.GetCurrentPlayerHero().Hero.DisplayName}】需要弃{throwCount}张牌。");
                var cardsToThrow = cards.Take(throwCount).ToList();

                //将该牌置入TempCardDesk
                await PlayerContext.Player.RemoveCardsInHand(cardsToThrow, null, null, null);
                Console.WriteLine($"{PlayerContext.Player.PlayerId}【{PlayerContext.Player.GetCurrentPlayerHero().Hero.DisplayName}】弃掉了{string.Join(",", cardsToThrow)}。");
            }
            await Task.FromResult(0);
        }

        /// <summary>
        /// 请求处理结束出牌逻辑。
        /// </summary>
        /// <returns></returns>
        public override async Task OnRequestStartStep_ExitMyRound()
        {

            await Task.FromResult(0);
        }

        /// <summary>
        /// 请求选择目标
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public override async Task<SelectedTargetsResponse> OnRequestSelectTargets(SelectedTargetsRequest request)
        {
            if (request == null)
            {
                throw new Exception("选择目标的请求不能为空。");
            }

            SelectedTargetsResponse response = new SelectedTargetsResponse();
            var alivePlayers = PlayerContext.GameLevel.GetAlivePlayers();

            switch (request.TargetType)
            {
                case AttackTypeEnum.None:
                    break;
                case AttackTypeEnum.Sha:
                    {
                        //选择杀的目标
                        return await GetTargets_Sha(request, alivePlayers);
                    };
                case AttackTypeEnum.Juedou:
                    {
                        //选择决斗的目标
                        return await GetTargets_Juedou(request, alivePlayers);
                    }
                //case AttackTypeEnum.Fenghuolangyan:
                //    {
                //        //选择决斗的目标
                //        return await GetTargets_Juedou(request, alivePlayers);
                //    }
                //case AttackTypeEnum.Wanjianqifa:
                //    break;
                //case AttackTypeEnum.GroupRequestWithConfirm:
                //    break;
                //case AttackTypeEnum.Wuzhongshengyou:
                //    break;
                //case AttackTypeEnum.Wuxiekeji:
                //    break;
                //case AttackTypeEnum.Wugufengdeng:
                //    break;
                //case AttackTypeEnum.Xiuyangshengxi:
                //    break;
                //case AttackTypeEnum.Longlindao:
                //    break;
                //case AttackTypeEnum.Luyeqiang:
                //    break;
                //case AttackTypeEnum.Bolangchui:
                //    break;
                //case AttackTypeEnum.Panlonggun:
                //    break;
                //case AttackTypeEnum.SelectCard:
                //    break;
                case AttackTypeEnum.Xiadan:
                    {
                        //选择侠胆的目标
                        return await GetTargets_XiaDan(request, alivePlayers);
                    }
                case AttackTypeEnum.Hongyan:
                    {
                        //选择红颜的目标
                        return await GetTargets_Hongyan(request, alivePlayers);
                    }
                case AttackTypeEnum.Gongxin:
                    {
                        //选择红颜的目标
                        return await GetTargets_Gongxin(request, alivePlayers);
                    }
                case AttackTypeEnum.Jiedaosharen:
                    {
                        //选择借刀杀人的目标
                        return await GetTargets_Jiedaosharen(request, alivePlayers);
                    }
                case AttackTypeEnum.Fudichouxin:
                    {
                        //选择釜底抽薪的目标
                        return await GetTargets_Fudichouxin(request, alivePlayers);
                    }
                case AttackTypeEnum.Huadiweilao:
                    {
                        //选择画地为牢的目标
                        return await GetTargets_Huadiweilao(request, alivePlayers);
                    }
                case AttackTypeEnum.Tannangquwu:
                    {
                        //选择探囊取物的目标
                        return await GetTargets_Tannangquwu(request, alivePlayers);
                    }
                case AttackTypeEnum.Liaoshang:
                    {
                        //选择疗伤的目标
                        return await GetTargets_Liaoshang(request, alivePlayers);
                    }
                case AttackTypeEnum.Zhiyu:
                    {
                        //选择疗伤的目标
                        return await GetTargets_Zhiyu(request, alivePlayers);
                    }
                default:
                    return new SelectedTargetsResponse()
                    {
                        Status = ResponseResultEnum.Success
                    };
            }

            return await Task.FromResult(response);
        }

        /// <summary>
        /// 获取Card的API值
        /// </summary>
        /// <param name="card"></param>
        /// <returns></returns>
        public AiValue GetCardAiValue(CardBase card)
        {
            var valueDic = new Dictionary<Type, AiValue>()
            {
                { typeof(Sha),new AiValue()
                {
                    Priority = 120,
                    Value = 200
                }},
                { typeof(Shan),new AiValue()
                {
                    Priority = 120,
                    Value = 90
                }},
                { typeof(Shoupenglei),new AiValue()
                {
                    Priority = 100,
                    Value = 80
                }},
                { typeof(Juedou),new AiValue(){
                    Priority = 150,
                    Value = 210
                }},
                { typeof(Yao),new AiValue(){
                    Priority = 175,
                    Value = 250
                }},
                { typeof(Fenghuolangyan),new AiValue(){
                    Priority = 170,
                    Value = 200
                }},
                { typeof(Wanjianqifa),new AiValue(){
                    Priority = 170,
                    Value = 200
                }},
                { typeof(Xiuyangshengxi),new AiValue(){
                    Priority = 160,
                    Value = 100
                }},
                { typeof(Jiedaosharen),new AiValue(){
                    Priority = 190,
                    Value = 80
                }},
                { typeof(Fudichouxin),new AiValue(){
                    Priority = 200,
                    Value = 200
                }},
                { typeof(Wugufengdeng),new AiValue(){
                    Priority = 100,
                    Value = 80
                }},
                { typeof(Wuxiekeji),new AiValue(){
                    Priority = 210,
                    Value = 200
                }},
                { typeof(Wuzhongshengyou),new AiValue(){
                    Priority = 220,
                    Value = 220
                }},
                { typeof(Tannangquwu),new AiValue(){
                    Priority = 180,
                    Value = 220
                }},
                { typeof(Huadiweilao),new AiValue(){
                    Priority = 100,
                    Value = 220
                }},
                { typeof(Luyeqiang),new AiValue(){
                    Priority = 160,
                    Value = 150
                }},
                { typeof(Hufu),new AiValue(){
                    Priority = 100,
                    Value = 230
                }},
                { typeof(Yuchangjian),new AiValue(){
                    Priority = 150,
                    Value = 80
                }},
                { typeof(Longlindao),new AiValue(){
                    Priority = 160,
                    Value = 80
                }},
                { typeof(Langyabang),new AiValue(){
                    Priority = 150,
                    Value = 80
                }},
                { typeof(Bawanggong),new AiValue(){
                    Priority = 160,
                    Value = 80
                }},
                { typeof(Panlonggun),new AiValue(){
                    Priority = 150,
                    Value = 80
                }},
                { typeof(Yuruyi),new AiValue(){
                    Priority = 160,
                    Value = 220
                }},
            };

            return valueDic.ContainsKey(card.GetType()) ? valueDic[(card).GetType()] : new AiValue()
            {
                Priority = 100,
                Value = 100
            };
        }

        #region 私有方法

        #region OnRequestSelectTargets 选择目标的逻辑
        /// <summary>
        /// 获取治愈的目标
        /// </summary>
        /// <param name="request"></param>
        /// <param name="players"></param>
        /// <returns></returns>
        private async Task<SelectedTargetsResponse> GetTargets_Zhiyu(SelectedTargetsRequest request, IEnumerable<Player> players)
        {
            SelectedTargetsResponse response = new SelectedTargetsResponse();
            //选择的目标：血量最低、手牌最少的友方
            var targets = players.Where(p => p.IsSameGroup(PlayerContext.Player) && p.GetCurrentPlayerHero().CurrentLife < p.GetCurrentPlayerHero().GetAttackFactor().MaxLife).OrderBy(p => p.GetCurrentPlayerHero().CurrentLife).ThenBy(p => p.CardsInHand.Count);
            foreach (var target in targets)
            {
                if (response.Targets.Count >= request.MaxTargetCount)
                {
                    break;
                }
                if (await PlayerContext.Player.IsAvailableForPlayer(target, request.SrcCards?.FirstOrDefault(), AttackTypeEnum.Zhiyu))
                {
                    response.Targets.Add(target);
                }
            }

            return response;
        }

        /// <summary>
        /// 获取疗伤的目标
        /// </summary>
        /// <param name="request"></param>
        /// <param name="players"></param>
        /// <returns></returns>
        private async Task<SelectedTargetsResponse> GetTargets_Liaoshang(SelectedTargetsRequest request, IEnumerable<Player> players)
        {
            SelectedTargetsResponse response = new SelectedTargetsResponse();
            //选择的目标：血量最低、手牌最少的友方
            var targets = players.Where(p => p.IsSameGroup(PlayerContext.Player) && p.GetCurrentPlayerHero().CurrentLife < p.GetCurrentPlayerHero().GetAttackFactor().MaxLife).OrderBy(p => p.GetCurrentPlayerHero().CurrentLife).ThenBy(p => p.CardsInHand.Count);
            foreach (var target in targets)
            {
                if (response.Targets.Count >= request.MaxTargetCount)
                {
                    break;
                }
                if (await PlayerContext.Player.IsAvailableForPlayer(target, request.SrcCards?.FirstOrDefault(), AttackTypeEnum.Liaoshang))
                {
                    response.Targets.Add(target);
                }
            }

            return response;
        }

        /// <summary>
        /// 获取画地为牢的目标
        /// </summary>
        /// <param name="request"></param>
        /// <param name="players"></param>
        /// <returns></returns>
        private async Task<SelectedTargetsResponse> GetTargets_Huadiweilao(SelectedTargetsRequest request, IEnumerable<Player> players)
        {
            SelectedTargetsResponse response = new SelectedTargetsResponse();
            //选择的目标：选择手牌/血量比最高的一个敌方。5牌/4血-3牌/2血
            var targets = players.Where(p => !p.IsSameGroup(PlayerContext.Player)).OrderBy(p => p.CardsInHand.Count / p.GetCurrentPlayerHero().CurrentLife).ThenBy(p => p.CardsInHand.Count);
            foreach (var target in targets)
            {
                if (response.Targets.Count >= request.MaxTargetCount)
                {
                    break;
                }
                if (await PlayerContext.Player.IsAvailableForPlayer(target, request.SrcCards?.FirstOrDefault(), AttackTypeEnum.Huadiweilao))
                {
                    response.Targets.Add(target);
                }
            }

            return response;
        }

        /// <summary>
        /// 获取杀的目标
        /// </summary>
        /// <param name="request"></param>
        /// <param name="players"></param>
        /// <returns></returns>
        private async Task<SelectedTargetsResponse> GetTargets_Sha(SelectedTargetsRequest request, IEnumerable<Player> players)
        {
            SelectedTargetsResponse response = new SelectedTargetsResponse();
            //选择杀的目标：血量最低的，手牌最少的
            var targets = players.Where(p => !p.IsSameGroup(PlayerContext.Player)).OrderBy(p => p.GetCurrentPlayerHero().CurrentLife).ThenBy(p => p.CardsInHand.Count);
            foreach (var target in targets)
            {
                if (response.Targets.Count >= request.MaxTargetCount)
                {
                    break;
                }
                //获取两个player之间的距离，检查是否在攻击范围内
                var canBeSelected = await PlayerContext.Player.IsAvailableForPlayer(target, request.SrcCards?.FirstOrDefault(), AttackTypeEnum.Sha);
                if (canBeSelected)
                {
                    response.Targets.Add(target);
                }
            }

            return response;
        }

        /// <summary>
        /// 获取决斗的目标
        /// </summary>
        /// <param name="request"></param>
        /// <param name="players"></param>
        /// <returns></returns>
        private async Task<SelectedTargetsResponse> GetTargets_Juedou(SelectedTargetsRequest request, IEnumerable<Player> players)
        {
            SelectedTargetsResponse response = new SelectedTargetsResponse();
            //选择杀的目标：血量最低的，手牌最少的
            var targets = players.Where(p => !p.IsSameGroup(PlayerContext.Player)).OrderBy(p => p.GetCurrentPlayerHero().CurrentLife).ThenBy(p => p.CardsInHand.Count);
            foreach (var target in targets)
            {
                if (response.Targets.Count >= request.MaxTargetCount)
                {
                    break;
                }

                if (await PlayerContext.Player.IsAvailableForPlayer(target, request.SrcCards?.FirstOrDefault(), AttackTypeEnum.Juedou))
                {
                    response.Targets.Add(target);
                }
            }

            return response;
        }

        /// <summary>
        /// 获取侠胆的目标
        /// </summary>
        /// <param name="request"></param>
        /// <param name="players"></param>
        /// <returns></returns>
        private async Task<SelectedTargetsResponse> GetTargets_XiaDan(SelectedTargetsRequest request, IEnumerable<Player> players)
        {
            SelectedTargetsResponse response = new SelectedTargetsResponse();
            //选择的目标：血量最低的，手牌最少的
            var targets = players.OrderBy(p => p.IsSameGroup(PlayerContext.Player)).ThenBy(p => p.CardsInHand.Count);
            foreach (var target in targets)
            {
                if (response.Targets.Count >= request.MaxTargetCount)
                {
                    break;
                }
                if (await PlayerContext.Player.IsAvailableForPlayer(target, request.SrcCards?.FirstOrDefault(), AttackTypeEnum.Xiadan))
                {
                    response.Targets.Add(target);
                }
            }

            return response;
        }

        /// <summary>
        /// 获取红颜的目标
        /// </summary>
        /// <param name="request"></param>
        /// <param name="players"></param>
        /// <returns></returns>
        private async Task<SelectedTargetsResponse> GetTargets_Hongyan(SelectedTargetsRequest request, IEnumerable<Player> players)
        {
            SelectedTargetsResponse response = new SelectedTargetsResponse();
            //选择的目标：
            //  1. 敌方存活人数>=2时：选择血量最低的和最高的敌方
            //  2. 敌方存活人数=1时：如果该敌方手中的牌数<=2且己方队友手中杀的数量>=1则选择
            List<Player> enemies = new List<Player>();
            foreach (var player in players)
            {
                if (!player.IsSameGroup(PlayerContext.Player) &&
                    (await PlayerContext.Player.IsAvailableForPlayer(player, null, AttackTypeEnum.Hongyan)))
                {
                    enemies.Add(player);
                }
            }
            enemies = enemies.OrderBy(p => p.GetCurrentPlayerHero().CurrentLife).ToList();
            if (enemies.Count() >= 2)
            {
                response.Targets.Add(enemies.First());
                response.Targets.Add(enemies.Last());
                return response;
            }
            else if (enemies.Count() == 1)
            {
                var enemy = enemies.First();
                if (enemy.CardsInHand.Count <= 2)
                {
                    List<Player> friends = new List<Player>();
                    foreach (var player in players)
                    {
                        if (player.IsSameGroup(PlayerContext.Player) && player != PlayerContext.Player && player.CardsInHand.Count(c => c is Sha) >= 1 &&
                            (await PlayerContext.Player.IsAvailableForPlayer(player, null, AttackTypeEnum.Hongyan)))
                        {
                            friends.Add(player);
                        }
                    }
                    var friend = friends.OrderByDescending(p => p.CardsInHand.Count(c => c is Sha)).FirstOrDefault();
                    if (friend != null)
                    {
                        response.Targets.Add(enemies.First());
                        response.Targets.Add(friend);
                    }
                }
            }

            return response;
        }


        /// <summary>
        /// 获取攻心的目标
        /// </summary>
        /// <param name="request"></param>
        /// <param name="players"></param>
        /// <returns></returns>
        private async Task<SelectedTargetsResponse> GetTargets_Gongxin(SelectedTargetsRequest request, IEnumerable<Player> players)
        {
            SelectedTargetsResponse response = new SelectedTargetsResponse();
            //选择杀的目标：血量最低的，手牌最少的
            var targets = players.Where(p => !p.IsSameGroup(PlayerContext.Player)).OrderBy(p => p.GetCurrentPlayerHero().CurrentLife).ThenBy(p => p.CardsInHand.Count);
            foreach (var target in targets)
            {
                if (response.Targets.Count >= request.MaxTargetCount)
                {
                    break;
                }

                if (await PlayerContext.Player.IsAvailableForPlayer(target, request.SrcCards?.FirstOrDefault(), AttackTypeEnum.Gongxin))
                {
                    response.Targets.Add(target);
                }
            }

            return response;
        }

        /// <summary>
        /// 获取借刀杀人的目标
        /// </summary>
        /// <param name="request"></param>
        /// <param name="players"></param>
        /// <returns></returns>
        private async Task<SelectedTargetsResponse> GetTargets_Jiedaosharen(SelectedTargetsRequest request, IEnumerable<Player> players)
        {
            SelectedTargetsResponse response = new SelectedTargetsResponse();
            //选择借刀人对象，获取所有的有武器的玩家：
            var playersWithWeapons = players.Where(p => p.EquipmentSet?.Any(e => e is IWeapon) == true).ToList();
            List<Player> enemies = new List<Player>();
            playersWithWeapons.ForEach(async p =>
            {
                if (!p.IsSameGroup(PlayerContext.Player) && (await PlayerContext.Player.IsAvailableForPlayer(p, null, AttackTypeEnum.Jiedaosharen)))
                {
                    enemies.Add(p);
                }
            });
            enemies = enemies.OrderBy(p => p.CardsInHand.Count).ToList();

            var firstEnemy = enemies.FirstOrDefault();
            if (firstEnemy != null)
            {
                //1. 敌方有武器。选择敌方手牌最少的玩家作为借刀人；再检查有没有另外的敌方，如果有，则选择另外的敌方作为被杀人。如果没有，则只有敌方没有手牌的情况下选择借刀杀一个血量最高的友方
                if (enemies.Count() > 1)
                {
                    response.Targets.Add(enemies.First());
                    response.Targets.Add(enemies.Last());
                    return response;
                }

                if (firstEnemy.CardsInHand == null || firstEnemy.CardsInHand.Count <= 0)
                {
                    response.Targets.Add(enemies.First());
                    response.Targets.Add(playersWithWeapons.Where(p => p.IsSameGroup(PlayerContext.Player)).OrderByDescending(p => p.GetCurrentPlayerHero().CurrentLife).First());
                    return response;
                }
            }
            else if (playersWithWeapons.Any(p => p.IsSameGroup(PlayerContext.Player)))
            {
                //2. 敌方没武器，我方有武器。如果敌方有血量=1且该友方手牌中杀的数量>=1，则选择一个杀数量最多的一个友方作为借刀人
                List<Player> friends = new List<Player>();
                playersWithWeapons.ForEach(async p =>
                {
                    if (p.IsSameGroup(PlayerContext.Player) && p.CardsInHand.Count(c => c is Sha) >= 1 && (await PlayerContext.Player.IsAvailableForPlayer(p, null, AttackTypeEnum.Jiedaosharen)))
                    {
                        friends.Add(p);
                    }
                });
                friends = friends.OrderBy(p => p.CardsInHand.Count).ToList();
                var friend = friends.OrderByDescending(p => p.CardsInHand.Count(c => c is Sha)).FirstOrDefault();
                var enemy = enemies.FirstOrDefault(p => p.GetCurrentPlayerHero().CurrentLife <= 1);
                if (friend != null && enemy != null)
                {
                    response.Targets.Add(enemy);
                    response.Targets.Add(friend);
                    return response;
                }
            }
            //3. 敌方和我方都没有武器。不借刀
            return response;
        }

        /// <summary>
        /// 获取釜底抽薪的目标
        /// </summary>
        /// <param name="request"></param>
        /// <param name="players"></param>
        /// <returns></returns>
        private async Task<SelectedTargetsResponse> GetTargets_Fudichouxin(SelectedTargetsRequest request, IEnumerable<Player> players)
        {
            SelectedTargetsResponse response = new SelectedTargetsResponse();
            //选择的目标：
            //  1. 友方有负面标记牌且在范围内，则选择抽取友方的负面标记牌；
            var friendPlayersWithNegativeMarks = players.Where(p => p.IsSameGroup(PlayerContext.Player) && p != PlayerContext.Player && p.Marks.Any(m => m.MarkType == MarkTypeEnum.Card && m.IsNegativeMark() == true)).ToList();
            //釜底抽薪是没有距离限制的。
            friendPlayersWithNegativeMarks.ForEach(async friendPlayersWithNegativeMark =>
            {
                if (response.Targets.Count >= request.MaxTargetCount)
                {
                    return;
                }

                //todo:潜在的衔接问题，真正执行釜底抽薪的时候，要再判断一下到底是抽牌还是抽标记
                if (await PlayerContext.Player.IsAvailableForPlayer(friendPlayersWithNegativeMark, request.SrcCards?.FirstOrDefault(), AttackTypeEnum.Fudichouxin))
                {
                    response.Targets.Add(friendPlayersWithNegativeMark);
                }
            });


            if (response.Targets.Count >= request.MaxTargetCount)
            {
                return response;
            }

            //  2. 友方没有负面标记牌或者不在范围内，则选择敌方血量最低的，手牌最少的（必须有手牌或者装备牌或者标记牌）
            var enemies = players.Where(p => !p.IsSameGroup(PlayerContext.Player) && (p.CardsInHand?.Any() == true || p.EquipmentSet?.Any() == true)).OrderBy(p => p.GetCurrentPlayerHero().CurrentLife);
            foreach (var target in enemies)
            {
                if (response.Targets.Count >= request.MaxTargetCount)
                {
                    break;
                }

                if (await PlayerContext.Player.IsAvailableForPlayer(target, request.SrcCards?.FirstOrDefault(), AttackTypeEnum.Fudichouxin))
                {
                    response.Targets.Add(target);
                }
            }

            return response;
        }

        /// <summary>
        /// 获取探囊取物的目标
        /// </summary>
        /// <param name="request"></param>
        /// <param name="players"></param>
        /// <returns></returns>
        private async Task<SelectedTargetsResponse> GetTargets_Tannangquwu(SelectedTargetsRequest request, IEnumerable<Player> players)
        {
            SelectedTargetsResponse response = new SelectedTargetsResponse();
            //选择的目标：
            //  1. 友方有负面标记牌且在范围内，则选择抽取友方的负面标记牌；
            var friendPlayersWithNegativeMarks = players.Where(p => p.IsSameGroup(PlayerContext.Player) && p != PlayerContext.Player && p.Marks.Any(m => m.MarkType == MarkTypeEnum.Card && m.IsNegativeMark() == true)).ToList();
            //探囊取物有距离限制的。
            friendPlayersWithNegativeMarks.ForEach(async friendPlayersWithNegativeMark =>
            {
                if (response.Targets.Count >= request.MaxTargetCount)
                {
                    return;
                }

                //todo:潜在的衔接问题，真正执行釜底抽薪的时候，要再判断一下到底是抽牌还是抽标记
                if (await PlayerContext.Player.IsAvailableForPlayer(friendPlayersWithNegativeMark, request.SrcCards?.FirstOrDefault(), AttackTypeEnum.Tannangquwu))
                {
                    response.Targets.Add(friendPlayersWithNegativeMark);
                }
            });


            if (response.Targets.Count >= request.MaxTargetCount)
            {
                return response;
            }

            //  2. 友方没有负面标记牌或者不在范围内，则选择敌方血量最低的，手牌最少的（必须有手牌或者装备牌或者标记牌）
            var enemies = players.Where(p => !p.IsSameGroup(PlayerContext.Player) && (p.CardsInHand?.Any() == true || p.EquipmentSet?.Any() == true)).OrderBy(p => p.GetCurrentPlayerHero().CurrentLife);
            foreach (var target in enemies)
            {
                if (response.Targets.Count >= request.MaxTargetCount)
                {
                    break;
                }

                if (await PlayerContext.Player.IsAvailableForPlayer(target, request.SrcCards?.FirstOrDefault(), AttackTypeEnum.Fudichouxin))
                {
                    response.Targets.Add(target);
                }
            }

            return response;
        }

        #endregion

        #region 主动出牌逻辑
        /// <summary>
        /// 打出牌
        /// </summary>
        /// <returns>返回是否要继续下个回合检查出牌</returns>
        private async Task<bool> PlayCard<T>(IEnumerable<ISkillButton> enabledSkillButtons) where T : CardBase
        {
            bool shouldContinue = false;
            var cards = PlayerContext.Player.CardsInHand.Where(p => p is T || (p is ChangedCard c && c.TargetCard is T)).ToList();
            foreach (var card in cards)
            {
                var success = await card.Popup();
                if (success)
                {
                    shouldContinue = true;
                }
            }

            if (enabledSkillButtons == null)
            {
                return shouldContinue;
            }

            foreach (var skillBtn in enabledSkillButtons)
            {
                var request = new CardRequestContext();
                var shuouldTrigger = await OnRequestTriggerSkill(skillBtn.GetButtonInfo().SkillType, request);
                if (shuouldTrigger)
                {
                    var response = new CardResponseContext();
                    await skillBtn.GetButtonInfo().OnClick(request, PlayerContext.Player.RoundContext, response);
                    if (response.ResponseResult == ResponseResultEnum.Success || response.Cards?.Any() == true)
                    {
                        //触发了技能，返回一张ChangedCard或者MutedCard.
                        var virtualCard = response.Cards.First();
                        if (!shouldContinue)
                        {
                            shouldContinue = await virtualCard.Popup();
                        }
                    }
                }
            }

            return shouldContinue;
        }

        ///// <summary>
        ///// 打出釜底抽薪
        ///// </summary>
        ///// <returns>返回是否要继续下个回合检查出牌</returns>
        //private async Task<bool> PlayFudichouxin()
        //{
        //    bool shouldContinue = false;
        //    var fudichouxins = PlayerContext.Player.CardsInHand.Where(p => p is Fudichouxin).ToList();
        //    foreach (var fudichouxin in fudichouxins)
        //    {
        //        var success = await fudichouxin.Popup();
        //        if (success)
        //        {
        //            shouldContinue = true;
        //        }
        //    }

        //    return shouldContinue;
        //}

        ///// <summary>
        ///// 打出有借刀杀人
        ///// </summary>
        ///// <returns>返回是否要继续下个回合检查出牌</returns>
        //private async Task<bool> PlayJiedaosharen()
        //{
        //    bool shouldContinue = false;
        //    var jiedaosharens = PlayerContext.Player.CardsInHand.Where(p => p is Jiedaosharen).ToList();
        //    foreach (var jiedaosharen in jiedaosharens)
        //    {
        //        var targets = await jiedaosharen.SelectTargets(new SelectTargetRequest()
        //        {
        //            MinCount = 1,
        //            MaxCount = 1
        //        });
        //        if (targets != null && targets.Count == 2)
        //        {
        //            await jiedaosharen.PlayCard(new CardRequestContext()
        //            {
        //                AttackType = AttackTypeEnum.Jiedaosharen,
        //                TargetPlayers = targets //借刀杀人的targets，第一个player代表被借刀的人，第二个代表是要杀的人
        //            }, PlayerContext.Player.RoundContext);
        //            //出过牌，继续检查是否有需要出的牌
        //            shouldContinue = true;
        //        }
        //    }
        //    return shouldContinue;
        //}

        ///// <summary>
        ///// 打出探囊取物
        ///// </summary>
        ///// <returns>返回是否要继续下个回合检查出牌</returns>
        //private async Task<bool> PlayTannangquwu()
        //{
        //    bool shouldContinue = false;
        //    var tannangquwus = PlayerContext.Player.CardsInHand.Where(p => p is Tannangquwu).ToList();
        //    foreach (var tannangquwu in tannangquwus)
        //    {
        //        var targets = await tannangquwu.SelectTargets(new SelectTargetRequest()
        //        {
        //            MinCount = 1,
        //            MaxCount = 1
        //        });
        //        if (targets != null && targets.Any())
        //        {
        //            await tannangquwu.PlayCard(new CardRequestContext()
        //            {
        //                AttackType = AttackTypeEnum.Tannangquwu,
        //                TargetPlayers = targets
        //            }, PlayerContext.Player.RoundContext);
        //            //出过牌，继续检查是否有需要出的牌
        //            shouldContinue = true;
        //        }
        //    }

        //    return shouldContinue;
        //}

        ///// <summary>
        ///// 打出无中生有
        ///// </summary>
        ///// <returns>返回是否要继续下个回合检查出牌</returns>
        //private async Task<bool> PlayWuzhongshengyou()
        //{
        //    bool shouldContinue = false;
        //    var wuzhongshengyous = PlayerContext.Player.CardsInHand.Where(p => p is Wuzhongshengyou).ToList();
        //    foreach (var wuzhongshengyou in wuzhongshengyous)
        //    {
        //        await wuzhongshengyou.PlayCard(new CardRequestContext()
        //        {
        //            AttackType = AttackTypeEnum.Wuzhongshengyou,
        //        }, PlayerContext.Player.RoundContext);
        //        //出过牌，继续检查是否有需要出的牌
        //        shouldContinue = true;
        //    }

        //    return shouldContinue;
        //}

        ///// <summary>
        ///// 打出药
        ///// </summary>
        ///// <returns>返回是否要继续下个回合检查出牌</returns>
        //private async Task<bool> PlayYao()
        //{
        //    bool shouldContinue = false;
        //    var cards = PlayerContext.Player.CardsInHand.Where(p => p is Yao).ToList();
        //    foreach (var card in cards)
        //    {
        //        await card.PlayCard(new CardRequestContext()
        //        {
        //            TargetPlayers = new List<Player>()
        //            {
        //                PlayerContext.Player
        //            }
        //        }, PlayerContext.Player.RoundContext);
        //        //出过牌，继续检查是否有需要出的牌
        //        shouldContinue = true;
        //    }

        //    return shouldContinue;
        //}

        ///// <summary>
        ///// 打出画地为牢
        ///// </summary>
        ///// <returns>返回是否要继续下个回合检查出牌</returns>
        //private async Task<bool> PlayHuadiweilao()
        //{
        //    bool shouldContinue = false;
        //    var cards = PlayerContext.Player.CardsInHand.Where(p => p is Yao).ToList();
        //    foreach (var card in cards)
        //    {
        //        var targets = await card.SelectTargets(new SelectTargetRequest()
        //        {
        //            MinCount = 1,
        //            MaxCount = 1
        //        });
        //        if (targets != null && targets.Any())
        //        {
        //            await card.PlayCard(new CardRequestContext()
        //            {
        //                TargetPlayers = targets
        //            }, PlayerContext.Player.RoundContext);
        //            //出过牌，继续检查是否有需要出的牌
        //            shouldContinue = true;
        //        }
        //    }

        //    return shouldContinue;
        //}

        ///// <summary>
        ///// 打出万箭齐发
        ///// </summary>
        ///// <returns>返回是否要继续下个回合检查出牌</returns>
        //private async Task<bool> PlayWanjianqifa()
        //{
        //    bool shouldContinue = false;
        //    var cards = PlayerContext.Player.CardsInHand.Where(p => p is Wanjianqifa).ToList();
        //    foreach (var card in cards)
        //    {
        //        await card.PlayCard(new CardRequestContext(), PlayerContext.Player.RoundContext);
        //        //出过牌，继续检查是否有需要出的牌
        //        shouldContinue = true;
        //    }

        //    return shouldContinue;
        //}

        ///// <summary>
        ///// 打出烽火狼烟
        ///// </summary>
        ///// <returns>返回是否要继续下个回合检查出牌</returns>
        //private async Task<bool> PlayFenghuolangyan()
        //{
        //    bool shouldContinue = false;
        //    var cards = PlayerContext.Player.CardsInHand.Where(p => p is Fenghuolangyan).ToList();
        //    foreach (var card in cards)
        //    {
        //        await card.PlayCard(new CardRequestContext(), PlayerContext.Player.RoundContext);
        //        //出过牌，继续检查是否有需要出的牌
        //        shouldContinue = true;
        //    }

        //    return shouldContinue;
        //}

        ///// <summary>
        ///// 打出休养生息
        ///// </summary>
        ///// <returns>返回是否要继续下个回合检查出牌</returns>
        //private async Task<bool> PlayXiuyangshengxi()
        //{
        //    bool shouldContinue = false;
        //    var cards = PlayerContext.Player.CardsInHand.Where(p => p is Xiuyangshengxi).ToList();
        //    foreach (var card in cards)
        //    {
        //        if (card.CanBePlayed())
        //        {
        //            await card.PlayCard(new CardRequestContext(), PlayerContext.Player.RoundContext);
        //            //出过牌，继续检查是否有需要出的牌
        //            shouldContinue = true;
        //        }
        //    }

        //    return shouldContinue;
        //}

        ///// <summary>
        ///// 打出休养生息
        ///// </summary>
        ///// <returns>返回是否要继续下个回合检查出牌</returns>
        //private async Task<bool> PlayShoupenglei()
        //{
        //    bool shouldContinue = false;
        //    var cards = PlayerContext.Player.CardsInHand.Where(p => p is Shoupenglei).ToList();
        //    foreach (var card in cards)
        //    {
        //        if (card.CanBePlayed())
        //        {
        //            await card.PlayCard(new CardRequestContext(), PlayerContext.Player.RoundContext);
        //            //出过牌，继续检查是否有需要出的牌
        //            shouldContinue = true;
        //        }
        //    }

        //    return shouldContinue;
        //}

        ///// <summary>
        ///// 打出决斗
        ///// </summary>
        ///// <returns>返回是否要继续下个回合检查出牌</returns>
        //private async Task<bool> PlayJuedou()
        //{
        //    bool shouldContinue = false;
        //    var cards = PlayerContext.Player.CardsInHand.Where(p => p is Juedou).ToList();
        //    foreach (var card in cards)
        //    {
        //        var targets = await card.SelectTargets(new SelectTargetRequest()
        //        {
        //            MinCount = 1,
        //            MaxCount = 1
        //        });
        //        if (targets != null && targets.Any())
        //        {
        //            await card.PlayCard(new CardRequestContext()
        //            {
        //                TargetPlayers = targets
        //            }, PlayerContext.Player.RoundContext);
        //            //出过牌，继续检查是否有需要出的牌
        //            shouldContinue = true;
        //        }
        //    }

        //    return shouldContinue;
        //}

        //private async Task<bool> PlaySha(IEnumerable<CardBase> cards)
        //{
        //    bool shouldContinue = false;
        //    if (!Sha.CanBePlayed(PlayerContext))
        //    {
        //        return shouldContinue;
        //    }

        //    //有杀的情况，直接出杀
        //    foreach (var card in cards)
        //    {
        //        if ((card is Sha sha && sha.CanBePlayed()) || (card is ChangedCard csha && csha.CanBePlayed()))
        //        {
        //            var targets = await card.SelectTargets(new SelectTargetRequest()
        //            {
        //                MinCount = 1,
        //                MaxCount = 1
        //            });
        //            if (targets != null && targets.Any())
        //            {
        //                await card.PlayCard(new CardRequestContext()
        //                {
        //                    TargetPlayers = targets
        //                }, PlayerContext.Player.RoundContext);
        //                //出过牌，继续检查是否有需要出的牌
        //                shouldContinue = true;
        //            }
        //        }
        //    }

        //    return shouldContinue;
        //}

        ///// <summary>
        ///// 打出杀
        ///// </summary>
        ///// <returns>返回是否要继续下个回合检查出牌</returns>
        //private async Task<bool> PlaySha()
        //{
        //    bool shouldContinue = false;
        //    if (!Sha.CanBePlayed(PlayerContext))
        //    {
        //        return shouldContinue;
        //    }


        //    //有杀的情况，直接出杀
        //    var cards = PlayerContext.Player.CardsInHand.Where(p => p is Sha).ToList();
        //    if (!shouldContinue)
        //    {
        //        shouldContinue = await PlaySha(cards);
        //    }

        //    //没杀的情况，看是否有技能能够提供杀
        //    if (!Sha.CanBePlayed(PlayerContext))
        //    {
        //        return shouldContinue;
        //    }
        //    //找到所有可以提供杀的且能够启用的技能
        //    var shaSkills = PlayerContext.Player.GetAllSkillButtons().Where(s => s.IsEnabled() && s is IAbility ability && ability.CanProvideSha());
        //    foreach (var shaSkill in shaSkills)
        //    {
        //        if (!Sha.CanBePlayed(PlayerContext))
        //        {
        //            break;
        //        }

        //        var request = new CardRequestContext();
        //        var shuouldTrigger =
        //            await OnRequestTriggerSkill(shaSkill.GetButtonInfo().SkillType, request);
        //        if (shuouldTrigger)
        //        {
        //            var response = new CardResponseContext();
        //            await shaSkill.GetButtonInfo().OnClick(request, PlayerContext.Player.RoundContext, response);
        //            if (response.ResponseResult == ResponseResultEnum.Success || response.Cards?.Any() == true)
        //            {
        //                //触发了技能，返回一张ChangedCard或者MutedCard.
        //                var virtualCard = response.Cards.First();
        //                if (!shouldContinue)
        //                {
        //                    shouldContinue = await PlaySha(new List<CardBase>() { virtualCard });
        //                }
        //            }
        //        }
        //    }
        //    return shouldContinue;
        //}

        /// <summary>
        /// 处理能够增强杀的技能
        /// </summary>
        /// <returns></returns>
        private async Task<bool> TriggerSkill_EnhanceSha()
        {
            var shouldContinue = false;
            #region 处理能够增强杀的技能

            //目前只有三板斧需要增强
            var enhanceSkills = PlayerContext.Player.GetAllSkillButtons().Where(s => s.IsEnabled() && s is IEnhanceSha).OrderByDescending(s => (s as IEnhanceSha).EnhancePriority());
            foreach (var enhanceSkill in enhanceSkills)
            {
                var request = new CardRequestContext();
                var shuouldTrigger =
                    await OnRequestTriggerSkill(enhanceSkill.GetButtonInfo().SkillType, request);
                if (shuouldTrigger)
                {
                    var response = new CardResponseContext();
                    await enhanceSkill.GetButtonInfo().OnClick(request, PlayerContext.Player.RoundContext, response);
                    //如三板斧没有被cancel，（比如要发动三板斧，却没有杀的情况）
                    if (response.ResponseResult != ResponseResultEnum.Cancelled)
                    {
                        shouldContinue = true;
                    }
                }
            }

            #endregion

            return shouldContinue;
        }

        /// <summary>
        /// 处理所有的主动技能
        /// </summary>
        /// <returns></returns>
        private async Task<bool> TriggerSkills()
        {
            var shouldContinue = false;

            var skills = PlayerContext.Player.GetAllSkillButtons().Where(s => s.IsEnabled());
            foreach (var enhanceSkill in skills)
            {
                var request = new CardRequestContext();
                var shuouldTrigger =
                    await OnRequestTriggerSkill(enhanceSkill.GetButtonInfo().SkillType, request);
                if (shuouldTrigger)
                {
                    var response = new CardResponseContext();
                    await enhanceSkill.GetButtonInfo().OnClick(request, PlayerContext.Player.RoundContext, response);
                    //如果发动过技能，则需要继续下一次判断
                    if (response.ResponseResult != ResponseResultEnum.Cancelled)
                    {
                        shouldContinue = true;
                    }
                }
            }

            return shouldContinue;
        }

        /// <summary>
        /// 打出休养生息
        /// </summary>
        /// <returns>返回是否要继续下个回合检查出牌</returns>
        private async Task<bool> EquipWeapon<T>() where T : EquipmentBase
        {
            bool shouldContinue = true;

            var card = PlayerContext.Player.CardsInHand.FirstOrDefault(p => p is T);
            if (card != null)
            {
                await card.PlayCard(new CardRequestContext(), PlayerContext.Player.RoundContext);
                //出过牌，继续检查是否有需要出的牌
                shouldContinue = false;
            }

            return shouldContinue;
        }

        /// <summary>
        /// 是否应该装备虎符。
        ///     如果当前攻击范围内（没有武器情况下）有敌人且手中有杀或者能够提供杀，则装备
        /// </summary>
        /// <returns></returns>
        private bool ShouldEquipHufu()
        {
            var shaDistance = PlayerContext.Player.RoundContext.AttackDynamicFactor.ShaDistance +
                              PlayerContext.Player.RoundContext.AttackDynamicFactor.TannangDistance;
            var nextPlayer = PlayerContext.Player.GetNextPlayer(false);
            bool shouldEquip = false;
            //检查没有武器情况下攻击范围内是否有敌人
            while (nextPlayer != null && nextPlayer != PlayerContext.Player)
            {
                if (!nextPlayer.IsSameGroup(PlayerContext.Player))
                {
                    var dist = PlayerContext.GameLevel.GetPlayersDistance(PlayerContext.Player, nextPlayer);
                    if (dist.ShaDistanceWithouWeapon <= shaDistance)
                    {
                        shouldEquip = true;
                        break;
                    }
                }

                nextPlayer = nextPlayer.GetNextPlayer(false);
            }
            //检查是否有杀或者技能能提供杀
            if (shouldEquip)
            {
                shouldEquip = PlayerContext.Player.CardsInHand?.Any(p => p is Sha) == true;
                if (shouldEquip)
                {
                    return shouldEquip;
                }
                var shaSkills = PlayerContext.Player.GetAllSkillButtons().Where(s => s.IsEnabled() && s is IAbility ability && ability.CanProvideSha());
                shouldEquip = shaSkills.Any();
            }
            return shouldEquip;
        }

        /// <summary>
        /// 打出武器牌
        /// </summary>
        /// <returns>返回是否要继续下个回合检查出牌</returns>
        private async Task<bool> PlayWeaponCards()
        {
            bool shouldContinueEquipWeapon = true;
            //装备武器逻辑：默认装备攻击距离最远的装备和进攻马

            //武器牌：
            //  博浪锤
            if (shouldContinueEquipWeapon)
            {
                shouldContinueEquipWeapon = await EquipWeapon<Bolangchui>();
            }
            //  霸王弓
            if (shouldContinueEquipWeapon)
            {
                shouldContinueEquipWeapon = await EquipWeapon<Bawanggong>();
            }
            //  狼牙棒
            if (shouldContinueEquipWeapon)
            {
                shouldContinueEquipWeapon = await EquipWeapon<Langyabang>();
            }
            //  芦叶枪
            if (shouldContinueEquipWeapon)
            {
                shouldContinueEquipWeapon = await EquipWeapon<Luyeqiang>();
            }
            //  盘龙棍
            if (shouldContinueEquipWeapon)
            {
                shouldContinueEquipWeapon = await EquipWeapon<Panlonggun>();
            }
            //  龙鳞刀
            if (shouldContinueEquipWeapon)
            {
                shouldContinueEquipWeapon = await EquipWeapon<Luyeqiang>();
            }

            //  鱼肠剑
            if (shouldContinueEquipWeapon)
            {
                shouldContinueEquipWeapon = await EquipWeapon<Yuchangjian>();
            }
            //  进攻马
            if (shouldContinueEquipWeapon)
            {
                shouldContinueEquipWeapon = await EquipWeapon<Jingongma>();
            }
            //  虎符
            if (shouldContinueEquipWeapon && ShouldEquipHufu())
            {
                //是否要装备虎符？
                shouldContinueEquipWeapon = await EquipWeapon<Hufu>();
            }

            //防具：
            //  防御马
            //  玉如意 
            await EquipWeapon<Yuruyi>();
            await EquipWeapon<Fangyuma>();

            return !shouldContinueEquipWeapon;
        }

        #endregion

        #region 芦叶枪技能
        /// <summary>
        /// 是否要发动龙鳞刀.
        /// </summary>
        /// <param name="cardRequestContext"></param>
        /// <returns></returns>
        private bool ShouldTriggerSkill_Luyeqiang(CardRequestContext cardRequestContext)
        {
            var target = cardRequestContext.TargetPlayers?.FirstOrDefault();
            if (target == null && PlayerContext.Player.CardsInHand.Count < 4)
            {
                return false;
            }
            //如果是队友，血量低于3则卸牌，否则不触发，其他情况都触发

            if (target != null && target.IsSameGroup(cardRequestContext.SrcPlayer) && target.GetCurrentPlayerHero().CurrentLife <= 3)
            {
                return false;
            }

            return true;
        }



        #endregion

        #region 玉如意技能
        /// <summary>
        /// 是否要发动玉如意.
        /// </summary>
        /// <param name="cardRequestContext"></param>
        /// <returns></returns>
        private bool ShouldTriggerSkill_Yuruyi(CardRequestContext cardRequestContext)
        {
            var target = cardRequestContext.TargetPlayers.FirstOrDefault();
            if (target == null)
            {
                throw new Exception("攻击目标不能为空。");
            }
            //如果是队友，返回false
            //如果是敌人，则返回true
            if (target.IsSameGroup(cardRequestContext.SrcPlayer) && PlayerContext.Player.GetCurrentPlayerHero().CurrentLife > 3)
            {
                return false;
            }
            else
            {
                return true;
            }
        }


        #endregion

        #region 盘龙棍技能
        /// <summary>
        /// 是否要发动盘龙棍.
        /// </summary>
        /// <param name="cardRequestContext"></param>
        /// <returns></returns>
        private bool ShouldTriggerSkill_Panlonggun(CardRequestContext cardRequestContext)
        {
            var target = cardRequestContext.TargetPlayers.FirstOrDefault();
            if (target == null)
            {
                throw new Exception("攻击目标不能为空。");
            }
            //如果是队友，返回false
            //如果是敌人，则返回true
            if (target.IsSameGroup(cardRequestContext.SrcPlayer))
            {
                return false;
            }
            else
            {
                return true;
            }
        }


        #endregion

        #region 龙鳞刀技能
        /// <summary>
        /// 是否要发动龙鳞刀.
        /// </summary>
        /// <param name="cardRequestContext"></param>
        /// <returns></returns>
        private bool ShouldTriggerSkill_Longlindao(CardRequestContext cardRequestContext)
        {
            var target = cardRequestContext.TargetPlayers.FirstOrDefault();
            if (target == null)
            {
                throw new Exception("攻击目标不能为空。");
            }
            //如果是队友，血量低于3则卸牌，否则掉血
            //如果是敌人，如果血量低于3，则掉血，否则卸牌
            if (target.IsSameGroup(cardRequestContext.SrcPlayer))
            {
                if (target.GetCurrentPlayerHero().CurrentLife <= 3)
                {
                    return true;
                }
                return false;
            }
            else
            {
                if (target.GetCurrentPlayerHero().CurrentLife <= 3)
                {
                    return false;
                }
                return true;
            }
        }


        #endregion

        #region 博浪锤技能

        /// <summary>
        /// 是否要发动博浪锤.
        /// 发动条件：
        ///     1. 被攻击的对象是敌方
        ///     2. 且本人手牌和装备牌中有至少两张手牌（除博浪锤之外）
        /// </summary>
        /// <param name="cardRequestContext"></param>
        /// <returns></returns>
        private bool ShouldTriggerSkill_Bolangchui(CardRequestContext cardRequestContext)
        {
            var target = cardRequestContext.TargetPlayers.FirstOrDefault();
            if (target == null)
            {
                throw new Exception("攻击目标不能为空。");
            }

            if (target.IsSameGroup(cardRequestContext.SrcPlayer))
            {
                return false;
            }

            var totalCount = cardRequestContext.SrcPlayer.CardsInHand.Count +
                 cardRequestContext.SrcPlayer.EquipmentSet.Count(p => !(p is Bolangchui));
            if (totalCount >= 2)
            {
                return true;
            }
            return false;
        }

        #endregion

        private async Task RemoveEquipment<T>(CardResponseContext responseContext, PanelBase panel)
        {
            //如果有画地为牢，则将其取掉
            var equipment = panel.EquipmentCards.FirstOrDefault(p => p.Card is T);
            if (equipment != null)
            {
                //Console.WriteLine($"{PlayerContext.Player.PlayerId}的【{PlayerContext.Player.GetCurrentPlayerHero().Hero.DisplayName}】从{panel.CardOwner.PlayerId}的【{panel.CardOwner.GetCurrentPlayerHero().Hero.DisplayName}】抽取了{equipment.Card.DisplayName}");
                //await panel.CardOwner.RemoveEquipment(equipment.Card, null, null, null);
                equipment.SelectedBy = PlayerContext.Player;
                responseContext.Cards.Add(equipment.Card);
            }

            await Task.FromResult(0);
        }

        /// <summary>
        /// 处理从手牌、装备区中出牌
        /// </summary>
        /// <param name="cardRequestContext"></param>
        /// <param name="excludeEquipCards">要排除的卡类型(装备区)</param>
        /// <returns></returns>
        private async Task<CardResponseContext> GetResponseCardByCardType_InHandsAmdEquipment(CardRequestContext cardRequestContext, List<string> excludeEquipCards)
        {
            var sumCards = new List<CardBase>(PlayerContext.Player.CardsInHand);
            sumCards.AddRange(excludeEquipCards == null ? PlayerContext.Player.EquipmentSet : PlayerContext.Player.EquipmentSet.Where(p => excludeEquipCards.All(e => !p.Name.Equals(e))));

            var cardsToPlay = await GetCardsOrderByAiValue(cardRequestContext, sumCards, true);
            return new CardResponseContext()
            {
                Cards = cardsToPlay.ToList(),
            };
        }

        /// <summary>
        /// 处理响应群体技能，如无懈可击、药
        /// </summary>
        /// <param name="cardRequestContext"></param>
        /// <returns></returns>
        private async Task<CardResponseContext> GetResponseCardByCardType_GroupRequestWithConfirm(CardRequestContext cardRequestContext)
        {
            if (cardRequestContext.RequestCard is Wuxiekeji)
            {
                //出无懈可击的条件：
                //烽火狼烟、万箭齐发：    请求来源是同阵营的且请求玩家血量低于3时可以出无懈
                //五谷丰登： 如果请求来源是敌方阵营，且待摸的牌中有AiValue>wuzhongshengyou(药、无中生有、探囊取物、虎符)的则无懈
                //探囊取物、釜底抽薪、画地为牢：    如果是请求来源是友方，则无懈可击
                //无中生有： 如果请求来源是敌方，则无懈可击
                //休养生息： 如果请求来源是敌方且敌方生命<=2则无懈可击
                //决斗：   如果请求来源是友方且友方血量<=2则无懈
                //借刀杀人、无懈可击： 如果请求来源（发动借刀的人）是敌方，则无懈可击
                if (cardRequestContext.AttackType == AttackTypeEnum.Fenghuolangyan ||
                    cardRequestContext.AttackType == AttackTypeEnum.Wanjianqifa)
                {
                    if (cardRequestContext.SrcPlayer.IsSameGroup(PlayerContext.Player) && cardRequestContext.SrcPlayer.GetCurrentPlayerHero().CurrentLife < 3)
                    {
                        return await GetResponseCardByCardType_Any(cardRequestContext);
                    }
                }
                else if (cardRequestContext.AttackType == AttackTypeEnum.Wugufengdeng)
                {
                    if (!cardRequestContext.SrcPlayer.IsSameGroup(PlayerContext.Player) &&
                        cardRequestContext.SrcCards?.Any(c => c is Yao || c is Wuzhongshengyou || c is Tannangquwu || c is Hufu) == true)
                    {
                        return await GetResponseCardByCardType_Any(cardRequestContext);
                    }
                }
                else if (cardRequestContext.AttackType == AttackTypeEnum.Tannangquwu ||
                         cardRequestContext.AttackType == AttackTypeEnum.Huadiweilao ||
                                         cardRequestContext.AttackType == AttackTypeEnum.Fudichouxin)
                {
                    if (cardRequestContext.SrcPlayer.IsSameGroup(PlayerContext.Player))
                    {
                        return await GetResponseCardByCardType_Any(cardRequestContext);
                    }
                }
                else if (cardRequestContext.AttackType == AttackTypeEnum.Wuzhongshengyou)
                {
                    if (!cardRequestContext.SrcPlayer.IsSameGroup(PlayerContext.Player))
                    {
                        return await GetResponseCardByCardType_Any(cardRequestContext);
                    }
                }
                else if (cardRequestContext.AttackType == AttackTypeEnum.Xiuyangshengxi)
                {
                    if (!cardRequestContext.SrcPlayer.IsSameGroup(PlayerContext.Player) && cardRequestContext.SrcPlayer.GetCurrentPlayerHero().CurrentLife < 3)
                    {
                        return await GetResponseCardByCardType_Any(cardRequestContext);
                    }
                }
                else if (cardRequestContext.AttackType == AttackTypeEnum.Juedou)
                {
                    if ((cardRequestContext.SrcPlayer.IsSameGroup(PlayerContext.Player) && cardRequestContext.SrcPlayer.GetCurrentPlayerHero().CurrentLife < 3) || cardRequestContext.SrcPlayer == PlayerContext.Player)
                    {
                        return await GetResponseCardByCardType_Any(cardRequestContext);
                    }
                }
                else if (cardRequestContext.AttackType == AttackTypeEnum.Wuxiekeji || cardRequestContext.AttackType == AttackTypeEnum.Jiedaosharen)
                {
                    if (!cardRequestContext.SrcPlayer.IsSameGroup(PlayerContext.Player))
                    {
                        return await GetResponseCardByCardType_Any(cardRequestContext);
                    }
                }
            }
            //出药的条件：是友方需要药
            else if (cardRequestContext.RequestCard is Yao)
            {
                if (!cardRequestContext.SrcPlayer.IsSameGroup(PlayerContext.Player))
                {
                    return await GetResponseCardByCardType_Any(cardRequestContext);
                }
            }

            return new CardResponseContext()
            {
                ResponseResult = ResponseResultEnum.Failed,
            };
        }

        /// <summary>
        /// 处理借刀杀人
        /// </summary>
        /// <param name="cardRequestContext"></param>
        /// <returns></returns>
        private async Task<CardResponseContext> GetResponseCardByCardType_Jiedaosharen(CardRequestContext cardRequestContext)
        {
            //什么情况不出杀？
            //没有杀或者目标是队友且队友的血量低于3
            var target = cardRequestContext.TargetPlayers.First();
            if (target.IsSameGroup(PlayerContext.Player) && target.GetCurrentPlayerHero().CurrentLife <= 3)
            {
                return new CardResponseContext()
                {
                    ResponseResult = ResponseResultEnum.Failed,
                    Cards = new List<CardBase>()
                };
            }

            var res = await OnRequestResponseCard(new CardRequestContext()
            {
                MaxCardCountToPlay = 1,
                MinCardCountToPlay = 1,
                AttackType = AttackTypeEnum.None,
                RequestCard = new Sha()
            });
            return res;
        }

        /// <summary>
        /// 处理从手牌中出牌
        /// </summary>
        /// <param name="cardRequestContext"></param>
        /// <returns></returns>
        private async Task<CardResponseContext> GetResponseCardByCardType_InHands(CardRequestContext cardRequestContext)
        {
            var sumCards = new List<CardBase>(PlayerContext.Player.CardsInHand);

            var cardsToPlay = await GetCardsOrderByAiValue(cardRequestContext, sumCards, true);
            return new CardResponseContext()
            {
                Cards = cardsToPlay.ToList(),
            };
        }

        /// <summary>
        /// 处理“任意”类型的请求
        /// </summary>
        /// <param name="cardRequestContext"></param>
        /// <returns></returns>
        private async Task<CardResponseContext> GetResponseCardByCardType_Any(CardRequestContext cardRequestContext)
        {
            await Task.Delay(new Random().Next(10, 100));
            var sumCards = new List<CardBase>(PlayerContext.Player.CardsInHand);
            //返回一张价值最低的牌
            if (PlayerContext.Player.EquipmentSet != null)
            {
                sumCards.AddRange(PlayerContext.Player.EquipmentSet);
            }

            var cardsToPlay = await GetCardsOrderByAiValue(cardRequestContext, sumCards, true);
            return new CardResponseContext()
            {
                Cards = cardsToPlay?.ToList(),
            };
        }

        /// <summary>
        /// 按照AI value由低到高取开
        /// </summary>
        /// <param name="cardRequestContext"></param>
        /// <param name="cards"></param>
        /// <param name="asc">ai value 升序</param>
        /// <returns></returns>
        private async Task<List<CardBase>> GetCardsOrderByAiValue(CardRequestContext cardRequestContext, List<CardBase> cards, bool asc)
        {
            //没有指定出什么牌，则返回任意牌
            if (cardRequestContext.RequestCard != null)
            {
                cards = cards.Where(s => s.GetType() == cardRequestContext.RequestCard.GetType()).ToList();
            }

            List<CardBase> cardsToPlay = new List<CardBase>();
            if (cards.Count >= cardRequestContext.MinCardCountToPlay)
            {
                var avCards = asc ? cards.OrderBy(p => GetCardAiValue(p).Value) : cards.OrderByDescending(p => GetCardAiValue(p).Value);

                cardsToPlay = avCards.Take(cardRequestContext.MaxCardCountToPlay).ToList();
                return cardsToPlay;
            }
            else
            {
                var skillBtns = PlayerContext.Player.GetAllSkillButtons();

                //如果不够，看有没有技能能提供
                //todo:暂时是技能能够提供所需卡牌就触发
                if (cardRequestContext.RequestCard != null && skillBtns.Any())
                {
                    IEnumerable<ISkillButton> availiableSkills = null;
                    if (cardRequestContext.RequestCard is Sha)
                    {
                        availiableSkills = skillBtns.Where(s => s.IsEnabled() && s is IAbility && ((IAbility)s).CanProvideSha());
                    }
                    else if (cardRequestContext.RequestCard is Shan)
                    {
                        availiableSkills = skillBtns.Where(s => s.IsEnabled() && s is IAbility && ((IAbility)s).CanProvideShan());
                    }
                    else if (cardRequestContext.RequestCard is Yao)
                    {
                        availiableSkills = skillBtns.Where(s => s.IsEnabled() && s is IAbility && ((IAbility)s).CanProvideYao());
                    }
                    else if (cardRequestContext.RequestCard is Wuxiekeji)
                    {
                        availiableSkills = skillBtns.Where(s => s.IsEnabled() && s is IAbility && ((IAbility)s).CanProviderWuxiekeji());
                    }

                    cardsToPlay.AddRange(await GetCardsFromSkills(cardRequestContext, availiableSkills));
                }
            }

            //不够就不出
            return cardsToPlay;
        }

        private async Task<List<CardBase>> GetCardsFromSkills(CardRequestContext cardRequest, IEnumerable<ISkillButton> availiableSkills)
        {
            List<CardBase> cardsToPlay = new List<CardBase>();
            foreach (var availiableSkill in availiableSkills)
            {
                if (cardsToPlay.Count > cardRequest.MaxCardCountToPlay)
                {
                    break;
                }
                var shouldTrigger = await OnRequestTriggerSkill(availiableSkill.GetButtonInfo().SkillType, cardRequest);
                if (shouldTrigger)
                {
                    var tempRes = new CardResponseContext();
                    await availiableSkill.GetButtonInfo().OnClick(cardRequest, null, tempRes);
                    if (tempRes.ResponseResult == ResponseResultEnum.Success)
                    {
                        cardsToPlay.AddRange(tempRes.Cards);
                        continue;
                    }
                }
            }
            return cardsToPlay;
        }
        #endregion

    }

    public class AiValue
    {
        public int Priority { get; set; }
        public double Value { get; set; }
    }
}
