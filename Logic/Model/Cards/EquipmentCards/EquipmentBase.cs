using Logic.Cards;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Logic.GameLevel;
using Logic.Model.Cards.Interface;
using Logic.Model.Interface;

namespace Logic.Model.Cards.EquipmentCards
{
    public class EquipmentBase : CardBase, IEquipment, IAbility
    {
        //public int DefenseDistance { get; set; }
        //public int TannangDistance { get; set; }
        //public int AttackFactor.ShaDistance { get; set; }
        //public int MaxShaCount { get; set; }
        //public int MaxShaTargetCount { get; set; }
        public AttackDynamicFactor AttackFactor { get; set; }

        public override bool CanBePlayed()
        {
            return false;
        }

        public override Task Popup()
        {
            throw new NotImplementedException();
        }

        public Task Equip()
        {
            throw new NotImplementedException();
        }

        public Task UnEquip()
        {
            throw new NotImplementedException();
        }

        public bool IsViewableInSkillPanel()
        {
            return false;
        }

        public bool CanProvideSha()
        {
            throw new NotImplementedException();
        }

        public bool CanProvideShan()
        {
            throw new NotImplementedException();
        }

        public bool CanProvideJuedou()
        {
            throw new NotImplementedException();
        }

        public bool CanProvideFenghuolangyan()
        {
            throw new NotImplementedException();
        }

        public bool CanProvideWanjianqifa()
        {
            throw new NotImplementedException();
        }

        public bool CanProvideTannangquwu()
        {
            throw new NotImplementedException();
        }
    }
}
