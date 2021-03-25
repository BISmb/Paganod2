using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Paganod.Shared.Type.Query
{
    public enum OrderBy
    {
        Asc,
        Desc
    }

    public enum FilterOrder
    {
        Default,
        Asc,
        Desc
    }

    public enum FilterType
    {
        Equals,
        In,
        BeginsWith,
        EndsWith,
        Contains,
        GreaterThan,
        GreaterThanOrEqual,
        LessThan,
        LessThanOrEqual,
        NotEquals
    }

    public enum FilterAndOr
    {
        NotSet,
        And,
        Or
    }

    public class GroupFilter
    {
        public int GroupId { get; set; }
        public FilterAndOr AndOr { get; set; }
        public List<ODataFilter> Filters { get; set; }
    }

    public class ODataQuery
    {
        public string BaseTableName { get; set; }
        public List<string> SelectFields { get; set; } = new();
        public Dictionary<string, OrderBy> Order { get; set; } = new();
        public int Skip { get; set; }
        public int Top { get; set; }
        public bool Count { get; set; }


        public List<GroupFilter> GroupFilters { get; set; } = new();

        public List<ODataFilter> Filters { get; set; } = new();

        private int _GroupId { get; set; }
        private FilterAndOr _GroupAndOr { get; set; }

        private Dictionary<string, string> _QueryParams = null;

        public ODataQuery(string strBaseTableName)
        {
            BaseTableName = strBaseTableName;
        }

        public ODataQuery(string strBaseTableName, Dictionary<string, string> dicQueryParams)
        {
            BaseTableName = strBaseTableName;
            _QueryParams = dicQueryParams;

            // take out of string a setup query object
            // strQueryString
        }

        //public ODataQuery() { }

        public ODataQuery Select(params string[] fieldsToSelect)
        {
            SelectFields.AddRange(fieldsToSelect);
            return this;
        }

        //public ODataQuery OrderBy(string orderByField)
        //{
        //    OrderByField = orderByField;
        //    return this;
        //}

        public ODataQuery OrderBy(string strOrderByField, OrderBy eOrder = Query.OrderBy.Asc)
        {
            Order.Add(strOrderByField, eOrder);
            return this;
        }

        public ODataQuery OrderBy(params (string orderByField, FilterOrder orderType)[] orderClauses)
        {
            return this;
        }

        //public ODataQuery Group(Action<ODataQuery> additionalFilters)
        //{
        //    _GroupId = GroupFilters.Count;
        //    additionalFilters(this);
        //    return this;
        //}

        //public ODataQuery GroupAnd(Action<ODataQuery> additionalFilters)
        //{
        //    _GroupAndOr = Enums.Data.FilterAndOr.And;
        //    Group(additionalFilters);
        //    return this;
        //}

        //public ODataQuery GroupOr(Action<ODataQuery> additionalFilters)
        //{
        //    _GroupAndOr = Enums.Data.FilterAndOr.Or;
        //    Group(additionalFilters);
        //    return this;
        //}

        public IDictionary<string, string> GetDynamicDictionary()
        {
            if (_QueryParams is not null)
                return _QueryParams;

            IList<ODataFilter> orderedFilters = Filters.OrderBy(x => x._Index).ToList();
            string strFinalFilter = "";

            foreach (var group in GroupFilters)
            {
                if (group.AndOr != FilterAndOr.NotSet)
                    strFinalFilter += (group.AndOr == FilterAndOr.And) ? " and " : " or ";

                strFinalFilter += $"({String.Join(" ", group.Filters)})";
            }

            string strOrderBy = "";
            for (int o = 0; o < Order.Count; o++)
            {
                var oOrderBy = Order.ElementAt(0);
                if (o != 0) strOrderBy += ", ";
                strOrderBy = $"{oOrderBy.Key} {oOrderBy.Value.ToString().ToLower()}";
            }

            return new Dictionary<string, string>()
            {
                { "table", BaseTableName },
                { "select", string.Join(",", SelectFields) },
                { "orderby", strOrderBy },
                { "filter", strFinalFilter },
                { "skip", $"{Skip}" },
                { "top", $"{Top}" },
            };
        }

        public ODataQuery WithPaging(int intPage, int intPageSize)
        {
            Skip = (intPage * intPageSize) - intPageSize;
            Top = (intPage * intPageSize); // intPageSize;

            return this;
        }

        public ODataQuery Where(string strPropertyName, FilterType enumFilterType, object oValue)
            => Add(strPropertyName, enumFilterType, oValue, false, FilterAndOr.NotSet);

        public ODataQuery And(string strPropertyName, FilterType enumFilterType, object oValue)
            => Add(strPropertyName, enumFilterType, oValue, false, FilterAndOr.And);

        public ODataQuery Or(string strPropertyName, FilterType enumFilterType, object oValue)
            => Add(strPropertyName, enumFilterType, oValue, false, FilterAndOr.Or);

        public ODataQuery AndNot(string strPropertyName, FilterType enumFilterType, object oValue)
            => Add(strPropertyName, enumFilterType, oValue, true, FilterAndOr.And);

        public ODataQuery OrNot(string strPropertyName, FilterType enumFilterType, object oValue)
            => Add(strPropertyName, enumFilterType, oValue, true, FilterAndOr.Or);

        private ODataQuery Add(string strPropertyName, FilterType enumFilterType, object oValue, bool not, FilterAndOr andOr)
        {
            //Filters.Add(new ODataFilter(Filters.Count, strPropertyName, enumFilterType, oValue, not, andOr));

            if (!GroupFilters.Where(x => x.GroupId == _GroupId).Any())
                GroupFilters.Add(new GroupFilter() { GroupId = _GroupId, AndOr = _GroupAndOr, Filters = new() });

            //var group = .First(x => x.GroupId == _GroupId);
            GroupFilters[0].AndOr = _GroupAndOr;
            GroupFilters[0].Filters.Add(new ODataFilter(Filters.Count, strPropertyName, enumFilterType, oValue, not, andOr));

            return this;
        }
    }

    public class ODataFilter
    {
        public int _Index { get; set; }

        public string PropertyName { get; set; }
        public FilterType FilterType { get; set; }
        public object Value { get; set; }

        public FilterAndOr AndOr { get; set; }
        public bool Not { get; set; }

        //public Type ValueType => Value.GetType();
        //public FieldType TargetType { get; set; }

        public ODataFilter() { }

        public ODataFilter(int newIndex, string strPropertyName, FilterType enumFilterType, object oValue, bool blnNot = false, FilterAndOr andOr = FilterAndOr.NotSet)
        {
            _Index = newIndex;

            PropertyName = strPropertyName;
            FilterType = enumFilterType;
            Value = oValue;
            Not = blnNot;
            AndOr = andOr;
        }

        public override string ToString()
        {
            IList<string> Fragments = new List<string>();

            switch (AndOr)
            {
                case FilterAndOr.And:
                    Fragments.Add("and");
                    break;
                case FilterAndOr.Or:
                    Fragments.Add("or");
                    break;

                case FilterAndOr.NotSet:
                default:
                    break;
            };

            if (Not) Fragments.Add("not");

            if (Value.GetType() == typeof(string))
                Value = $"'{Value}'";

            if (Value.GetType() == typeof(string[]))
                for (int i = 0; i < ((string[])Value).Length; i++)
                    ((string[])Value)[i] = $"'{((string[])Value)[i]}'";

            if (typeof(IEnumerable<>).IsAssignableFrom(Value.GetType()))
                Value = string.Join(',', $"{(IEnumerable<object>)Value}");


            string FilterValue = $"'{Value}'";

            if (Int32.TryParse(Value.ToString(), out _)
                || Int64.TryParse(Value.ToString(), out _)
                || Decimal.TryParse(Value.ToString(), out _)
                || Double.TryParse(Value.ToString(), out _)
            )
                FilterValue = $"{Value}";


            //string FilterValue = Value switch
            //{
            //    DateTime => $"'{Value}'",
            //    string => $"'{Value}'",
            //    char => $"'{Value}'",

            //    _ => $"{Value}",
            //};

            string FilterString = FilterType switch
            {
                FilterType.BeginsWith => $"beginswith({PropertyName}, {FilterValue})",
                FilterType.EndsWith => $"endswith({PropertyName}, {FilterValue})",
                FilterType.In => $"{PropertyName} in ({FilterValue})",
                FilterType.Contains => $"contains({PropertyName},{FilterValue})",
                FilterType.Equals => $"{PropertyName} eq {FilterValue}",
                FilterType.NotEquals => $"{PropertyName} ne {FilterValue}",

                FilterType.GreaterThanOrEqual => $"{PropertyName} ge {FilterValue}",
                FilterType.GreaterThan => $"{PropertyName} gt {FilterValue}",
                FilterType.LessThanOrEqual => $"{PropertyName} le {FilterValue}",
                FilterType.LessThan => $"{PropertyName} lt {FilterValue}",



                _ => throw new NotImplementedException(),
            };

            Fragments.Add(FilterString);

            return String.Join(" ", Fragments);
        }
    }
}
