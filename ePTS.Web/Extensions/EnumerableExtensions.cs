using ePTS.Data;
using ePTS.Entities.Reference;
using ePTS.Models.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Collections;

namespace ePTS.Web.Extensions
{
    public static class EnumerableExtensions
    {
        public static IEnumerable<T> SelectRecursive<T>(this IEnumerable<T> source, Func<T, IEnumerable<T>> selector)
        {
            foreach (var parent in source)
            {
                yield return parent;

                var children = selector(parent);
                foreach (var child in SelectRecursive(children, selector))
                    yield return child;
            }
        }

        public static IEnumerable<RefLocation> ListLocations(IEnumerable<RefLocation> locations, string id)
        {
            var current = locations.Where(n => n.RefLocationId == id).FirstOrDefault();
            if (current == null)
                return Enumerable.Empty<RefLocation>();

            return Enumerable.Concat(new[] { current }, ListLocations(locations, current.ParentLocationId!));
        }
    }
}
