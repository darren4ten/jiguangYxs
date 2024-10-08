﻿using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using Logic.Annotations;
using Logic.Enums;
using Logic.GameLevel;
using Logic.Model.Enums;
using Logic.Model.Skill;

namespace Logic.Model.Hero
{
    public abstract class HeroBase : IHero, INotifyPropertyChanged
    {
        public int Id { get; set; }
        public HeroGroupEnum HeroGroup { get; set; }

        private string _name;
        /// <summary>
        /// 英雄名称
        /// </summary>
        public string Name
        {
            get => _name;
            set
            {
                _name = value;
                OnPropertyChanged();
            }
        }

        private string _displayName;
        /// <summary>
        /// 显示名称
        /// </summary>
        public string DisplayName
        {
            get => _displayName;
            set
            {
                _displayName = value;
                OnPropertyChanged();
            }
        }

        /// <summary>
        /// 先手值
        /// </summary>
        public int Xianshou { get; set; }

        /// <summary>
        /// 性别
        /// </summary>
        public GenderEnum Gender { get; set; }

        /// <summary>
        /// 最大生命值
        /// </summary>
        public int MaxLife { get; set; }

        /// <summary>
        /// 攻击属性
        /// </summary>
        protected AttackDynamicFactor AttackFactor { get; set; }

        public string Image { get; set; }

        /// <summary>
        /// 主技能集合
        /// </summary>
        public List<SkillBase> MainSkillSet { get; set; }

        /// <summary>
        /// 副技能集合
        /// </summary>
        public List<SkillBase> SubSkillSet { get; set; }

        public HeroBase()
        {
            AttackFactor = AttackDynamicFactor.GetDefaultBaseAttackFactor();
        }

        public AttackDynamicFactor GetBaseAttackFactor()
        {
            return AttackFactor;
        }

        public event PropertyChangedEventHandler PropertyChanged;

        [NotifyPropertyChangedInvocator]
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
