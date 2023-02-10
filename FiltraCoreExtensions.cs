using Newtonsoft.Json;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic.Core;

namespace HBH.FiltraCore
{
    public static class FiltraCoreExtensions
    {
        public static IQueryable<T> ApplyAdvanceFilters<T, TInput>(this IQueryable<T> qry, TInput input)
        {
            return qry.ApplyTerm(input)
                      .ApplyFilters(input);
        }

        /// <summary>
        /// this function reads the term field from input and apply the filter on the query base on TermBy field
        /// </summary>
        private static IQueryable<T> ApplyTerm<T, TInput>(this IQueryable<T> qry, TInput input)
        {
            var term = (string)input.GetType().GetProperty("Term")?.GetValue(input);

            var termBy = (string)input.GetType().GetProperty("TermBy")?.GetValue(input);

            if (!string.IsNullOrWhiteSpace(term) && !string.IsNullOrWhiteSpace(termBy))
            {
                var termByList = termBy.Split(',').ToList();

                if (termByList != null && termByList.Count > 0)
                {
                    var colsConditions = termByList.Aggregate("", (current, p) => current + $"{(!string.IsNullOrWhiteSpace(current) ? " || " : " ")} x.{p.FirstCharToUpper()}.ToLower().Contains(\"{term}\")");

                    var condition = $"x => {colsConditions}";

                    qry = qry.Where(condition);
                }
            }

            return qry;
        }


        /// <summary>
        ///  this function reads the Filters from input and apply the filter on the query
        /// </summary>
        private static IQueryable<T> ApplyFilters<T, TInput>(this IQueryable<T> qry, TInput input)
        {
            var filtersStr = (string)input.GetType().GetProperty("Filters")?.GetValue(input);

            var filters = JsonConvert.DeserializeObject<List<string[]>>(filtersStr ?? "[]") ??
                          new List<string[]>();

            if (filters != null && filters.Count > 0)
            {
                filters.ForEach(p =>
                {
                    var FILTER_NAME = p[0];
                    var FILTER_TYPE = p[1];
                    var FILTER_VALUE =
                            FILTER_TYPE == PropSearchFilterType.IsNull ||
                            FILTER_TYPE == PropSearchFilterType.IsNotNull ? "" : p[2];

                    switch (FILTER_TYPE)
                    {
                        case PropSearchFilterType.Contains:
                            qry = qry.Where($"x=> x.{FILTER_NAME}.ToLower().Contains(\"{FILTER_VALUE.ToLower()}\")");
                            break;

                        case PropSearchFilterType.NotContains:
                            qry = qry.Where($"x=> !x.{FILTER_NAME}.ToLower().Contains(\"{FILTER_VALUE.ToLower()}\")");
                            break;

                        case PropSearchFilterType.StartWith:
                            qry = qry.Where($"x=> x.{FILTER_NAME}.ToLower().StartsWith(\"{FILTER_VALUE.ToLower()}\")");
                            break;

                        case PropSearchFilterType.EndsWith:
                            qry = qry.Where($"x=> x.{FILTER_NAME}.ToLower().EndsWith(\"{FILTER_VALUE.ToLower()}\")");
                            break;

                        case PropSearchFilterType.Equal:
                            qry = qry.Where($"x=> x.{FILTER_NAME}.ToLower() == \"{FILTER_VALUE.ToLower()}\"");
                            break;

                        case PropSearchFilterType.NotEqual:
                            qry = qry.Where($"x=> x.{FILTER_NAME}.ToLower() != \"{FILTER_VALUE.ToLower()}\"");
                            break;

                        case PropSearchFilterType.LessThan:
                            qry = qry.Where($"x=> x.{FILTER_NAME}.ToLower() < \"{FILTER_VALUE.ToLower()}\"");
                            break;

                        case PropSearchFilterType.LessThanOrEqual:
                            qry = qry.Where($"x=> x.{FILTER_NAME}.ToLower() <= \"{FILTER_VALUE.ToLower()}\"");
                            break;

                        case PropSearchFilterType.GreaterThan:
                            qry = qry.Where($"x=> x.{FILTER_NAME}.ToLower() > \"{FILTER_VALUE.ToLower()}\"");
                            break;

                        case PropSearchFilterType.GreaterThanOrEqual:
                            qry = qry.Where($"x=> x.{FILTER_NAME}.ToLower() >= \"{FILTER_VALUE.ToLower()}\"");
                            break;

                        case PropSearchFilterType.IsNull:
                            qry = qry.Where($"x=> x.{FILTER_NAME} == null || x.{FILTER_NAME}.TrimEnd().TrimStart() == \"\" ");
                            break;

                        case PropSearchFilterType.IsNotNull:
                            qry = qry.Where($"x=> x.{FILTER_NAME} != null");
                            break;
                    }
                });
            }

            return qry;
        }
    }
}
