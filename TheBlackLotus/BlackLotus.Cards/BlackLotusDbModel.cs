using EixoX.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BlackLotus.Cards
{
    public abstract class BlackLotusDbModel<T>
    {
        public static ClassSelect<T> Select()
        {
            return BlackLotusDb<T>.Instance.Select();
        }

        public static ClassSelect<T> Search(string filter)
        {
            return BlackLotusDb<T>.Instance.Search(filter);
        }

        public static T WithIdentity(object identity)
        {
            return BlackLotusDb<T>.Instance.WithIdentity(identity);
        }

        public static T WithMember(string memberName, object memberValue)
        {
            return BlackLotusDb<T>.Instance.WithMember(memberName, memberValue);
        } 
    }
}