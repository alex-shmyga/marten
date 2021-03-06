using System;
using System.Linq;
using System.Reflection;
using Baseline;
using Marten.Util;
using NpgsqlTypes;

namespace Marten.Schema
{
    public abstract class Field
    {
        protected Field(EnumStorage enumStorage, MemberInfo member, bool notNull = false) : this(enumStorage, new[] { member }, notNull)
        {
        }

        protected Field(EnumStorage enumStorage, MemberInfo[] members, bool notNull = false)
        {
            Members = members;
            MemberName = members.Select(x => x.Name).Join("");

            FieldType = members.Last().GetMemberType();

            PgType = TypeMappings.GetPgType(FieldType, enumStorage);
            _enumStorage = enumStorage;

            NotNull = notNull;
        }

        public Type FieldType { get; }
        public string PgType { get; set; } // settable so it can be overidden by users

        public MemberInfo[] Members { get; }
        public string MemberName { get; }

        public NpgsqlDbType NpgsqlDbType => TypeMappings.ToDbType(FieldType);

        public bool NotNull { get; }

        protected readonly EnumStorage _enumStorage;
    }
}
