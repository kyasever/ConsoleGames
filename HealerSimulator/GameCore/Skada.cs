using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace HealerSimulator
{
    public class SkadaData
    {
        public int Damage = 0;
        public int BeDamaged = 0;
        public int Heal = 0;
        public int BeHeal = 0;
    }

    /// <summary>
    /// 一条消息记录的构成
    /// </summary>
    public struct SkadaRecord
    {
        /// <summary>
        /// 变动的来源
        /// </summary>
        public Character Source;
        /// <summary>
        /// 变动的承受者
        /// </summary>
        public Character Accept;
        /// <summary>
        /// 使用的技能
        /// </summary>
        public Skill UseSkill;
        /// <summary>
        /// 变动的数值
        /// </summary>
        public int Value;


    }

    /// <summary>
    /// DPS统计器,单例模式,可以清空,可以输出结果
    /// </summary>
    public class Skada
    {
        #region 单例
        private static Skada instance;
        public static Skada Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new Skada();
                }
                return instance;
            }
        }
        private Skada() { }
        #endregion

        public Dictionary<Character, SkadaData> data = new Dictionary<Character, SkadaData>();

        public List<SkadaRecord> recordList = new List<SkadaRecord>();

        /// <summary>
        /// 获取DPS排行榜
        /// </summary>
        public List<KeyValuePair<Character, SkadaData>> GetDamageRank()
        {
            List<KeyValuePair<Character, SkadaData>> list = new List<KeyValuePair<Character, SkadaData>>();
            foreach (var v in data)
            {
                list.Add(v);
            }
            //按照造成伤害排序并返回
            list.Sort((a, b) => { return a.Value.Damage.CompareTo(b.Value.Damage); });
            return list;
        }

        /// <summary>
        /// 添加一条统计信息
        /// </summary>
        public void AddRecord(SkadaRecord record)
        {
            recordList.Add(record);
            //说明造成的是伤害
            if (record.Value <= 0)
            {
                SafeGet(record.Source).Damage += record.Value;
                SafeGet(record.Accept).BeDamaged += record.Value;
            }
            else
            {
                SafeGet(record.Source).Heal += record.Value;
                SafeGet(record.Accept).BeHeal += record.Value;
            }
        }

        public SkadaData SafeGet(Character c)
        {
            if (!data.ContainsKey(c))
            {
                data[c] = new SkadaData();
            }
            return data[c];
        }
    }
}
